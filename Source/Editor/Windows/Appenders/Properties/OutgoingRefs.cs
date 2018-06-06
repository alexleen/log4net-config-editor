// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Models;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class OutgoingRefs : RefsBase
    {
        private readonly XmlNode mLog4NetNode;

        public OutgoingRefs(ObservableCollection<IProperty> container, XmlNode log4NetNode)
            : base(container, new GridLength(1, GridUnitType.Star))
        {
            mLog4NetNode = log4NetNode;
        }

        public override void Load(XmlNode originalNode)
        {
            XmlNodeList refs = originalNode.SelectNodes("appender-ref");

            foreach (XmlNode @ref in refs)
            {
                string name = @ref.Attributes["ref"].Value;
                RefsCollection.Add(new LoggerModel(name, originalNode, true));
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
        }
    }
}
