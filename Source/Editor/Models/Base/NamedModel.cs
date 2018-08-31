// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors.Base;
using Editor.Models.Base;
using System.Xml;

namespace Editor.Models
{
    public abstract class NamedModel : ModelBase
    {
        public NamedModel(string name, XmlNode node, DescriptorBase descriptor)
            : base(node, descriptor)
        {
            Name = name;
        }

        //TODO get from node attr?
        public string Name { get; }
    }
}
