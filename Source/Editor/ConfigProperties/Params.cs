// Copyright © 2020 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Definitions.Factory;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Models;
using Editor.SaveStrategies;
using Editor.Utilities;
using Editor.Windows;
using Editor.Windows.SizeLocation;
using Editor.XML;

namespace Editor.ConfigProperties
{
    internal class Params : PropertyBase
    {
        private readonly IConfiguration mConfiguration;
        private readonly IMessageBoxService mMessageBoxService;

        internal Params(IConfiguration configuration, IMessageBoxService messageBoxService)
            : base(new GridLength(1, GridUnitType.Star))
        {
            mConfiguration = configuration;
            mMessageBoxService = messageBoxService;

            ExistingParams = new ObservableCollection<ParamModel>();
            Add = new Command(AddParamClick);
        }

        public ObservableCollection<ParamModel> ExistingParams { get; }

        public ICommand Add { get; }

        private void AddParamClick()
        {
            ShowParamWindow(new ParamModel(null, ShowParamWindow, RemoveModel));
        }

        private void ShowParamWindow(ParamModel paramModel)
        {
            XmlElement newParam = mConfiguration.ConfigXml.CreateElement(ParamDescriptor.Param.ElementName);

            IElementConfiguration configuration = new ElementConfiguration(mConfiguration, paramModel.Node, newParam);

            ISaveStrategy saveStrategy = new AddSaveStrategy<ParamModel>(paramModel, AddModel, newParam);

            Window paramWindow = new ElementWindow(configuration,
                                                   DefinitionFactory.Create(ParamDescriptor.Param, configuration),
                                                   WindowSizeLocationFactory.Create(ParamDescriptor.Param),
                                                   saveStrategy);

            mMessageBoxService.ShowWindow(paramWindow);
        }

        private void AddModel(ParamModel paramModel)
        {
            ExistingParams.Add(paramModel);
        }

        private void RemoveModel(ParamModel paramModel)
        {
            ExistingParams.Remove(paramModel);
        }

        public override void Load(IElementConfiguration config)
        {
            XmlNodeList paramNodes = config.OriginalNode.SelectNodes(Log4NetXmlConstants.Param);

            foreach (XmlNode paramNode in paramNodes)
            {
                AddModel(new ParamModel(paramNode, ShowParamWindow, RemoveModel));
            }
        }

        public override void Save(IElementConfiguration config)
        {
            foreach (ParamModel param in ExistingParams)
            {
                config.NewNode.AppendChild(param.Node);
            }
        }
    }
}
