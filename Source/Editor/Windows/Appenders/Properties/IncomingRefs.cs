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
            XmlNodeList asyncAppenders = mLog4NetNode.SelectNodes("appender[@type='Log4Net.Async.AsyncForwardingAppender,Log4Net.Async']");

            if (asyncAppenders != null)
            {
                foreach (XmlNode asyncAppender in asyncAppenders)
                {
                    if (Equals(asyncAppender, mOriginalAppender))
                    {
                        continue;
                    }

                    string name = asyncAppender.Attributes?["name"]?.Value;
                    RefsCollection.Add(new LoggerModel(name, asyncAppender, false));
                }
            }

            XmlNode root = mLog4NetNode.SelectSingleNode("root");

            if (root != null)
            {
                RefsCollection.Add(new LoggerModel("root", root, false));
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

                    if (appenderRefs != null && appenderRefs.Count > 0)
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
