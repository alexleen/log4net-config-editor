// Copyright © 2018 Alex Leendertsen

using Editor.Enums;

namespace Editor.Descriptors
{
    public class FilterDescriptor
    {
        public static FilterDescriptor DenyAll, LevelMatch, LevelRange, LoggerMatch, Mdc, Ndc, Property, String;

        static FilterDescriptor()
        {
            DenyAll = new FilterDescriptor("Deny All", FilterType.DenyAll);
        }

        public FilterDescriptor(string name, FilterType type)
        {
            Name = name;
            Type = type;
        }

        public static bool TryFindByTypeNamespace(string typeNamespace, out FilterDescriptor filter)
        {
            switch (typeNamespace)
            {
                case "log4net.Filter.DenyAllFilter":
                    filter = DenyAll;
                    break;
                case "log4net.Filter.LoggerMatchFilter":
                    filter = LoggerMatch;
                    break;
                default:
                    filter = null;
                    break;
            }

            return false;
        }

        public string Name { get; }

        public FilterType Type { get; }
    }
}
