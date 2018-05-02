// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Appenders
{
    /// <summary>
    /// Interaction logic for AddFileAppenderWindow.xaml
    /// </summary>
    public abstract partial class AppenderWindow : Window
    {
        public static readonly GridLength AutoGridRowHeight = GridLength.Auto;

        protected readonly XmlDocument ConfigXml;
        protected readonly XmlNode Log4NetNode;

        /// <summary>
        /// Appender node this window is referencing.
        /// Never null. Could be an existing appender for editing or it may be "empty" -
        /// i.e. a new new appender element with no attributes or children.
        /// </summary>
        protected readonly XmlNode AppenderNode;

        private readonly ChangeType mChangeType;

        public ObservableCollection<IAppenderProperty> AppenderProperties { get; }

        protected AppenderWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, XmlNode appenderNode, ChangeType changeType)
        {
            InitializeComponent();
            DataContext = this;

            Owner = owner ?? throw new ArgumentException(nameof(owner));
            ConfigXml = configXml ?? throw new ArgumentException(nameof(configXml));
            Log4NetNode = log4NetNode ?? throw new ArgumentException(nameof(log4NetNode));
            AppenderNode = appenderNode ?? throw new ArgumentException(nameof(appenderNode));
            mChangeType = changeType;
            AppenderProperties = new ObservableCollection<IAppenderProperty>();
        }

        private void WindowOnLoaded(object sender, EventArgs e)
        {
            AddAppropriateProperties();

            foreach (IAppenderProperty appenderProperty in AppenderProperties)
            {
                appenderProperty.Load();
            }
        }

        protected abstract void AddAppropriateProperties();

        protected abstract AppenderDescriptor Descriptor { get; }

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            if (AppenderProperties.Any(prop => !prop.TryValidate()))
            {
                return;
            }

            //If we're editing, just replace the existing appender with a new one
            XmlNode appenderNode = mChangeType == ChangeType.Add ? AppenderNode : ConfigXml.CreateElement("appender");

            //TODO add this as a property?
            appenderNode.AppendAttribute(ConfigXml, "type", Descriptor.TypeNamespace);

            foreach (IAppenderProperty appenderProperty in AppenderProperties)
            {
                appenderProperty.Save(ConfigXml);
            }

            if (mChangeType == ChangeType.Add)
            {
                Log4NetNode.AppendChild(appenderNode);
            }
            else
            {
                Log4NetNode.ReplaceChild(appenderNode, AppenderNode);
            }

            Close();
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
