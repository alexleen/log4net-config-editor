// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Descriptors;

namespace Editor.Models
{
    public class FilterModel
    {
        public FilterModel(FilterDescriptor descriptor, XmlNode node)
        {
            Descriptor = descriptor;
            Node = node;
        }

        public FilterDescriptor Descriptor { get; }

        public XmlNode Node { get; set; }
    }
}
