// Copyright © 2020 Alex Leendertsen

namespace Editor.Descriptors.Base
{
    public abstract class DescriptorBase
    {
        protected DescriptorBase(string name, string elementName, string typeNamespace)
        {
            Name = name;
            ElementName = elementName;
            TypeNamespace = typeNamespace;
        }

        /// <summary>
        /// Display name used in the UI.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Name of the element in the configuration XML.
        /// </summary>
        public string ElementName { get; }

        /// <summary>
        /// log4net type namespace.
        /// </summary>
        public string TypeNamespace { get; }

        public override string ToString()
        {
            return Name;
        }
    }
}
