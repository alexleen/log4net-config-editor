// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Models;
using Editor.Utilities;

namespace Editor.Windows.Appenders.Properties
{
    public class Refs : AppenderPropertyBase
    {
        private readonly XmlNode mLog4NetNode;
        private readonly Name mNameProperty;

        public Refs(XmlNode log4NetNode, Name nameProperty, ObservableCollection<IAppenderProperty> container)
            : base(container, new GridLength(1, GridUnitType.Star))
        {
            mLog4NetNode = log4NetNode;
            mNameProperty = nameProperty;
            RefsCollection = new ObservableCollection<LoggerModel>();

            //Loading refs only requires the log4net node, which we get during construction
            LoadRefs();
        }

        public ObservableCollection<LoggerModel> RefsCollection { get; set; }

        public override void Load(XmlNode originalAppenderNode)
        {
            //This will only be called if there's an original appender node, which means there's probably an appender name
            //Refresh to hopefully pick up refs that are using the newly loaded name which was not available at construction time
            RefsCollection.Clear();
            LoadRefs();
        }

        private void LoadRefs()
        {
            XmlNodeList asyncAppenders = mLog4NetNode.SelectNodes("appender[@type='Log4Net.Async.AsyncForwardingAppender,Log4Net.Async']");

            if (asyncAppenders != null)
            {
                foreach (XmlNode asyncAppender in asyncAppenders)
                {
                    XmlNode appenderRef = asyncAppender.SelectSingleNode($"appender-ref[@ref='{mNameProperty.Value}']");
                    string name = asyncAppender.Attributes?["name"]?.Value;
                    RefsCollection.Add(new LoggerModel(name, asyncAppender, appenderRef != null));
                }
            }

            XmlNode root = mLog4NetNode.SelectSingleNode("root");

            if (root != null)
            {
                XmlNode appenderRef = root.SelectSingleNode($"appender-ref[@ref='{mNameProperty.Value}']");
                RefsCollection.Add(new LoggerModel("root", root, appenderRef != null));
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newAppenderNode)
        {
            foreach (LoggerModel loggerModel in RefsCollection)
            {
                XmlNodeList appenderRefs = loggerModel.LoggerNode.SelectNodes($"appender-ref[@ref='{mNameProperty.Value}']");

                if (loggerModel.IsEnabled)
                {
                    if (appenderRefs == null || appenderRefs.Count == 0)
                    {
                        //Doesn't exist - add
                        xmlDoc.CreateElementWithAttribute("appender-ref", "ref", mNameProperty.Value).AppendTo(loggerModel.LoggerNode);
                    }
                    else if (appenderRefs.Count == 1)
                    {
                        //Only one - we're good
                    }
                    else
                    {
                        //More than one - remove all
                        foreach (XmlNode appenderRef in appenderRefs)
                        {
                            loggerModel.LoggerNode.RemoveChild(appenderRef);
                        }

                        //Add
                        xmlDoc.CreateElementWithAttribute("appender-ref", "ref", mNameProperty.Value).AppendTo(loggerModel.LoggerNode);
                    }
                }
                else
                {
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
