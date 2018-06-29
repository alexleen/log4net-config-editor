// Copyright © 2018 Alex Leendertsen

namespace Editor.Descriptors.Base
{
    public abstract class DescriptorBase
    {
        protected DescriptorBase(string name, string elementName)
        {
            Name = name;
            ElementName = elementName;
        }

        /// <summary>
        /// Display name used in the UI.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Name of the element in the configuration XML.
        /// </summary>
        public string ElementName { get; protected set; }
    }
}
