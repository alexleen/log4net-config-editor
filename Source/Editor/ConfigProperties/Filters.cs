// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Definitions.Factory;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Models;
using Editor.SaveStrategies;
using Editor.Utilities;
using Editor.Windows;
using Editor.Windows.SizeLocation;
using Editor.XML;

namespace Editor.ConfigProperties
{
    public class Filters : PropertyBase
    {
        private readonly IConfiguration mConfiguration;
        private readonly IMessageBoxService mMessageBoxService;

        public Filters(IConfiguration configuration, IMessageBoxService messageBoxService)
            : base(new GridLength(1, GridUnitType.Star))
        {
            mConfiguration = configuration;
            mMessageBoxService = messageBoxService;

            AvailableFilters = new[]
            {
                FilterDescriptor.DenyAll,
                FilterDescriptor.LevelMatch,
                FilterDescriptor.LevelRange,
                FilterDescriptor.LoggerMatch,
                FilterDescriptor.String,
                FilterDescriptor.Property
            };

            ExistingFilters = new ObservableCollection<FilterModel>();

            AddFilter = new Command(AddFilterOnClick);
            Help = new Command(HelpOnClick);
        }

        public IEnumerable<FilterDescriptor> AvailableFilters { get; }

        public ObservableCollection<FilterModel> ExistingFilters { get; }

        public ICommand AddFilter { get; }

        public ICommand Help { get; }

        private void AddFilterOnClick(object filter)
        {
            FilterDescriptor descriptor = (FilterDescriptor)filter;

            if (descriptor != FilterDescriptor.DenyAll)
            {
                ShowFilterWindow(new FilterModel(descriptor, null, ShowFilterWindow, Remove, MoveUp, MoveDown));
            }
            else
            {
                XmlElement filterElement = mConfiguration.ConfigXml.CreateElementWithAttribute(Log4NetXmlConstants.Filter, Log4NetXmlConstants.Type, FilterDescriptor.DenyAll.TypeNamespace);
                Add(new FilterModel(FilterDescriptor.DenyAll, filterElement, ShowFilterWindow, Remove, MoveUp, MoveDown));
            }
        }

        private void HelpOnClick()
        {
            mMessageBoxService.ShowInformation("Filters form a chain that the event has to pass through. Any filter along the way can accept the event and stop processing, deny the event and stop processing, or allow the event on to the next filter. If the event gets to the end of the filter chain without being denied it is implicitly accepted and will be logged. To reorder filters, click either '↑' or '↓'.");
        }

        private void ShowFilterWindow(FilterModel filterModel)
        {
            XmlElement newFilter = mConfiguration.ConfigXml.CreateElementWithAttribute(Log4NetXmlConstants.Filter, Log4NetXmlConstants.Type, filterModel.Descriptor.TypeNamespace);

            IElementConfiguration configuration = new ElementConfiguration(mConfiguration, filterModel.Node, newFilter);

            ISaveStrategy saveStrategy = new AddSaveStrategy<FilterModel>(filterModel, Add, newFilter);

            Window filterWindow = new ElementWindow(configuration,
                                                    DefinitionFactory.Create(filterModel.Descriptor, configuration),
                                                    WindowSizeLocationFactory.Create(filterModel.Descriptor),
                                                    saveStrategy);

            mMessageBoxService.ShowWindow(filterWindow);
        }

        private void Add(FilterModel filterModel)
        {
            ExistingFilters.Add(filterModel);
        }

        private void Remove(FilterModel filterModel)
        {
            ExistingFilters.Remove(filterModel);
        }

        private void MoveUp(FilterModel filterModel)
        {
            int oldIndex = ExistingFilters.IndexOf(filterModel);
            int newIndex = oldIndex == 0 ? 0 : oldIndex - 1;
            ExistingFilters.Move(oldIndex, newIndex);
        }

        private void MoveDown(FilterModel filterModel)
        {
            int oldIndex = ExistingFilters.IndexOf(filterModel);
            int newIndex = oldIndex == ExistingFilters.Count - 1 ? ExistingFilters.Count - 1 : oldIndex + 1;
            ExistingFilters.Move(oldIndex, newIndex);
        }

        public override void Load(IElementConfiguration config)
        {
            XmlNodeList filterNodes = config.OriginalNode.SelectNodes(Log4NetXmlConstants.Filter);

            if (filterNodes == null)
            {
                return;
            }

            foreach (XmlNode filterNode in filterNodes)
            {
                if (FilterDescriptor.TryFindByTypeNamespace(filterNode.Attributes?[Log4NetXmlConstants.Type]?.Value, out FilterDescriptor filterDescriptor))
                {
                    Add(new FilterModel(filterDescriptor, filterNode, ShowFilterWindow, Remove, MoveUp, MoveDown));
                }
            }
        }

        public override void Save(IElementConfiguration config)
        {
            foreach (FilterModel filter in ExistingFilters)
            {
                config.NewNode.AppendChild(filter.Node);
            }
        }
    }
}
