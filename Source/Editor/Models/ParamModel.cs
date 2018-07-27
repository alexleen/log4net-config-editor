// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors;
using System.Xml;

namespace Editor.Models
{
    internal class ParamModel : ChildModel
    {
        public ParamModel(XmlNode node, string name)
            : base(ParamDescriptor.Param.ElementName, node)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
