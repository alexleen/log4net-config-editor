// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Models;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class IncomingRefs : RefsBase
    {
        private readonly Name mNameProperty;
        private readonly IElementConfiguration mAppenderConfiguration;

        public IncomingRefs(ReadOnlyCollection<IProperty> container, Name nameProperty, IElementConfiguration appenderConfiguration)
            : base(container, "↓ Refs:", "This appender can be referenced in the following elements:")
        {
            mNameProperty = nameProperty;
            mAppenderConfiguration = appenderConfiguration;
            LoadAvailableLocations();
        }

        /// <summary>
        /// Finds all available locations for appender-refs.
        /// These locations are enabled (or not) in the <see cref="Load"/> method.
        /// </summary>
        private void LoadAvailableLocations()
        {
            foreach (LoggerModel logger in XmlUtilities.FindAvailableAppenderRefLocations(mAppenderConfiguration.Log4NetNode))
            {
                if (Equals(logger.Node, mAppenderConfiguration.OriginalNode))
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
                    if (mNameProperty.Changed.HasValue && mNameProperty.Changed.Value)
                    {
                        RemoveOldRefsFrom(loggerModel, mNameProperty.OriginalName);
                    }

                    XmlUtilities.AddAppenderRefToNode(xmlDoc, loggerModel.Node, mNameProperty.Value);
                }
                else
                {
                    RemoveOldRefsFrom(loggerModel, mNameProperty.Changed.HasValue && mNameProperty.Changed.Value ? mNameProperty.OriginalName : mNameProperty.Value);
                }
            }
        }

        private static void RemoveOldRefsFrom(LoggerModel loggerModel, string name)
        {
            XmlNodeList oldRefs = loggerModel.Node.SelectNodes($"appender-ref[@ref='{name}']");

            foreach (XmlNode appenderRef in oldRefs)
            {
                loggerModel.Node.RemoveChild(appenderRef);
            }
        }
    }
}
