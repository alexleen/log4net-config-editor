// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Definitions.Factory;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows;
using Editor.Windows.Appenders;
using log4net.Core;
using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace Editor.ConfigProperties
{
    public class Mapping : PropertyBase
    {
        private const string MappingName = "mapping";
        private const string LevelName = "level";
        private const string ForeColorName = "foreColor";
        private const string BackColorName = "backColor";
        private readonly IConfiguration mConfiguration;

        public Mapping(ReadOnlyCollection<IProperty> container, IConfiguration configuration)
            : base(container, new GridLength(1, GridUnitType.Star))
        {
            Mappings = new ObservableCollection<MappingModel>();
            Add = new Command(ShowMappingWindowForAdd);
            mConfiguration = configuration;
        }

        public ObservableCollection<MappingModel> Mappings { get; }

        public ICommand Add { get; }

        private void ShowMappingWindowForAdd()
        {
            //MappingWindow mappingWindow = new MappingWindow();

            //mMessageBoxService.ShowWindow(mappingWindow);

            //if (!mappingWindow.Result.Equals(default))
            //{
            //    Mappings.Add(new MappingModel(mappingWindow.Result.Level, mappingWindow.Result.ForeColor, mappingWindow.Result.BackColor, OnRemove, OnReplace, mMessageBoxService));
            //}
            ShowMappingWindow(new MappingModel(null, OnRemove, ShowMappingWindow));
        }

        private void ShowMappingWindow(MappingModel mappingModel)
        {
            XmlElement newMapping = mConfiguration.ConfigXml.CreateElement("mapping");

            IElementConfiguration elementConfiguration = new ElementConfiguration(mConfiguration, mappingModel.Node, newMapping);

            ElementWindow elementWindow = new ElementWindow(elementConfiguration, 
                                          DefinitionFactory.Create(MappingDescriptor.Mapping, elementConfiguration), 
                                          WindowSizeLocationFactory.Create(MappingDescriptor.Mapping), 
                                          new MappingSaveStrategy(mappingModel, model => Mappings.Add(model), newMapping));
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
                //string level = mappingNode.GetValueAttributeValueFromChildElement(LevelName);
                //string foreColor = mappingNode.GetValueAttributeValueFromChildElement(ForeColorName);
                //string backColor = mappingNode.GetValueAttributeValueFromChildElement(BackColorName);

                //if (string.IsNullOrEmpty(level) || !Log4NetUtilities.TryParseLevel(level, out Level parsedLevel))
                //{
                //    continue;
                //}

                //ConsoleColor? foreColorEnum = null;
                //if (Enum.TryParse(foreColor, out ConsoleColor foreParsed))
                //{
                //    foreColorEnum = foreParsed;
                //}

                //ConsoleColor? backColorEnum = null;
                //if (Enum.TryParse(backColor, out ConsoleColor backParsed))
                //{
                //    backColorEnum = backParsed;
                //}

                Mappings.Add(new MappingModel(mappingNode, OnRemove, ShowMappingWindow));
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            foreach (MappingModel mappingModel in Mappings)
            {
                //XmlElement mappingElement = xmlDoc.CreateElement(MappingName);
                //xmlDoc.CreateElementWithValueAttribute(LevelName, mappingModel.Level.Name).AppendTo(mappingElement);

                //if (mappingModel.ForeColor != null)
                //{
                //    xmlDoc.CreateElementWithValueAttribute(ForeColorName, mappingModel.ForeColor.ToString()).AppendTo(mappingElement);
                //}

                //if (mappingModel.BackColor != null)
                //{
                //    xmlDoc.CreateElementWithValueAttribute(BackColorName, mappingModel.BackColor.ToString()).AppendTo(mappingElement);
                //}

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
