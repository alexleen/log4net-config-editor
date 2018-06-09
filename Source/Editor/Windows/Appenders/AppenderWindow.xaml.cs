// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;
using Editor.Descriptors;
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
        protected readonly XmlDocument ConfigXml;
        protected readonly XmlNode Log4NetNode;

        /// <summary>
        /// Original appender node being edited.
        /// Null if new appender.
        /// </summary>
        protected readonly XmlNode OriginalAppenderNode;

        /// <summary>
        /// New appender node.
        /// This will be used to overwrite the <see cref="OriginalAppenderNode"/>, if it exists.
        /// </summary>
        protected readonly XmlNode NewAppenderNode;

        public ObservableCollection<IProperty> AppenderProperties { get; }

        protected AppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode)
            : base("AppenderWindowPlacement")
        {
            InitializeComponent();
            DataContext = this;

            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
            ConfigXml = configXml ?? throw new ArgumentNullException(nameof(configXml));
            Log4NetNode = log4NetNode ?? throw new ArgumentNullException(nameof(log4NetNode));
            OriginalAppenderNode = appenderNode;
            NewAppenderNode = ConfigXml.CreateElement("appender");
            AppenderProperties = new ObservableCollection<IProperty>();
            Loaded += WindowOnLoaded;
        }

        private void WindowOnLoaded(object sender, EventArgs e)
        {
            AddAppropriateProperties();

            if (OriginalAppenderNode == null)
            {
                return;
            }

            // Load may add additional properties - hence the for loop and not foreach
            // ReSharper disable once ForCanBeConvertedToForeach
            for (int index = 0; index < AppenderProperties.Count; index++)
            {
                AppenderProperties[index].Load(OriginalAppenderNode);
            }
        }

        protected abstract void AddAppropriateProperties();

        protected abstract AppenderDescriptor Descriptor { get; }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            if (AppenderProperties.Any(prop => !prop.TryValidate(this)))
            {
                return;
            }

            //TODO add this as a property?
            NewAppenderNode.AppendAttribute(ConfigXml, "type", Descriptor.TypeNamespace);

            foreach (IProperty appenderProperty in AppenderProperties)
            {
                appenderProperty.Save(ConfigXml, NewAppenderNode);
            }

            if (OriginalAppenderNode == null)
            {
                //New appender
                Log4NetNode.AppendChild(NewAppenderNode);
            }
            else
            {
                //Editing appender - replace
                Log4NetNode.ReplaceChild(NewAppenderNode, OriginalAppenderNode);
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
