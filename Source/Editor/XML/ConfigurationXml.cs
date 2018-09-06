using Editor.Descriptors;
using Editor.Enums;
using Editor.Interfaces;
using Editor.Models;
using Editor.Models.Base;
using Editor.Utilities;
using log4net.Core;
using System;
using System.Collections.ObjectModel;
using System.Text;
using System.Xml;

namespace Editor.XML
{
    internal class ConfigurationXml : IConfigurationXml
    {
        private readonly string mFilename;
        private readonly IMessageBoxService mMessageBoxService;

        public ConfigurationXml(string filename, IMessageBoxService messageBoxService)
        {
            mFilename = filename ?? throw new ArgumentNullException(nameof(filename));
            mMessageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            Children = new ObservableCollection<ModelBase>();
        }

        public void CreateNew()
        {
            ConfigXml = new XmlDocument();
            ConfigXml.AppendChild(ConfigXml.CreateElement(Log4NetXmlConstants.Log4Net));
            Save();
        }

        public void Load()
        {
            ConfigXml = new XmlDocument();
            ConfigXml.Load(mFilename);

            bool? unrecognizedAppender = Reload();

            if (unrecognizedAppender.HasValue && unrecognizedAppender.Value)
            {
                mMessageBoxService.ShowWarning("At least one unrecognized appender was found in this configuration.");
            }
        }

        public bool? Reload()
        {
            XmlNodeList log4NetNodes = ConfigXml.SelectNodes("//log4net");

            if (log4NetNodes == null || log4NetNodes.Count == 0)
            {
                mMessageBoxService.ShowError("Could not find log4net configuration.");
                return null;
            }

            if (log4NetNodes.Count > 1)
            {
                mMessageBoxService.ShowWarning("More than one 'log4net' element was found in the specified file. Using the first occurrence.");
            }

            Log4NetNode = log4NetNodes[0];

            Children.Clear();

            bool unrecognized = LoadAppenders();
            LoadChildrenWithName(RendererDescriptor.Renderer.ElementName);
            LoadChildrenWithName(ParamDescriptor.Param.ElementName);
            foreach (LoggerModel logger in XmlUtilities.GetRootLoggerAndLoggers(Log4NetNode))
            {
                Children.Add(logger);
            }

            LoadRootAttributes();

            return unrecognized;
        }

        private bool LoadAppenders()
        {
            //Only selects appenders under this log4net element
            XmlNodeList appenderList = Log4NetNode.SelectNodes("appender");

            bool unrecognized = false;

            if (appenderList != null)
            {
                foreach (XmlNode node in appenderList)
                {
                    if (AppenderModel.TryCreate(node, Log4NetNode, out AppenderModel model))
                    {
                        Children.Add(model);
                    }
                    else
                    {
                        unrecognized = true;
                    }
                }
            }

            return unrecognized;
        }

        private void LoadChildrenWithName(string elementName)
        {
            XmlNodeList rendererList = Log4NetNode.SelectNodes(elementName);

            foreach (XmlNode renderer in rendererList)
            {
                Children.Add(new RendererModel(renderer));
            }
        }

        private void LoadRootAttributes()
        {
            if (bool.TryParse(Log4NetNode.Attributes?[Log4NetXmlConstants.Debug]?.Value, out bool debugResult) && debugResult)
            {
                Debug = true;
            }
            else
            {
                Debug = false;
            }

            if (Enum.TryParse(Log4NetNode.Attributes?[Log4NetXmlConstants.Update]?.Value, out Update update) && Equals(update, Update.Overwrite))
            {
                Update = Update.Overwrite;
            }
            else
            {
                Update = Update.Merge;
            }

            if (Log4NetUtilities.TryParseLevel(Log4NetNode.Attributes?[Log4NetXmlConstants.Threshold]?.Value, out Level levelResult) && !Equals(levelResult, Level.All))
            {
                Threshold = levelResult;
            }
            else
            {
                Threshold = Level.All;
            }
        }

        public void Save()
        {
            SaveRootAttributes();

            using (XmlTextWriter xtw = new XmlTextWriter(mFilename, Encoding.UTF8) { Formatting = Formatting.Indented })
            {
                ConfigXml.Save(xtw);
            }
        }

        private void SaveRootAttributes()
        {
            if (Debug)
            {
                Log4NetNode.AppendAttribute(ConfigXml, Log4NetXmlConstants.Debug, "true");
            }
            else
            {
                Log4NetNode.Attributes.RemoveNamedItem(Log4NetXmlConstants.Debug);
            }

            if (Update == Update.Overwrite)
            {
                //"Merge" is default, so we only need to add an attribute when "Overwrite" is selected
                Log4NetNode.AppendAttribute(ConfigXml, Log4NetXmlConstants.Update, Update.ToString());
            }
            else
            {
                Log4NetNode.Attributes.RemoveNamedItem(Log4NetXmlConstants.Update);
            }

            if (!Equals(Threshold, Level.All))
            {
                //"All" is default, so we only need to add an attribute when something other than "All" is selected
                Log4NetNode.AppendAttribute(ConfigXml, Log4NetXmlConstants.Threshold, Threshold.Name);
            }
            else
            {
                Log4NetNode.Attributes.RemoveNamedItem(Log4NetXmlConstants.Threshold);
            }
        }

        public void RemoveRefsTo(AppenderModel appenderModel)
        {
            //Remove all appender refs
            foreach (XmlNode refModel in XmlUtilities.FindAppenderRefs(Log4NetNode, appenderModel.Name))
            {
                refModel.ParentNode?.RemoveChild(refModel);
            }
        }

        public IElementConfiguration CreateElementConfigurationFor(ModelBase originalModel, string newElementName)
        {
            return new ElementConfiguration(this, originalModel?.Node, ConfigXml.CreateElement(newElementName));
        }

        public XmlDocument ConfigXml { get; private set; }

        public XmlNode Log4NetNode { get; private set; }

        public bool Debug { get; set; }

        public Update Update { get; set; }

        public Level Threshold { get; set; }

        public ObservableCollection<ModelBase> Children { get; private set; }

    }
}
