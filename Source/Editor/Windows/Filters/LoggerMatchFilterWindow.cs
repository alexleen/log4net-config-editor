// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Models;

namespace Editor.Windows.Filters
{
    public class LoggerMatchFilterWindow : FilterWindowBase
    {
        public LoggerMatchFilterWindow(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml)
            : base(owner, filterModel, appenderNode, configXml)
        {
        }

        protected override void Configure()
        {
            GridLength zeroGridLength = new GridLength(0);
            xLevelToMatchRow.Height = zeroGridLength;
            xMinLevelRow.Height = zeroGridLength;
            xMaxLevelRow.Height = zeroGridLength;
            xRegexToMatchRow.Height = zeroGridLength;
            xStringToMatchRow.Height = zeroGridLength;
        }

        protected override void Load(XmlNode filterNode)
        {
            string value = filterNode.SelectSingleNode("loggerToMatch")?.Attributes?["value"]?.Value;
            xLoggerToMatchTextBox.Text = value;
        }

        protected override void Save(XmlDocument configXml, XmlNode filterNode)
        {
            XmlAttribute valueAttr = configXml.CreateAttribute("value");
            valueAttr.Value = xLoggerToMatchTextBox.Text;

            XmlElement loggerToMatchElement = configXml.CreateElement("loggerToMatch");
            loggerToMatchElement.Attributes.Append(valueAttr);

            AddOrUpdate(filterNode, loggerToMatchElement);
        }
    }
}
