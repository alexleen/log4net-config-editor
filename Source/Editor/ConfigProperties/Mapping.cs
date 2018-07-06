// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Definitions.Factory;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows;
using Editor.Windows.SizeLocation;

namespace Editor.ConfigProperties
{
    public class Mapping : PropertyBase
    {
        private const string MappingName = "mapping";

        private readonly IConfiguration mConfiguration;
        private readonly IMessageBoxService mMessageBoxService;

        public Mapping(ReadOnlyCollection<IProperty> container, IConfiguration configuration, IMessageBoxService messageBoxService)
            : base(container, new GridLength(1, GridUnitType.Star))
        {
            Mappings = new ObservableCollection<MappingModel>();
            Add = new Command(ShowMappingWindowForAdd);
            mConfiguration = configuration;
            mMessageBoxService = messageBoxService;
        }

        public ObservableCollection<MappingModel> Mappings { get; }

        public ICommand Add { get; }

        private void ShowMappingWindowForAdd()
        {
            ShowMappingWindow(new MappingModel(OnRemove, ShowMappingWindow));
        }

        private void ShowMappingWindow(MappingModel mappingModel)
        {
            XmlElement newMapping = mConfiguration.ConfigXml.CreateElement("mapping");

            IElementConfiguration elementConfiguration = new ElementConfiguration(mConfiguration, mappingModel.Node, newMapping);

            ElementWindow elementWindow = new ElementWindow(elementConfiguration,
                                                            DefinitionFactory.Create(MappingDescriptor.Mapping, elementConfiguration),
                                                            WindowSizeLocationFactory.Create(MappingDescriptor.Mapping),
                                                            new MappingSaveStrategy(mappingModel, model => Mappings.Add(model), newMapping));

            mMessageBoxService.ShowWindow(elementWindow);
        }

        private void OnRemove(MappingModel mappingModel)
        {
            Mappings.Remove(mappingModel);
        }

        public override void Load(XmlNode originalNode)
        {
            XmlNodeList mappings = originalNode.SelectNodes("mapping");

            foreach (XmlNode mappingNode in mappings)
            {
                Mappings.Add(new MappingModel(OnRemove, ShowMappingWindow, mappingNode));
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            foreach (MappingModel mappingModel in Mappings)
            {
                newNode.AppendChild(mappingModel.Node);
            }
        }

        private class MappingSaveStrategy : ISaveStrategy
        {
            private readonly MappingModel mMappingModel;
            private readonly Action<MappingModel> mAdd;
            private readonly XmlElement mNewMapping;

            public MappingSaveStrategy(MappingModel mappingModel, Action<MappingModel> add, XmlElement newMapping)
            {
                mMappingModel = mappingModel ?? throw new ArgumentNullException(nameof(mappingModel));
                mAdd = add ?? throw new ArgumentNullException(nameof(add));
                mNewMapping = newMapping ?? throw new ArgumentNullException(nameof(newMapping));
            }

            public void Execute()
            {
                if (mMappingModel.Node == null)
                {
                    mAdd(mMappingModel);
                }

                mMappingModel.Node = mNewMapping;
            }
        }
    }
}
