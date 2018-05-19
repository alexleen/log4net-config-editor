// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Filters.Properties
{
    public class MaxLevel : LevelPropertyBase
    {
        private const string LevelMaxName = "levelMax";

        public MaxLevel(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto, "Max Level:", true)
        {
        }

        public override void Load(XmlNode originalAppenderNode)
        {
            SelectedLevel = originalAppenderNode.GetValueAttributeValueFromChildElement(LevelMaxName);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            if (!string.IsNullOrEmpty(SelectedLevel))
            {
                xmlDoc.CreateElementWithValueAttribute(LevelMaxName, SelectedLevel).AppendTo(newAppenderNode);
            }
        }
    }
}
