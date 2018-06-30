// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties;
using Editor.Descriptors;

namespace Editor.Definitions.Filters
{
    internal class LoggerMatchFilter : FilterSkeleton
    {
        public override string Name => "Logger Match Filter";

        public override FilterDescriptor Descriptor => FilterDescriptor.LoggerMatch;

        public override void Initialize()
        {
            AddProperty(new LoggerToMatch(Properties) { IsFocused = true });
            base.Initialize();
        }
    }
}
