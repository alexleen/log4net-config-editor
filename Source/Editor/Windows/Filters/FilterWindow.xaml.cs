// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Windows;
using System.Xml;
using Editor.Models;
using log4net.Core;

namespace Editor.Windows.Filters
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public abstract partial class FilterWindowBase : Window
    {
        private readonly FilterModel mFilterModel;
        private readonly XmlNode mAppenderNode;
        private readonly XmlDocument mConfigXml;

        private readonly IDictionary<string, Level> mLevelsByName = new Dictionary<string, Level>
        {
            { Level.Off.Name, Level.Off },
            { Level.Debug.Name, Level.Debug },
            { Level.Info.Name, Level.Info },
            { Level.Notice.Name, Level.Notice },
            { Level.Warn.Name, Level.Warn },
            { Level.Error.Name, Level.Error },
            { Level.Severe.Name, Level.Severe },
            { Level.Critical.Name, Level.Critical },
            { Level.Alert.Name, Level.Alert },
            { Level.Fatal.Name, Level.Fatal },
            { Level.Emergency.Name, Level.Emergency },
            { Level.All.Name, Level.All }
        };

        protected FilterWindowBase(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml)
        {
            InitializeComponent();
            Owner = owner;
            mFilterModel = filterModel;
            mAppenderNode = appenderNode;
            mConfigXml = configXml;
            PopulateComboBoxes();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            Configure();

            if (mFilterModel.Node != null)
            {
                Load(mFilterModel.Node);
            }
        }

        private void PopulateComboBoxes()
        {
            IEnumerable<string> levelNames = mLevelsByName.Keys;

            xLevelToMatchComboBox.ItemsSource = levelNames;
            xMinLevelComboBox.ItemsSource = levelNames;
            xMaxLevelComboBox.ItemsSource = levelNames;
        }

        protected abstract void Configure();

        /// <summary>
        /// Loads necessary information from the specified node.        
        /// </summary>
        /// <param name="filterNode">Guaranteed non-null (not called if null)</param>
        protected abstract void Load(XmlNode filterNode);

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            if (!TryValidateInputs())
            {
                return;
            }
            
            XmlNode filterNode;

            if (mFilterModel.Node == null) //New filter
            {
                XmlAttribute filterTypeAttribute = mConfigXml.CreateAttribute("type");
                filterTypeAttribute.Value = mFilterModel.Descriptor.TypeNamespace;

                XmlElement filterElement = mConfigXml.CreateElement("filter");
                filterElement.Attributes.Append(filterTypeAttribute);

                mAppenderNode.AppendChild(filterElement);

                filterNode = filterElement;
            }
            else
            {
                filterNode = mFilterModel.Node;
            }

            Save(mConfigXml, filterNode);
            Close();
        }

        /// <summary>
        /// Validates necessary inputs.
        /// Returns true if inputs are successfully validated.
        /// False otherwise.
        /// </summary>
        /// <returns></returns>
        protected abstract bool TryValidateInputs();

        /// <summary>
        /// Saves filter to configuration.
        /// </summary>
        /// <param name="configXml"></param>
        /// <param name="filterNode"></param>
        protected abstract void Save(XmlDocument configXml, XmlNode filterNode);

        protected static void AddOrUpdate(XmlNode filterNode, XmlNode filterChild)
        {
            XmlNode childMatch = filterNode.SelectSingleNode(filterChild.Name);

            if (childMatch == null)
            {
                filterNode.AppendChild(filterChild);
            }
            else
            {
                childMatch.Attributes?.RemoveAll();

                foreach (XmlAttribute childNodeAttribute in filterChild.Attributes)
                {
                    childMatch.Attributes.Append(childNodeAttribute);
                }
            }
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
