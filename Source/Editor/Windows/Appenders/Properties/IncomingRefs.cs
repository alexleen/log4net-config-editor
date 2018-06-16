// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class IncomingRefs : RefsBase
    {
        private readonly XmlNode mLog4NetNode;
        private readonly Name mNameProperty;
        private readonly XmlNode mOriginalAppender;

        public IncomingRefs(XmlNode log4NetNode, Name nameProperty, ObservableCollection<IProperty> container, XmlNode originalAppender)
            : base(container, "↓ Refs:", "This appender can be referenced in the following elements:")
        {
            mLog4NetNode = log4NetNode;
            mNameProperty = nameProperty;
            mOriginalAppender = originalAppender;
            LoadAvailableLocations();
        }

        /// <summary>
        /// Finds all available locations for appender-refs.
        /// These locations are enabled (or not) in the <see cref="Load"/> method.
        /// </summary>
        private void LoadAvailableLocations()
        {
            foreach (LoggerModel logger in XmlUtilities.FindAvailableAppenderRefLocations(mLog4NetNode))
            {
                if (Equals(logger.LoggerNode, mOriginalAppender))
                {
                    continue;
                }

                RefsCollection.Add(logger);
            }
        }

        public override void Load(XmlNode originalNode)
        {
            foreach (LoggerModel loggerModel in RefsCollection)
            {
                loggerModel.IsEnabled = loggerModel.LoggerNode.SelectSingleNode($"appender-ref[@ref='{mNameProperty.Value}']") != null;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            foreach (LoggerModel loggerModel in RefsCollection)
            {
                if (loggerModel.IsEnabled)
                {
                    XmlUtilities.AddAppenderRefToNode(xmlDoc, loggerModel.LoggerNode, mNameProperty.Value);
                }
                else
                {
                    XmlNodeList appenderRefs = loggerModel.LoggerNode.SelectNodes($"appender-ref[@ref='{mNameProperty.Value}']");

                    if (appenderRefs.Count > 0)
                    {
                        foreach (XmlNode appenderRef in appenderRefs)
                        {
                            loggerModel.LoggerNode.RemoveChild(appenderRef);
                        }
                    }
                }
            }
        }
    }
}
