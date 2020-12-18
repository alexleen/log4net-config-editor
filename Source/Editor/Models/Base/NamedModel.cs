// Copyright © 2018 Alex Leendertsen

using System.Xml;
using Editor.Descriptors.Base;
using Editor.Utilities;

namespace Editor.Models.Base
{
    public abstract class NamedModel : ModelBase
    {
        protected NamedModel(XmlNode node, DescriptorBase descriptor)
            : base(node, descriptor)
        {
        }

        /// <summary>
        /// <see cref="Log4NetXmlConstants.Name"/> attribute's value for this model's node. 
        /// Null if node is not set or attribute does not exist.
        /// </summary>
        public virtual string Name => Node?.FindNodeAttributeValue(Log4NetXmlConstants.Name);
    }
}
