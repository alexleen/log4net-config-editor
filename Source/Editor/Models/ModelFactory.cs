// Copyright © 2019 Alex Leendertsen

using System.Xml;
using Editor.Descriptors;
using Editor.Models.Base;
using Editor.Models.ConfigChildren;
using Editor.Utilities;

namespace Editor.Models
{
    internal static class ModelFactory
    {
        internal static bool TryCreate(XmlNode node, XmlNode log4netNode, out ModelBase model)
        {
            switch (node.Name)
            {
                case Log4NetXmlConstants.Root:
                    model = new RootLoggerModel(node, false, LoggerDescriptor.Root);
                    return true;
                case Log4NetXmlConstants.Logger:
                    model = new LoggerModel(node, false, LoggerDescriptor.Logger);
                    return true;
                case Log4NetXmlConstants.Appender:
                    if (AppenderModel.TryCreate(node, log4netNode, out AppenderModel appenderModel))
                    {
                        model = appenderModel;
                        return true;
                    }
                    break;
                case Log4NetXmlConstants.Renderer:
                    model = new RendererModel(node);
                    return true;
                case Log4NetXmlConstants.Param:
                    model = new ParamModel(node);
                    return true;
            }

            model = null;
            return false;
        }
    }
}
