// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Models;

namespace Editor.Windows.Filters
{
    public class LevelMatchFilterWindow : FilterWindowBase
    {
        public LevelMatchFilterWindow(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml)
            : base(owner, filterModel, appenderNode, configXml)
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
            string value = filterNode.SelectSingleNode("levelToMatch")?.Attributes?["value"]?.Value;
            xLevelToMatchComboBox.SelectedItem = value;
        }

        protected override bool TryValidateInputs()
        {
            //A value is always selected (xLevelToMatchComboBox)
            return true;
        }

        protected override void Save(XmlDocument configXml, XmlNode filterNode)
        {
            XmlAttribute valueAttr = configXml.CreateAttribute("value");
            valueAttr.Value = (string)xLevelToMatchComboBox.SelectedItem;

            XmlElement levelToMatchElement = configXml.CreateElement("levelToMatch");
            levelToMatchElement.Attributes.Append(valueAttr);

            AddOrUpdate(filterNode, levelToMatchElement);
        }
    }
}
