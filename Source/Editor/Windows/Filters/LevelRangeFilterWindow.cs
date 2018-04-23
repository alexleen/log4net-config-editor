// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Windows.Controls.Primitives;
using System.Xml;
using Editor.Models;
using Editor.Utilities;

namespace Editor.Windows.Filters
{
    public class LevelRangeFilterWindow : FilterWindowBase
    {
        public LevelRangeFilterWindow(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml)
            : base(owner, filterModel, appenderNode, configXml)
        {
        }

        protected override void Configure()
        {
            GridLength zeroGridLength = new GridLength(0);
            xLoggerToMatchRow.Height = zeroGridLength;
            xLevelToMatchRow.Height = zeroGridLength;
            xRegexToMatchRow.Height = zeroGridLength;
            xStringToMatchRow.Height = zeroGridLength;
        }

        protected override void Load(XmlNode filterNode)
        {
            string levelMin = filterNode.SelectSingleNode("levelMin")?.Attributes?["value"]?.Value;
            xMinLevelComboBox.SelectedItem = levelMin;

            string levelMax = filterNode.SelectSingleNode("levelMax")?.Attributes?["value"]?.Value;
            xMaxLevelComboBox.SelectedItem = levelMax;
        }

        protected override bool TryValidateInputs()
        {
            bool minValueChosen = !string.IsNullOrEmpty((string)xMinLevelComboBox.SelectedItem);
            bool maxValueChosen = !string.IsNullOrEmpty((string)xMaxLevelComboBox.SelectedItem);

            if (minValueChosen || maxValueChosen)
            {
                return true;
            }

            MessageBox.Show(this, "At least one value must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        protected override void Save(XmlDocument configXml, XmlNode filterNode)
        {
            Process(configXml, filterNode, xMinLevelComboBox, "levelMin");
            Process(configXml, filterNode, xMaxLevelComboBox, "levelMax");
        }

        private static void Process(XmlDocument configXml, XmlNode filterNode, Selector comboBox, string elementName)
        {
            bool valueChosen = !string.IsNullOrEmpty((string)comboBox.SelectedItem);

            if (valueChosen)
            {
                XmlElement levelElement = configXml.CreateElementWithAttribute(elementName, "value", (string)comboBox.SelectedItem);
                XmlUtilities.AddOrUpdate(filterNode, levelElement);
            }
            else
            {
                XmlNode existingLevel = filterNode.SelectSingleNode(elementName);

                if (existingLevel != null)
                {
                    filterNode.RemoveChild(existingLevel);
                }
            }
        }
    }
}
