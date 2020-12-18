// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Models.ConfigChildren;
using Editor.Utilities;
using Editor.XML;

namespace Editor.ConfigProperties
{
    public class OutgoingRefs : RefsBase
    {
        private readonly IElementConfiguration mConfiguration;

        public OutgoingRefs(IElementConfiguration appenderConfiguration)
            : base("↑ Refs:", "This element can reference the following appenders:")
        {
            mConfiguration = appenderConfiguration;
            RefsCollection = new ObservableCollection<AppenderModel>();
            LoadPossibleReferences();
        }

        public ObservableCollection<AppenderModel> RefsCollection { get; set; }

        private void LoadPossibleReferences()
        {
            IEnumerable<XmlNode> availableRefs = mConfiguration.FindLog4NetNodeChildren(Log4NetXmlConstants.Appender);

            foreach (XmlNode appender in availableRefs)
            {
                string name = appender.FindNodeAttributeValue(Log4NetXmlConstants.Name);

                if (Equals(appender, mConfiguration.OriginalNode) || RefsCollection.Any(@ref => Equals(@ref.Name, name)))
                {
                    continue;
                }

                if (AppenderModel.TryCreate(appender, mConfiguration.Log4NetNode, out AppenderModel appenderModel))
                {
                    RefsCollection.Add(appenderModel);
                }
            }
        }

        public override void Load(IElementConfiguration config)
        {
            foreach (AppenderModel appenderModel in RefsCollection)
            {
                appenderModel.IsEnabled = config.OriginalNode.SelectSingleNode($"appender-ref[@ref='{appenderModel.Name}']") != null;
            }
        }

        public override void Save(IElementConfiguration config)
        {
            foreach (AppenderModel appenderModel in RefsCollection.Where(@ref => @ref.IsEnabled))
            {
                config.Save(new Element("appender-ref", new[] { ("ref", appenderModel.Name) }));
            }
        }
    }
}
