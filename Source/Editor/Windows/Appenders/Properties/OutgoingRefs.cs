// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class OutgoingRefs : RefsBase
    {
        private readonly XmlNode mLog4NetNode;

        public OutgoingRefs(XmlNode log4NetNode, ObservableCollection<IProperty> container)
            : base(container, "↑ Refs:", "This element can reference the following appenders:")
        {
            mLog4NetNode = log4NetNode;
        }

        public override void Load(XmlNode originalNode)
        {
            XmlNodeList existingRefs = originalNode.SelectNodes("appender-ref");

            foreach (XmlNode @ref in existingRefs)
            {
                string name = @ref.Attributes["ref"].Value;
                RefsCollection.Add(new LoggerModel(name, originalNode, true));
            }

            XmlNodeList availableRefs = mLog4NetNode.SelectNodes("appender");

            foreach (XmlNode appender in availableRefs)
            {
                string name = appender.Attributes["name"].Value;

                if (Equals(appender, originalNode) || RefsCollection.Any(@ref => Equals(@ref.Name, name)))
                {
                    continue;
                }

                RefsCollection.Add(new LoggerModel(name, originalNode, false));
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            foreach (LoggerModel loggerModel in RefsCollection.Where(@ref => @ref.IsEnabled))
            {
                xmlDoc.CreateElementWithAttribute("appender-ref", "ref", loggerModel.Name).AppendTo(newNode);
            }
        }
    }
}
