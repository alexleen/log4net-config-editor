// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using Editor.Descriptors.Base;
using Editor.Enums;

namespace Editor.Descriptors
{
    public class LayoutDescriptor : DescriptorBase
    {
        public static readonly LayoutDescriptor Simple, Pattern;
        private static readonly IDictionary<string, LayoutDescriptor> sDescriptorsByTypeNamespace;

        static LayoutDescriptor()
        {
            Simple = new LayoutDescriptor("Simple", LayoutType.Simple, "log4net.Layout.SimpleLayout");
            Pattern = new LayoutDescriptor("Pattern", LayoutType.Pattern, "log4net.Layout.PatternLayout");

            sDescriptorsByTypeNamespace = new Dictionary<string, LayoutDescriptor>
            {
                { Simple.TypeNamespace, Simple },
                { Pattern.TypeNamespace, Pattern }
            };
        }

        private LayoutDescriptor(string name, LayoutType type, string typeNamespace)
            : base(name)
        {
            Type = type;
            TypeNamespace = typeNamespace;
        }

        public static bool TryFindByTypeNamespace(string typeNamespace, out LayoutDescriptor layout)
        {
            if (typeNamespace == null)
            {
                layout = null;
                return false;
            }

            return sDescriptorsByTypeNamespace.TryGetValue(typeNamespace, out layout);
        }

        public LayoutType Type { get; }

        public string TypeNamespace { get; }
    }
}
