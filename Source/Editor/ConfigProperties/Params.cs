using Editor.ConfigProperties.Base;
using Editor.Definitions.Factory;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Models;
using Editor.Utilities;
using Editor.Windows;
using Editor.Windows.SizeLocation;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using System.Xml;

namespace Editor.ConfigProperties
{
    internal class Params : PropertyBase
    {
        private readonly IConfiguration mConfiguration;
        private readonly IMessageBoxService mMessageBoxService;

        internal Params(ReadOnlyCollection<IProperty> container, IConfiguration configuration, IMessageBoxService messageBoxService)
            : base(container, new GridLength(1, GridUnitType.Star))
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

            IElementConfiguration appenderConfiguration = new ElementConfiguration(mConfiguration, paramModel.Node, newParam);

            ISaveStrategy saveStrategy = new AddSaveStrategy<ParamModel>(paramModel, AddModel, newParam);

            Window paramWindow = new ElementWindow(appenderConfiguration,
                                                    DefinitionFactory.Create(ParamDescriptor.Param, appenderConfiguration),
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

        public override void Load(XmlNode originalNode)
        {
            XmlNodeList paramNodes = originalNode.SelectNodes(Log4NetXmlConstants.Param);

            if (paramNodes == null)
            {
                return;
            }

            foreach (XmlNode paramNode in paramNodes)
            {
                AddModel(new ParamModel(paramNode, ShowParamWindow, RemoveModel));
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            foreach (ParamModel param in ExistingParams)
            {
                newNode.AppendChild(param.Node);
            }
        }
    }
}
