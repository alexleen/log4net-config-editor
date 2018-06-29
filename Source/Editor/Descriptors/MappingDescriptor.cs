// Copyright © 2018 Alex Leendertsen

using Editor.Descriptors.Base;

namespace Editor.Descriptors
{
    public class MappingDescriptor : DescriptorBase
    {
        public static readonly MappingDescriptor Mapping;

        static MappingDescriptor()
        {
            Mapping = new MappingDescriptor("Mapping");
        }

        public MappingDescriptor(string name)
            : base(name, "mapping")
        {
        }
    }
}
