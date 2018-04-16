// Copyright © 2018 Alex Leendertsen

using Editor.Enums;
using Editor.Models;

namespace Editor.Descriptors
{
    public class LayoutDescriptor
    {
        public static LayoutDescriptor Simple, Pattern;

        static LayoutDescriptor()
        {
            Simple = new LayoutDescriptor("Simple", LayoutType.Simple);
            Pattern = new LayoutDescriptor("Pattern", LayoutType.Pattern);
        }

        public LayoutDescriptor(string name, LayoutType type)
        {
            Name = name;
            Type = type;
        }

        public static bool TryFindByTypeNamespace(string typeNamespace, out LayoutDescriptor layout)
        {
            switch (typeNamespace)
            {
                case "log4net.Layout.PatternLayout":
                    layout = Pattern;
                    break;
                case "log4net.Layout.SimpleLayout":
                    layout = Simple;
                    break;
                default:
                    layout = null;
                    return false;
            }

            return true;
        }

        public string Name { get; }

        public LayoutType Type { get; }
    }
}
