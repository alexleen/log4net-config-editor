// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Xml;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;
using Microsoft.Win32;

namespace Editor.Windows.Filters
{
    /// <summary>
    /// Interaction logic for FilterWindow.xaml
    /// </summary>
    public abstract partial class FilterWindowBase : IMessageBoxService
    {
        protected const int TextBoxWindowMinWidth = 350;
        private readonly FilterModel mFilterModel;
        private readonly XmlNode mAppenderNode;
        private readonly XmlDocument mConfigXml;
        private readonly Action<FilterModel> mAdd;

        public ObservableCollection<IProperty> FilterProperties { get; }

        protected FilterWindowBase(FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml, Action<FilterModel> add)
        {
            InitializeComponent();
            DataContext = this;
            mFilterModel = filterModel;
            mAppenderNode = appenderNode;
            mConfigXml = configXml;
            mAdd = add;
            FilterProperties = new ObservableCollection<IProperty>();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            AddAppropriateProperties();

            if (mFilterModel.Node == null)
            {
                return;
            }

            foreach (IProperty filterProperty in FilterProperties)
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
            if (FilterProperties.Any(prop => !prop.TryValidate(this)))
            {
                return;
            }

            XmlNode filterElement = mConfigXml.CreateElementWithAttribute("filter", "type", mFilterModel.Descriptor.TypeNamespace);

            foreach (IProperty filterProperty in FilterProperties)
            {
                filterProperty.Save(mConfigXml, filterElement);
            }

            mAppenderNode.AppendChild(filterElement);

            if (mFilterModel.Node == null)
            {
                mAdd(mFilterModel);
            }

            mFilterModel.Node = filterElement;

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
