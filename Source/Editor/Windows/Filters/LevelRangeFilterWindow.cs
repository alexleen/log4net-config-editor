// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;
using Editor.Models;

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
            bool mavValueChosen = !string.IsNullOrEmpty((string)xMaxLevelComboBox.SelectedItem);

            if (minValueChosen || mavValueChosen)
            {
                return true;
            }

            MessageBox.Show(this, "At least one value must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            return false;
        }

        protected override void Save(XmlDocument configXml, XmlNode filterNode)
        {
            //TODO these comboboxes can be left "empty"
            XmlAttribute levelMinValueAttr = configXml.CreateAttribute("value");
            levelMinValueAttr.Value = (string)xMinLevelComboBox.SelectedItem;

            XmlElement levelMinElement = configXml.CreateElement("levelMin");
            levelMinElement.Attributes.Append(levelMinValueAttr);

            AddOrUpdate(filterNode, levelMinElement);

            XmlAttribute levelMaxValueAttr = configXml.CreateAttribute("value");
            levelMaxValueAttr.Value = (string)xMaxLevelComboBox.SelectedItem;

            XmlElement levelMaxElement = configXml.CreateElement("levelMax");
            levelMaxElement.Attributes.Append(levelMaxValueAttr);

            AddOrUpdate(filterNode, levelMaxElement);
        }
    }
}
