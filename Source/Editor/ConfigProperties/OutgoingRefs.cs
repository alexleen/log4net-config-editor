// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Models;
using Editor.Utilities;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace Editor.ConfigProperties
{
    public class OutgoingRefs : RefsBase
    {
        private readonly IElementConfiguration mConfiguration;

        public OutgoingRefs(ReadOnlyCollection<IProperty> container, IElementConfiguration appenderConfiguration)
            : base(container, "↑ Refs:", "This element can reference the following appenders:")
        {
            mConfiguration = appenderConfiguration;
            LoadAvailableLocations();
            RefsCollection = new ObservableCollection<AppenderModel>();
        }

        public ObservableCollection<AppenderModel> RefsCollection { get; set; }

        private void LoadAvailableLocations()
        {
            XmlNodeList availableRefs = mConfiguration.Log4NetNode.SelectNodes("appender");

            foreach (XmlNode appender in availableRefs)
            {
                string name = appender.Attributes["name"].Value;

                if (Equals(appender, mConfiguration.OriginalNode) || RefsCollection.Any(@ref => Equals(@ref.Name, name)))
                {
                    continue;
                }

                if (TryCreate(appender, out AppenderModel appenderModel))
                {
                    RefsCollection.Add(appenderModel);
                }
            }
        }

        //TODO move to AppenderDescriptor and use here and in MainWindow
        private bool TryCreate(XmlNode appender, out AppenderModel appenderModel)
        {
            string type = appender.Attributes?["type"]?.Value;

            if (AppenderDescriptor.TryFindByTypeNamespace(type, out AppenderDescriptor descriptor))
            {
                string name = appender.Attributes?["name"].Value;
                int incomingReferences = mConfiguration.Log4NetNode.SelectNodes($"//appender-ref[@ref='{name}']").Count;
                appenderModel = new AppenderModel(descriptor, appender, name, incomingReferences);
                return true;
            }

            appenderModel = null;
            return false;
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
