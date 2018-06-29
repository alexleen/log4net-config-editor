// Copyright © 2018 Alex Leendertsen

namespace Editor.Descriptors.Base
{
    //TODO add element name as a property
    public abstract class DescriptorBase
    {
        protected DescriptorBase(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
