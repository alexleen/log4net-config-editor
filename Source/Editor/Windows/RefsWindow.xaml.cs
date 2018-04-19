// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Editor.Models;
using Editor.Utilities;

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
            LoadAvailableRefLocations();
        }

        private void LoadRefs()
        {
            xRefsGrid.ItemsSource = XmlUtilities.FindAppenderRefs(mLog4NetNode, mAppenderName);
        }

        private void LoadAvailableRefLocations()
        {
            XmlNodeList appenderList = mLog4NetNode.SelectNodes("appender");
            ICollection<XmlNode> items = new List<XmlNode>();

            if (appenderList != null)
            {
                foreach (XmlNode node in appenderList)
                {
                    if (node.Attributes?["type"]?.Value == "Log4Net.Async.AsyncForwardingAppender,Log4Net.Async")
                    {
                        items.Add(node);
                    }
                }
            }

            XmlNode root = mLog4NetNode.SelectSingleNode("root");

            if (root != null)
            {
                items.Add(root);
            }

            xRefComboBox.ItemsSource = items;
        }

        private void RemoveRefOnClick(object sender, RoutedEventArgs e)
        {
            RefModel refModel = (RefModel)((Button)sender).DataContext;
            refModel.AppenderRef.ParentNode.RemoveChild(refModel.AppenderRef);
            LoadRefs();
        }

        private void AddRefOnClick(object sender, RoutedEventArgs e)
        {
            XmlAttribute refAttribute = mConfigXml.CreateAttribute("ref");
            refAttribute.Value = mAppenderName;

            XmlNode newAppenderRef = mConfigXml.CreateNode(XmlNodeType.Element, null, "appender-ref", null);
            newAppenderRef.Attributes.Append(refAttribute);

            XmlNode selected = (XmlNode)xRefComboBox.SelectionBoxItem;
            selected.AppendChild(newAppenderRef);

            LoadRefs();
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
