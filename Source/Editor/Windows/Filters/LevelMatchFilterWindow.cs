// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows;
using System.Xml;
using Editor.Models;
using Editor.Utilities;

namespace Editor.Windows.Filters
{
    public class LevelMatchFilterWindow : FilterWindowBase
    {
        private const string LevelMatchName = "levelToMatch";

        public LevelMatchFilterWindow(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml, Action<FilterModel> add)
            : base(owner, filterModel, appenderNode, configXml, add)
        {
        }

        protected override void Configure()
        {
            GridLength zeroGridLength = new GridLength(0);
            xLoggerToMatchRow.Height = zeroGridLength;
            xMinLevelRow.Height = zeroGridLength;
            xMaxLevelRow.Height = zeroGridLength;
            xRegexToMatchRow.Height = zeroGridLength;
            xStringToMatchRow.Height = zeroGridLength;
        }

        protected override void Load(XmlNode filterNode)
        {
            xLevelToMatchComboBox.SelectedItem = filterNode.GetValueAttributeValueFromChildElement(LevelMatchName);
        }

        protected override bool TryValidateInputs()
        {
            //A value is always selected (xLevelToMatchComboBox)
            return true;
        }

        protected override void Save(XmlDocument configXml, XmlNode filterNode)
        {
            XmlElement levelToMatchElement = configXml.CreateElementWithValueAttribute(LevelMatchName, (string)xLevelToMatchComboBox.SelectedItem);
            XmlUtilities.AddOrUpdate(filterNode, levelToMatchElement);
        }
    }
}
