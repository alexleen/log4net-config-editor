// Copyright © 2020 Alex Leendertsen

using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Xml;
using SystemInterface.Xml;
using SystemWrapper.Xml;
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
        private readonly IToastService mToastService;
        protected readonly ICanLoadAndSaveXml LoadAndSave;
        private readonly ObservableCollection<ModelBase> mMutableChildren;
        private IXmlDocument mConfigXml;

        public ConfigurationXml(IToastService toastService, ICanLoadAndSaveXml loadAndSave)
        {
            mToastService = toastService ?? throw new ArgumentNullException(nameof(toastService));
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
                mToastService.ShowWarning("At least one unrecognized appender was found in this configuration.");
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
                mToastService.ShowError("Could not find log4net configuration.");
                return null;
            }

            if (log4NetNodes.Count > 1)
            {
                mToastService.ShowWarning("More than one 'log4net' element was found in the specified file. Using the first occurrence.");
            }

            Log4NetNode = log4NetNodes[0];

            mMutableChildren.Clear();

            bool unrecognized = false;
            foreach (XmlNode node in Log4NetNode.ChildNodes)
            {
                ModelCreateResult result = ModelFactory.TryCreate(node, Log4NetNode, out ModelBase model);

                if (result == ModelCreateResult.Success)
                {
                    mMutableChildren.Add(model);
                }
                else if (result == ModelCreateResult.UnknownAppender)
                {
                    unrecognized = true;
                }
            }

            LoadRootAttributes();

            return unrecognized;
        }

        private void LoadRootAttributes()
        {
            if (bool.TryParse(Log4NetNode.FindNodeAttributeValue(Log4NetXmlConstants.Debug), out bool debugResult) && debugResult)
            {
                Debug = true;
            }
            else
            {
                Debug = false;
            }

            if (Enum.TryParse(Log4NetNode.FindNodeAttributeValue(Log4NetXmlConstants.Update), out Update update) && Equals(update, Update.Overwrite))
            {
                Update = Update.Overwrite;
            }
            else
            {
                Update = Update.Merge;
            }

            if (Log4NetUtilities.TryParseLevel(Log4NetNode.FindNodeAttributeValue(Log4NetXmlConstants.Threshold), out Level levelResult) && !Equals(levelResult, Level.All))
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

        public Task SaveAsync(string path)
        {
            return LoadAndSave.SaveAsync(mConfigXml, path);
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
