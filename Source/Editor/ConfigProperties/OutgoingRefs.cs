// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Models.ConfigChildren;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class OutgoingRefs : RefsBase
    {
        private readonly IElementConfiguration mConfiguration;

        public OutgoingRefs(ReadOnlyCollection<IProperty> container, IElementConfiguration appenderConfiguration)
            : base(container, "↑ Refs:", "This element can reference the following appenders:")
        {
            mConfiguration = appenderConfiguration;
            RefsCollection = new ObservableCollection<AppenderModel>();
            LoadPossibleReferences();
        }

        public ObservableCollection<AppenderModel> RefsCollection { get; set; }

        private void LoadPossibleReferences()
        {
            XmlNodeList availableRefs = mConfiguration.Log4NetNode.SelectNodes("appender");

            foreach (XmlNode appender in availableRefs)
            {
                string name = appender.Attributes["name"].Value;

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

        public override void Load(XmlNode originalNode)
        {
            foreach (AppenderModel appenderModel in RefsCollection)
            {
                appenderModel.IsEnabled = originalNode.SelectSingleNode($"appender-ref[@ref='{appenderModel.Name}']") != null;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            foreach (AppenderModel appenderModel in RefsCollection.Where(@ref => @ref.IsEnabled))
            {
                xmlDoc.CreateElementWithAttribute("appender-ref", "ref", appenderModel.Name).AppendTo(newNode);
            }
        }
    }
}
