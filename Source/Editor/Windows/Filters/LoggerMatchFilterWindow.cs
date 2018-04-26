// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Models;
using Editor.Utilities;

namespace Editor.Windows.Filters
{
    public class LoggerMatchFilterWindow : FilterWindowBase
    {
        public LoggerMatchFilterWindow(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml)
            : base(owner, filterModel, appenderNode, configXml)
        {
            xLoggerToMatchTextBox.Focus();
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

        protected override bool TryValidateInputs()
        {
            if (string.IsNullOrEmpty(xLoggerToMatchTextBox.Text))
            {
                MessageBox.Show(this, "'Logger to Match' must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        protected override void Save(XmlDocument configXml, XmlNode filterNode)
        {
            XmlElement loggerToMatchElement = configXml.CreateElementWithAttribute("loggerToMatch", "value", xLoggerToMatchTextBox.Text);
            XmlUtilities.AddOrUpdate(filterNode, loggerToMatchElement);
        }
    }
}
