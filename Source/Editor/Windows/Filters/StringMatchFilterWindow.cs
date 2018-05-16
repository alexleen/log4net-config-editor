// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows;
using System.Xml;
using Editor.Models;
using Editor.Utilities;

namespace Editor.Windows.Filters
{
    public class StringMatchFilterWindow : FilterWindowBase
    {
        private const string StringMatchName = "stringToMatch";

        public StringMatchFilterWindow(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml, Action<FilterModel> add)
            : base(owner, filterModel, appenderNode, configXml, add)
        {
            xStringToMatchTextBox.Focus();
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
            xStringToMatchTextBox.Text = filterNode.GetValueAttributeValueFromChildElement(StringMatchName);
        }

        protected override bool TryValidateInputs()
        {
            if (string.IsNullOrEmpty(xStringToMatchTextBox.Text))
            {
                MessageBox.Show(this, "'String to Match' must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        protected override void Save(XmlDocument configXml, XmlNode filterNode)
        {
            XmlElement stringToMatchElement = configXml.CreateElementWithValueAttribute(StringMatchName, xStringToMatchTextBox.Text);
            XmlUtilities.AddOrUpdate(filterNode, stringToMatchElement);
        }
    }
}
