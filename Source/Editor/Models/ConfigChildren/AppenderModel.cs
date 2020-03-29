// Copyright © 2019 Alex Leendertsen

using System.Xml;
using Editor.Descriptors;
using Editor.Models.Base;
using Editor.Utilities;

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

        /// <summary>
        /// Attempts to create an <see cref="AppenderModel"/> from the specified <paramref name="appender"/> and <paramref name="log4NetNode"/>.
        /// </summary>
        /// <param name="appender">XML node representing the appender</param>
        /// <param name="log4NetNode">Parent log4net node, used to determine how many incoming references there are to the specified <paramref name="appender"/></param>
        /// <param name="appenderModel">Resulting appender model, if found</param>
        /// <returns></returns>
        public static bool TryCreate(XmlNode appender, XmlNode log4NetNode, out AppenderModel appenderModel)
        {
            string type = appender.Attributes[Log4NetXmlConstants.Type]?.Value;

            if (AppenderDescriptor.TryFindByTypeNamespace(type, out AppenderDescriptor descriptor))
            {
                string name = appender.Attributes[Log4NetXmlConstants.Name]?.Value;
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
