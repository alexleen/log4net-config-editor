// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Models;

namespace Editor.Windows.Filters
{
    public class StringMatchFilterWindow : FilterWindowBase
    {
        public StringMatchFilterWindow(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml)
            : base(owner, filterModel, appenderNode, configXml)
        {
        }

        protected override void Configure()
        {
            GridLength zeroGridLength = new GridLength(0);
            xLoggerToMatchRow.Height = zeroGridLength;
            xLevelToMatchRow.Height = zeroGridLength;
            xMinLevelRow.Height = zeroGridLength;
            xMaxLevelRow.Height = zeroGridLength;
            xRegexToMatchRow.Height = zeroGridLength;
        }

        protected override void Load(XmlNode filterNode)
        {
            string value = filterNode.SelectSingleNode("stringToMatch")?.Attributes?["value"]?.Value;
            xStringToMatchTextBox.Text = value;
        }

        protected override void Save(XmlDocument configXml, XmlNode filterNode)
        {
            XmlAttribute valueAttr = configXml.CreateAttribute("value");
            valueAttr.Value = xStringToMatchTextBox.Text;

            XmlElement stringToMatchElement = configXml.CreateElement("stringToMatch");
            stringToMatchElement.Attributes.Append(valueAttr);

            AddOrUpdate(filterNode, stringToMatchElement);
        }
    }
}
