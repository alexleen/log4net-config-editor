// Copyright © 2018 Alex Leendertsen

using System.Collections;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Editor.Properties;

namespace Editor.Windows
{
    /// <summary>
    /// Interaction logic for RefsWindow.xaml
    /// </summary>
    public partial class RefsWindow : Window
    {
        private readonly XmlDocument mConfigXml;
        private readonly XmlNode mLog4NetNode;
        private readonly string mAppenderName;

        public RefsWindow(Window owner, XmlDocument configXml, XmlNode log4NetNode, string appenderName)
        {
            InitializeComponent();
            Owner = owner;
            Title = $"refs for '{appenderName}'";
            mConfigXml = configXml;
            mLog4NetNode = log4NetNode;
            mAppenderName = appenderName;
            LoadRefs();
        }

        private void LoadRefs()
        {
            xRefsGrid.ItemsSource = FindAppenderRefs(mLog4NetNode, mAppenderName);
        }

        private static IEnumerable FindAppenderRefs(XmlNode log4NetNode, string appenderName)
        {
            XmlNodeList appenderRefs = log4NetNode.SelectNodes("//appender-ref");

            foreach (XmlNode appenderRef in appenderRefs)
            {
                if (appenderRef.Attributes?["ref"].Value == appenderName)
                {
                    yield return new RefModel(appenderRef);
                }
            }
        }

        private class RefModel
        {
            public RefModel(XmlNode appenderRef)
            {
                AppenderRef = appenderRef;
                ElementName = appenderRef.ParentNode.Name;
                Name = appenderRef.ParentNode.Attributes?["name"]?.Value;
                AppenderType = appenderRef.ParentNode.Attributes?["type"]?.Value;
            }

            public XmlNode AppenderRef { get; }
            public string ElementName { get; }
            public string Name { get; }
            public string AppenderType { get; }
        }

        private void RemoveRefOnClick(object sender, RoutedEventArgs e)
        {
            RefModel refModel = (RefModel)((Button)sender).DataContext;
            refModel.AppenderRef.ParentNode.RemoveChild(refModel.AppenderRef);
            mConfigXml.Save(Settings.Default.LastConfig);
            LoadRefs();
        }
    }
}
