// Copyright © 2020 Alex Leendertsen

using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.Windows
{
    /// <summary>
    /// Interaction logic for ElementWindow.xaml
    /// </summary>
    internal partial class ElementWindow
    {
        private readonly IMessageBoxService mMessageBoxService;
        private readonly IElementConfiguration mConfiguration;
        private readonly ISaveStrategy mSaveStrategy;

        public IElementDefinition PropertyDefinition { get; }

        public ElementWindow(IElementConfiguration appenderConfiguration,
                             IElementDefinition propertyDefinition,
                             IWindowSizeLocation windowSizeLocation,
                             ISaveStrategy saveStrategy)
            : base(windowSizeLocation.RetentionKey)
        {
            InitializeComponent();
            DataContext = this;
            mMessageBoxService = new MessageBoxService(this);

            mConfiguration = appenderConfiguration;
            mSaveStrategy = saveStrategy;
            SetWindowSizeLocation(windowSizeLocation);
            PropertyDefinition = propertyDefinition;
            PropertyDefinition.MessageBoxService = mMessageBoxService;
            Loaded += WindowOnLoaded;
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
            for (int index = 0; index < PropertyDefinition.Properties.Count; index++)
            {
                PropertyDefinition.Properties[index].Load(mConfiguration.OriginalNode);
            }
        }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            if (PropertyDefinition.Properties.Any(prop => !prop.TryValidate(mMessageBoxService)))
            {
                return;
            }

            foreach (IProperty appenderProperty in PropertyDefinition.Properties)
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
    }
}
