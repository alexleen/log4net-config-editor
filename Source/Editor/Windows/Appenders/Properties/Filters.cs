// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows.Filters;

namespace Editor.Windows.Appenders.Properties
{
    public class Filters : AppenderPropertyBase
    {
        private readonly Window mOwner;
        private readonly XmlDocument mXmlDoc;
        private readonly XmlNode mAppenderNode;

        public Filters(Window owner, XmlDocument xmlDoc, XmlNode appenderNode, ObservableCollection<IAppenderProperty> container)
            : base(container, new GridLength(1, GridUnitType.Star))
        {
            mOwner = owner;
            mXmlDoc = xmlDoc;
            mAppenderNode = appenderNode;

            AvailableFilters = new[]
            {
                FilterDescriptor.DenyAll,
                FilterDescriptor.LevelMatch,
                FilterDescriptor.LevelRange,
                FilterDescriptor.LoggerMatch,
                FilterDescriptor.String
            };

            ExistingFilters = new ObservableCollection<FilterModel>();
        }

        public IEnumerable<FilterDescriptor> AvailableFilters { get; }

        public FilterDescriptor SelectedFilter { get; set; }

        public ObservableCollection<FilterModel> ExistingFilters { get; }

        private void EditFilterOnClick(object sender, RoutedEventArgs e)
        {
            FilterModel filterModel = (FilterModel)((Button)sender).DataContext;
            ShowFilterWindow(filterModel);
        }

        private void RemoveFilterOnClick(object sender, RoutedEventArgs e)
        {
            FilterModel filterModel = (FilterModel)((Button)sender).DataContext;
            ExistingFilters.Remove(filterModel);
        }

        private void AddFilterOnClick(object sender, RoutedEventArgs e)
        {
            FilterModel filterModel;

            if (SelectedFilter != FilterDescriptor.DenyAll)
            {
                filterModel = new FilterModel(SelectedFilter, null);
                ShowFilterWindow(filterModel);
            }
            else
            {
                XmlElement filterElement = mXmlDoc.CreateElementWithAttribute("filter", "type", FilterDescriptor.DenyAll.TypeNamespace);
                filterModel = new FilterModel(FilterDescriptor.DenyAll, filterElement);
            }

            ExistingFilters.Add(filterModel);
        }

        private void ShowFilterWindow(FilterModel filterModel)
        {
            Window filterWindow = null;

            switch (filterModel.Descriptor.Type)
            {
                case FilterType.LevelMatch:
                    filterWindow = new LevelMatchFilterWindow(mOwner, filterModel, mAppenderNode, mXmlDoc);
                    break;
                case FilterType.LevelRange:
                    filterWindow = new LevelRangeFilterWindow(mOwner, filterModel, mAppenderNode, mXmlDoc);
                    break;
                case FilterType.LoggerMatch:
                    filterWindow = new LoggerMatchFilterWindow(mOwner, filterModel, mAppenderNode, mXmlDoc);
                    break;
                case FilterType.Mdc:
                    break;
                case FilterType.Ndc:
                    break;
                case FilterType.Property:
                    break;
                case FilterType.String:
                    filterWindow = new StringMatchFilterWindow(mOwner, filterModel, mAppenderNode, mXmlDoc);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            filterWindow?.ShowDialog();
        }

        private void MoveFilterUpOnClick(object sender, RoutedEventArgs e)
        {
            FilterModel filterModel = (FilterModel)((Button)sender).DataContext;
            int oldIndex = ExistingFilters.IndexOf(filterModel);
            int newIndex = oldIndex == 0 ? 0 : oldIndex - 1;
            ExistingFilters.Move(oldIndex, newIndex);
        }

        private void MoveFilterDownOnClick(object sender, RoutedEventArgs e)
        {
            FilterModel filterModel = (FilterModel)((Button)sender).DataContext;
            int oldIndex = ExistingFilters.IndexOf(filterModel);
            int newIndex = oldIndex == ExistingFilters.Count - 1 ? ExistingFilters.Count - 1 : oldIndex + 1;
            ExistingFilters.Move(oldIndex, newIndex);
        }

        private void HelpOnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Filters form a chain that the event has to pass through. Any filter along the way can accept the event and stop processing, deny the event and stop processing, or allow the event on to the next filter. If the event gets to the end of the filter chain without being denied it is implicitly accepted and will be logged. To reorder filters, click either '↑' or '↓'.",
                            "Help",
                            MessageBoxButton.OK,
                            MessageBoxImage.Information);
        }

        public override void Load(XmlNode appenderNode)
        {
            XmlNodeList filterNodes = appenderNode.SelectNodes("filter");

            if (filterNodes != null)
            {
                foreach (XmlNode filterNode in filterNodes)
                {
                    if (FilterDescriptor.TryFindByTypeNamespace(filterNode.Attributes?["type"]?.Value, out FilterDescriptor filterDescriptor))
                    {
                        ExistingFilters.Add(new FilterModel(filterDescriptor, filterNode));
                    }
                }
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode appenderNode)
        {
            foreach (FilterModel filter in ExistingFilters)
            {
                appenderNode.AppendChild(filter.Node);
            }
        }
    }
}
