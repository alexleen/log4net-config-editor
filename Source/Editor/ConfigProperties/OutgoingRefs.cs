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
        private readonly XmlNode mOriginalAppender;

        public OutgoingRefs(XmlNode log4NetNode, ObservableCollection<IProperty> container, XmlNode originalAppender)
            : base(container, "↑ Refs:", "This element can reference the following appenders:")
        {
            mLog4NetNode = log4NetNode;
            mOriginalAppender = originalAppender;
            LoadAvailableLocations();
        }

        private void LoadAvailableLocations()
        {
            XmlNodeList availableRefs = mLog4NetNode.SelectNodes("appender");

            foreach (XmlNode appender in availableRefs)
            {
                string name = appender.Attributes["name"].Value;

                if (Equals(appender, mOriginalAppender) || RefsCollection.Any(@ref => Equals(@ref.Name, name)))
                {
                    continue;
                }

                RefsCollection.Add(new LoggerModel(name, mOriginalAppender, false));
            }
        }

        public override void Load(XmlNode originalNode)
        {
            foreach (LoggerModel loggerModel in RefsCollection)
            {
                loggerModel.IsEnabled = originalNode.SelectSingleNode($"appender-ref[@ref='{loggerModel.Name}']") != null;
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
