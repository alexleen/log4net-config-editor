// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;
using log4net.Core;

namespace Editor.Windows.Appenders.Properties
{
    public class Mapping : PropertyBase
    {
        private const string MappingName = "mapping";
        private const string LevelName = "level";
        private const string ForeColorName = "foreColor";
        private const string BackColorName = "backColor";

        private readonly Window mOwner;

        public Mapping(Window owner, ObservableCollection<IProperty> container)
            : base(container, new GridLength(1, GridUnitType.Star))
        {
            mOwner = owner;
            Mappings = new ObservableCollection<MappingModel>();
            Add = new Command(ShowMappingWindowForAdd);
        }

        public ObservableCollection<MappingModel> Mappings { get; }

        public ICommand Add { get; }

        private void ShowMappingWindowForAdd()
        {
            MappingWindow mappingWindow = new MappingWindow { Owner = mOwner };
            mappingWindow.ShowDialog();

            if (!mappingWindow.Result.Equals(default))
            {
                Mappings.Add(new MappingModel(mOwner, mappingWindow.Result.Level, mappingWindow.Result.ForeColor, mappingWindow.Result.BackColor, OnRemove, OnReplace));
            }
        }

        private void OnReplace(MappingModel oldModel, MappingModel newModel)
        {
            int index = Mappings.IndexOf(oldModel);
            Mappings.RemoveAt(index);
            Mappings.Insert(index, newModel);
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
                string level = mappingNode.GetValueAttributeValueFromChildElement(LevelName);
                string foreColor = mappingNode.GetValueAttributeValueFromChildElement(ForeColorName);
                string backColor = mappingNode.GetValueAttributeValueFromChildElement(BackColorName);

                if (string.IsNullOrEmpty(level) || !Log4NetUtilities.TryParseLevel(level, out Level parsedLevel))
                {
                    continue;
                }

                ConsoleColor? foreColorEnum = null;
                if (Enum.TryParse(foreColor, out ConsoleColor foreParsed))
                {
                    foreColorEnum = foreParsed;
                }

                ConsoleColor? backColorEnum = null;
                if (Enum.TryParse(backColor, out ConsoleColor backParsed))
                {
                    backColorEnum = backParsed;
                }

                Mappings.Add(new MappingModel(mOwner, parsedLevel, foreColorEnum, backColorEnum, OnRemove, OnReplace));
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            foreach (MappingModel mappingModel in Mappings)
            {
                XmlElement mappingElement = xmlDoc.CreateElement(MappingName);
                xmlDoc.CreateElementWithValueAttribute(LevelName, mappingModel.Level.Name).AppendTo(mappingElement);

                if (mappingModel.ForeColor != null)
                {
                    xmlDoc.CreateElementWithValueAttribute(ForeColorName, mappingModel.ForeColor.ToString()).AppendTo(mappingElement);
                }

                if (mappingModel.BackColor != null)
                {
                    xmlDoc.CreateElementWithValueAttribute(BackColorName, mappingModel.BackColor.ToString()).AppendTo(mappingElement);
                }

                mappingElement.AppendTo(newNode);
            }
        }
    }
}
