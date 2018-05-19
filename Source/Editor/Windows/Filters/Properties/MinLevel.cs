// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Filters.Properties
{
    public class MinLevel : LevelPropertyBase
    {
        private const string LevelMinName = "levelMin";

        public MinLevel(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto, "Min Level:", true)
        {
        }

        public override void Load(XmlNode originalAppenderNode)
        {
            SelectedLevel = originalAppenderNode.GetValueAttributeValueFromChildElement(LevelMinName);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            if (!string.IsNullOrEmpty(SelectedLevel))
            {
                xmlDoc.CreateElementWithValueAttribute(LevelMinName, SelectedLevel).AppendTo(newAppenderNode);
            }
        }
    }
}
