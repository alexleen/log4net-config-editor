// Copyright © 2020 Alex Leendertsen

using System.Xml;
using Editor.Descriptors;
using Editor.Enums;
using Editor.Models.Base;
using Editor.Models.ConfigChildren;
using Editor.Utilities;

namespace Editor.Models
{
    internal static class ModelFactory
    {
        internal static ModelCreateResult TryCreate(XmlNode node, XmlNode log4NetNode, out ModelBase model)
        {
            model = null;

            switch (node.Name)
            {
                case Log4NetXmlConstants.Root:
                    model = new RootLoggerModel(node, false, LoggerDescriptor.Root);
                    return ModelCreateResult.Success;
                case Log4NetXmlConstants.Logger:
                    model = new LoggerModel(node, false, LoggerDescriptor.Logger);
                    return ModelCreateResult.Success;
                case Log4NetXmlConstants.Appender:
                    if (AppenderModel.TryCreate(node, log4NetNode, out AppenderModel appenderModel))
                    {
                        model = appenderModel;
                        return ModelCreateResult.Success;
                    }

                    return ModelCreateResult.UnknownAppender;
                case Log4NetXmlConstants.Renderer:
                    model = new RendererModel(node);
                    return ModelCreateResult.Success;
                case Log4NetXmlConstants.Param:
                    model = new ParamModel(node);
                    return ModelCreateResult.Success;
                default:
                    return ModelCreateResult.UnknownElement;
            }
        }
    }
}
