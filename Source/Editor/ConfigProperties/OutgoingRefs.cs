// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Models;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class OutgoingRefs : RefsBase
    {
        private readonly IElementConfiguration mAppenderConfiguration;

        public OutgoingRefs(ReadOnlyCollection<IProperty> container, IElementConfiguration appenderConfiguration)
            : base(container, "↑ Refs:", "This element can reference the following appenders:")
        {
            mAppenderConfiguration = appenderConfiguration;
            LoadAvailableLocations();
        }

        private void LoadAvailableLocations()
        {
            XmlNodeList availableRefs = mAppenderConfiguration.Log4NetNode.SelectNodes("appender");

            foreach (XmlNode appender in availableRefs)
            {
                string name = appender.Attributes["name"].Value;

                if (Equals(appender, mAppenderConfiguration.OriginalNode) || RefsCollection.Any(@ref => Equals(@ref.Name, name)))
                {
                    continue;
                }

                RefsCollection.Add(new LoggerModel("appender", name, mAppenderConfiguration.OriginalNode, false));
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
