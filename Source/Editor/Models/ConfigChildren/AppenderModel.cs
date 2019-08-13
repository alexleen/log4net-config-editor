// Copyright © 2019 Alex Leendertsen

using System.Xml;
using Editor.Descriptors;
using Editor.Models.Base;

namespace Editor.Models.ConfigChildren
{
    public class AppenderModel : NamedModel
    {
        public AppenderModel(AppenderDescriptor descriptor, XmlNode node, int incomingReferences)
            : base(node, descriptor)
        {
            IncomingReferences = incomingReferences;
        }

        public int IncomingReferences { get; }

        public bool HasReferences => IncomingReferences > 0;

        public bool IsEnabled { get; set; }

        public static bool TryCreate(XmlNode appender, XmlNode log4NetNode, out AppenderModel appenderModel)
        {
            string type = appender.Attributes["type"]?.Value;

            if (AppenderDescriptor.TryFindByTypeNamespace(type, out AppenderDescriptor descriptor))
            {
                string name = appender.Attributes["name"]?.Value;
                int incomingReferences = log4NetNode.SelectNodes($"//appender-ref[@ref='{name}']").Count;

                if (descriptor == AppenderDescriptor.Async)
                {
                    appenderModel = new AsyncAppenderModel(appender, incomingReferences);
                }
                else
                {
                    appenderModel = new AppenderModel(descriptor, appender, incomingReferences);
                }

                return true;
            }

            appenderModel = null;
            return false;
        }
    }
}
