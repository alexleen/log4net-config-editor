// Copyright © 2018 Alex Leendertsen

using System.Xml;

namespace Editor.Models
{
    public class RefModel
    {
        public RefModel(XmlNode appenderRef)
        {
            AppenderRef = appenderRef;
            ElementName = appenderRef.ParentNode.Name;
            Name = appenderRef.ParentNode.Attributes?["name"]?.Value;
            AppenderType = appenderRef.ParentNode.Attributes?["type"]?.Value;
        }

        public XmlNode AppenderRef { get; }
        public string ElementName { get; }
        public string Name { get; }
        public string AppenderType { get; }
    }
}
