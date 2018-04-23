// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Xml;
using Editor.Models;
using Editor.Utilities;

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
            IEnumerable<string> levelNames = new[] { string.Empty }.Concat(Log4NetUtilities.LevelsByName.Keys);

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

            if (mFilterModel.Node == null) //New filter
            {
                XmlElement filterElement = mConfigXml.CreateElementWithAttribute("filter", "type", mFilterModel.Descriptor.TypeNamespace);
                mAppenderNode.AppendChild(filterElement);
                mFilterModel.Node = filterElement;
            }

            Save(mConfigXml, mFilterModel.Node);
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

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
