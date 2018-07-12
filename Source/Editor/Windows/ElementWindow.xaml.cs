// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Editor.Interfaces;
using Microsoft.Win32;

namespace Editor.Windows
{
    /// <summary>
    /// Interaction logic for ElementWindow.xaml
    /// </summary>
    internal partial class ElementWindow : IMessageBoxService
    {
        private readonly IElementConfiguration mConfiguration;
        private readonly ISaveStrategy mSaveStrategy;

        public IWindowSizeLocation WindowSizeLocation { get; }
        public IElementDefinition PropertyDefinition { get; }
        public ObservableCollection<IProperty> Properties { get; }

        public ElementWindow(IElementConfiguration appenderConfiguration,
                             IElementDefinition propertyDefinition,
                             IWindowSizeLocation windowSizeLocation,
                             ISaveStrategy saveStrategy)
            : base(windowSizeLocation.RetentionKey)
        {
            InitializeComponent();
            DataContext = this;

            mConfiguration = appenderConfiguration;
            WindowSizeLocation = windowSizeLocation;
            mSaveStrategy = saveStrategy;
            SetWindowSizeLocation(windowSizeLocation);
            PropertyDefinition = propertyDefinition;
            PropertyDefinition.MessageBoxService = this;
            Properties = new ObservableCollection<IProperty>();
            Loaded += WindowOnLoaded;
            ((INotifyCollectionChanged)PropertyDefinition.Properties).CollectionChanged += PropertiesOnCollectionChanged;
            Icon = new BitmapImage(new Uri(PropertyDefinition.Icon));
        }

        private void SetWindowSizeLocation(IWindowSizeLocation windowSizeLocation)
        {
            ResizeMode = windowSizeLocation.ResizeMode;
            SizeToContent = windowSizeLocation.SizeToContent;
            Width = windowSizeLocation.Width.Value;
            MinWidth = windowSizeLocation.Width.Min;
            MaxWidth = windowSizeLocation.Width.Max;
            Height = windowSizeLocation.Height.Value;
            MinHeight = windowSizeLocation.Height.Min;
            MaxHeight = windowSizeLocation.Height.Max;
        }

        private void PropertiesOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (IProperty property in e.NewItems)
                {
                    Properties.Add(property);
                }
            }

            if (e.OldItems != null)
            {
                foreach (IProperty property in e.OldItems)
                {
                    Properties.Remove(property);
                }
            }
        }

        private void WindowOnLoaded(object sender, EventArgs e)
        {
            //This will add all properties to collection via event handler
            PropertyDefinition.Initialize();

            if (mConfiguration.OriginalNode == null)
            {
                return;
            }

            // Load may add additional properties - hence the for loop and not foreach
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int index = 0; index < Properties.Count; index++)
            {
                Properties[index].Load(mConfiguration.OriginalNode);
            }
        }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            if (Properties.Any(prop => !prop.TryValidate(this)))
            {
                return;
            }

            foreach (IProperty appenderProperty in Properties)
            {
                appenderProperty.Save(mConfiguration.ConfigXml, mConfiguration.NewNode);
            }

            mSaveStrategy.Execute();

            Close();
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        #region IMessageBoxService

        public void ShowError(string message)
        {
            MessageBox.Show(this, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowInformation(string message)
        {
            MessageBox.Show(this, message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public void ShowWindow(Window window)
        {
            window.Owner = this;
            window.ShowDialog();
        }

        public bool? ShowOpenFileDialog(out string fileName)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            bool? showDialog = ofd.ShowDialog();

            if (showDialog.HasValue && showDialog.Value)
            {
                fileName = ofd.FileName;
                return true;
            }

            fileName = null;
            return showDialog;
        }

        #endregion
    }
}
