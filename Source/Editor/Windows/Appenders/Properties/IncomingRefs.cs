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
                if (Equals(logger.Node, mOriginalAppender))
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
                loggerModel.IsEnabled = loggerModel.Node.SelectSingleNode($"appender-ref[@ref='{mNameProperty.Value}']") != null;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            foreach (LoggerModel loggerModel in RefsCollection)
            {
                if (loggerModel.IsEnabled)
                {
                    XmlUtilities.AddAppenderRefToNode(xmlDoc, loggerModel.Node, mNameProperty.Value);
                }
                else
                {
                    XmlNodeList appenderRefs = loggerModel.Node.SelectNodes($"appender-ref[@ref='{mNameProperty.Value}']");

                    if (appenderRefs.Count > 0)
                    {
                        foreach (XmlNode appenderRef in appenderRefs)
                        {
                            loggerModel.Node.RemoveChild(appenderRef);
                        }
                    }
                }
            }
        }
    }
}
