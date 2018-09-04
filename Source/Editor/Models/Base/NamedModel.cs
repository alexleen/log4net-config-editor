// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors.Base;
using Editor.Models.Base;
using Editor.Utilities;
using System.Xml;

namespace Editor.Models
{
    public abstract class NamedModel : ModelBase
    {
        public NamedModel(XmlNode node, DescriptorBase descriptor)
            : base(node, descriptor)
        {
        }

        /// <summary>
        /// <see cref="Log4NetXmlConstants.Name"/> attribute's value for this model's node. 
        /// Null if node is not set or attribute does not exist.
        /// </summary>
        public string Name => Node?.Attributes[Log4NetXmlConstants.Name]?.Value;
    }
}
