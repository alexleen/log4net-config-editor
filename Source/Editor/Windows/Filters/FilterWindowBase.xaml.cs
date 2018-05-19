// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;

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
        private readonly Action<FilterModel> mAdd;

        public ObservableCollection<IAppenderProperty> FilterProperties { get; }

        protected FilterWindowBase(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml, Action<FilterModel> add)
        {
            InitializeComponent();
            DataContext = this;
            Owner = owner;
            mFilterModel = filterModel;
            mAppenderNode = appenderNode;
            mConfigXml = configXml;
            mAdd = add;
            FilterProperties = new ObservableCollection<IAppenderProperty>();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            AddAppropriateProperties();

            if (mFilterModel.Node == null)
            {
                return;
            }

            foreach (IAppenderProperty filterProperty in FilterProperties)
            {
                filterProperty.Load(mFilterModel.Node);
            }
        }

        /// <summary>
        /// Configure the grid to display the appropriate filter properties.
        /// </summary>
        protected abstract void AddAppropriateProperties();

        private void SaveOnClick(object sender, RoutedEventArgs e)
        {
            if (FilterProperties.Any(prop => !prop.TryValidate()))
            {
                return;
            }

            XmlNode filterElement = mConfigXml.CreateElementWithAttribute("filter", "type", mFilterModel.Descriptor.TypeNamespace);

            foreach (IAppenderProperty filterProperty in FilterProperties)
            {
                filterProperty.Save(mConfigXml, filterElement);
            }

            mAppenderNode.AppendChild(filterElement);
            mFilterModel.Node = filterElement;

            if (mFilterModel.Node == null)
            {
                mAdd(mFilterModel);
            }

            Close();
        }

        private void CloseOnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
