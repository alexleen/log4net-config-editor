// Copyright © 2020 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;
using Editor.XML;

namespace Editor.ConfigProperties
{
    public class IncomingRefs : RefsBase
    {
        private readonly IElementConfiguration mAppenderConfiguration;
        private readonly Name mNameProperty;

        public IncomingRefs(Name nameProperty, IElementConfiguration appenderConfiguration)
            : base("↓ Refs:", "This appender can be referenced in the following elements:")
        {
            mNameProperty = nameProperty;
            mAppenderConfiguration = appenderConfiguration;
            RefsCollection = new ObservableCollection<IAcceptAppenderRef>();
            LoadAvailableLocations();
        }

        public ObservableCollection<IAcceptAppenderRef> RefsCollection { get; set; }

        /// <summary>
        ///     Finds all available locations for appender-refs.
        ///     These locations are enabled (or not) in the <see cref="Load" /> method.
        /// </summary>
        private void LoadAvailableLocations()
        {
            foreach (IAcceptAppenderRef logger in XmlUtilities.FindAvailableAppenderRefLocations(mAppenderConfiguration.Log4NetNode))
            {
                if (Equals(logger.Node, mAppenderConfiguration.OriginalNode))
                {
                    continue;
                }

                RefsCollection.Add(logger);
            }
        }

        public override void Load(IElementConfiguration config)
        {
            foreach (IAcceptAppenderRef loggerModel in RefsCollection)
            {
                loggerModel.IsEnabled = loggerModel.Node.SelectSingleNode($"appender-ref[@ref='{mNameProperty.Value}']") != null;
            }
        }

        public override void Save(IElementConfiguration config)
        {
            foreach (IAcceptAppenderRef loggerModel in RefsCollection)
            {
                if (loggerModel.IsEnabled)
                {
                    if (mNameProperty.Changed.IsTrue())
                    {
                        RemoveOldRefsFrom(loggerModel, mNameProperty.OriginalName);
                    }

                    XmlUtilities.AddAppenderRefToNode(loggerModel.Node, mNameProperty.Value, () => config.SaveToNode(loggerModel.Node, new Element("appender-ref", new[] { ("ref", mNameProperty.Value) })));
                }
                else
                {
                    RemoveOldRefsFrom(loggerModel, mNameProperty.Changed.IsTrue() ? mNameProperty.OriginalName : mNameProperty.Value);
                }
            }
        }

        private static void RemoveOldRefsFrom(IAcceptAppenderRef loggerModel, string name)
        {
            XmlNodeList oldRefs = loggerModel.Node.SelectNodes($"appender-ref[@ref='{name}']");

            foreach (XmlNode appenderRef in oldRefs)
            {
                loggerModel.Node.RemoveChild(appenderRef);
            }
        }
    }
}
