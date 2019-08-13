// Copyright © 2019 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Xml;
using SystemInterface.Xml;
using SystemWrapper.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Interfaces;
using Editor.Models;
using Editor.Models.Base;
using Editor.Models.ConfigChildren;
using Editor.Utilities;
using log4net.Core;

namespace Editor.XML
{
    internal class ConfigurationXml
    {
        private readonly IMessageBoxService mMessageBoxService;
        private readonly ObservableCollection<ModelBase> mMutableChildren;
        protected readonly ICanLoadAndSaveXml LoadAndSave;
        private IXmlDocument mConfigXml;

        public ConfigurationXml(IMessageBoxService messageBoxService, ICanLoadAndSaveXml loadAndSave)
        {
            mMessageBoxService = messageBoxService ?? throw new ArgumentNullException(nameof(messageBoxService));
            LoadAndSave = loadAndSave ?? throw new ArgumentNullException(nameof(loadAndSave));
            mMutableChildren = new ObservableCollection<ModelBase>();
            Children = new ReadOnlyObservableCollection<ModelBase>(mMutableChildren);
        }

        public void Load()
        {
            mConfigXml = LoadAndSave.Load();

            bool? unrecognizedAppender = Reload();

            if (unrecognizedAppender.IsTrue())
            {
                mMessageBoxService.ShowWarning("At least one unrecognized appender was found in this configuration.");
            }
        }

        public virtual bool? Reload()
        {
            if (mConfigXml == null)
            {
                throw new InvalidOperationException($"{nameof(Load)} must be called before {nameof(Reload)}.");
            }

            XmlNodeList log4NetNodes = mConfigXml.SelectNodes("//log4net");

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

            mMutableChildren.Clear();

            bool unrecognized = LoadAppenders();
            LoadRenderers();
            LoadParams();
            foreach (IAcceptAppenderRef logger in XmlUtilities.GetRootLoggerAndLoggers(Log4NetNode))
            {
                mMutableChildren.Add((ModelBase)logger);
            }

            LoadRootAttributes();

            return unrecognized;
        }

        private bool LoadAppenders()
        {
            //Only selects appenders under this log4net element
            XmlNodeList appenderList = Log4NetNode.SelectNodes("appender");

            bool unrecognized = false;

            foreach (XmlNode node in appenderList)
            {
                if (AppenderModel.TryCreate(node, Log4NetNode, out AppenderModel model))
                {
                    mMutableChildren.Add(model);
                }
                else
                {
                    unrecognized = true;
                }
            }

            return unrecognized;
        }

        private void LoadRenderers()
        {
            XmlNodeList rendererList = Log4NetNode.SelectNodes(RendererDescriptor.Renderer.ElementName);

            foreach (XmlNode renderer in rendererList)
            {
                mMutableChildren.Add(new RendererModel(renderer));
            }
        }

        private void LoadParams()
        {
            XmlNodeList rendererList = Log4NetNode.SelectNodes(ParamDescriptor.Param.ElementName);

            foreach (XmlNode renderer in rendererList)
            {
                mMutableChildren.Add(new ParamModel(renderer));
            }
        }

        private void LoadRootAttributes()
        {
            if (bool.TryParse(Log4NetNode.Attributes[Log4NetXmlConstants.Debug]?.Value, out bool debugResult) && debugResult)
            {
                Debug = true;
            }
            else
            {
                Debug = false;
            }

            if (Enum.TryParse(Log4NetNode.Attributes[Log4NetXmlConstants.Update]?.Value, out Update update) && Equals(update, Update.Overwrite))
            {
                Update = Update.Overwrite;
            }
            else
            {
                Update = Update.Merge;
            }

            if (Log4NetUtilities.TryParseLevel(Log4NetNode.Attributes[Log4NetXmlConstants.Threshold]?.Value, out Level levelResult) && !Equals(levelResult, Level.All))
            {
                Threshold = levelResult;
            }
            else
            {
                Threshold = Level.All;
            }
        }

        public virtual Task SaveAsync()
        {
            return LoadAndSave.SaveAsync(mConfigXml);
        }

        public void RemoveRefsTo(AppenderModel appenderModel)
        {
            //Remove all appender refs
            foreach (XmlNode refModel in XmlUtilities.FindAppenderRefs(Log4NetNode, appenderModel.Name))
            {
                refModel.ParentNode?.RemoveChild(refModel);
            }
        }

        public virtual void RemoveChild(ModelBase child)
        {
            Log4NetNode.RemoveChild(child.Node);

            mMutableChildren.Remove(child);

            if (child is AppenderModel appenderModel)
            {
                RemoveRefsTo(appenderModel);
            }
        }

        public XmlDocument ConfigXml => ((XmlDocumentWrap)mConfigXml).XmlDocumentInstance;

        public XmlNode Log4NetNode { get; private set; }

        private bool mDebug;

        public virtual bool Debug
        {
            get => mDebug;
            set
            {
                if (value == mDebug)
                {
                    return;
                }

                if (value)
                {
                    Log4NetNode.AppendAttribute(mConfigXml, Log4NetXmlConstants.Debug, "true");
                }
                else
                {
                    Log4NetNode.Attributes.RemoveNamedItem(Log4NetXmlConstants.Debug);
                }

                mDebug = value;
            }
        }

        private Update mUpdate;

        public virtual Update Update
        {
            get => mUpdate;
            set
            {
                if (value == mUpdate)
                {
                    return;
                }

                if (value == Update.Overwrite)
                {
                    //"Merge" is default, so we only need to add an attribute when "Overwrite" is selected
                    Log4NetNode.AppendAttribute(mConfigXml, Log4NetXmlConstants.Update, value.ToString());
                }
                else
                {
                    Log4NetNode.Attributes.RemoveNamedItem(Log4NetXmlConstants.Update);
                }

                mUpdate = value;
            }
        }

        private Level mThreshold = Level.All;

        public virtual Level Threshold
        {
            get => mThreshold;
            set
            {
                if (value == mThreshold)
                {
                    return;
                }

                if (!Equals(value, Level.All))
                {
                    //"All" is default, so we only need to add an attribute when something other than "All" is selected
                    Log4NetNode.AppendAttribute(mConfigXml, Log4NetXmlConstants.Threshold, value.Name);
                }
                else
                {
                    Log4NetNode.Attributes.RemoveNamedItem(Log4NetXmlConstants.Threshold);
                }

                mThreshold = value;
            }
        }

        public ReadOnlyObservableCollection<ModelBase> Children { get; }
    }
}
