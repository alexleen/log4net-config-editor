// Copyright © 2019 Alex Leendertsen

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using Editor.Descriptors;
using Editor.Properties;

namespace Editor.Models.ConfigChildren
{
    public interface IAppenderFactory
    {
        bool TryCreate(XmlNode appender, XmlNode log4NetNode, out AppenderModel appenderModel);
    }

    class AppenderFactory : IAppenderFactory
    {
        public bool TryCreate(XmlNode appender, XmlNode log4NetNode, out AppenderModel appenderModel)
        {
            string type = appender.Attributes["type"]?.Value;

            if (TryLookupCustomMapping(type, out string resolved))
            {
                type = resolved;
            }

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

        private bool TryLookupCustomMapping(string type, out string resolved)
        {
            if (!string.IsNullOrEmpty(Settings.Default.AppenderMappings))
            {
                AppenderMapping[] mappings;
                using (StringReader strReader = new StringReader(Settings.Default.AppenderMappings))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(AppenderMapping[]));
                    mappings = (AppenderMapping[])serializer.Deserialize(strReader);
                }

                IDictionary<string, AppenderMapping> mappingsByCustomType = mappings.ToDictionary(key => key.Custom);

                if (mappingsByCustomType.TryGetValue(type, out AppenderMapping mapped))
                {
                    resolved = mapped.Mapped;
                    return true;
                }
            }

            resolved = null;
            return false;
        }
    }
}
