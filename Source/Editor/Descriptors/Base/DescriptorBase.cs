// Copyright © 2018 Alex Leendertsen

namespace Editor.Descriptors
{
    public abstract class DescriptorBase
    {
        protected DescriptorBase(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
