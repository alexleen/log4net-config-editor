// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Editor.ConfigProperties.Interfaces;
using Editor.Interfaces;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;
using Microsoft.Win32;

namespace Editor.Windows.Appenders
{
    /// <summary>
    /// Interaction logic for AddFileAppenderWindow.xaml
    /// </summary>
    public abstract partial class AppenderWindow : IMessageBoxService
    {
        private readonly IAppenderConfiguration mAppenderConfiguration;
        private readonly IPropertyDefinition mPropertyDefinition;

        public ObservableCollection<IProperty> AppenderProperties { get; }

        protected AppenderWindow(Window owner, IAppenderConfiguration appenderConfiguration, IPropertyDefinition propertyDefinition)
            : base("AppenderWindowPlacement")
        {
            InitializeComponent();
            DataContext = this;

            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            mAppenderConfiguration = appenderConfiguration;
            mPropertyDefinition = propertyDefinition;
            AppenderProperties = new ObservableCollection<IProperty>();
            Loaded += WindowOnLoaded;
        }

        private void WindowOnLoaded(object sender, EventArgs e)
        {
            //TODO combine initialize and load to better facilitate dymanic properties?
            mPropertyDefinition.Initialize();

            foreach (IProperty property in mPropertyDefinition.Properties)
            {
                AppenderProperties.Add(property);
            }

            if (mAppenderConfiguration.OriginalAppenderNode == null)
            {
                return;
            }

            // Load may add additional properties - hence the for loop and not foreach
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int index = 0; index < AppenderProperties.Count; index++)
            {
                AppenderProperties[index].Load(mAppenderConfiguration.OriginalAppenderNode);
            }
        }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            if (AppenderProperties.Any(prop => !prop.TryValidate(this)))
            {
                return;
            }

            //TODO add this as a property?
            mAppenderConfiguration.NewAppenderNode.AppendAttribute(mAppenderConfiguration.ConfigXml, "type", Descriptor.TypeNamespace);

            foreach (IProperty appenderProperty in AppenderProperties)
            {
                appenderProperty.Save(mAppenderConfiguration.ConfigXml, mAppenderConfiguration.NewAppenderNode);
            }

            if (mAppenderConfiguration.OriginalAppenderNode == null)
            {
                //New appender
                mAppenderConfiguration.Log4NetNode.AppendChild(mAppenderConfiguration.NewAppenderNode);
            }
            else
            {
                //Editing appender - replace
                mAppenderConfiguration.Log4NetNode.ReplaceChild(mAppenderConfiguration.NewAppenderNode, mAppenderConfiguration.OriginalAppenderNode);
            }

            Close();
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public void ShowError(string message)
        {
            MessageBox.Show(this, message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowInformation(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButton.OK, MessageBoxImage.Information);
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
    }
}
