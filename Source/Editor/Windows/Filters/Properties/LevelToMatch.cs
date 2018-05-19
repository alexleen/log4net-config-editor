// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Filters.Properties
{
    public class LevelToMatch : LevelPropertyBase
    {
        private const string LevelMatchName = "levelToMatch";

        public LevelToMatch(ObservableCollection<IAppenderProperty> container)
            : base(container, GridLength.Auto, "Level to Match:")
        {
        }

        public override void Load(XmlNode originalAppenderNode)
        {
            SelectedLevel = originalAppenderNode.GetValueAttributeValueFromChildElement(LevelMatchName);
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            xmlDoc.CreateElementWithValueAttribute(LevelMatchName, SelectedLevel).AppendTo(newAppenderNode);
        }
    }
}
