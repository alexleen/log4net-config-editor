// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using System.Collections.ObjectModel;

namespace Editor.ConfigProperties
{
    internal class StaticLogFileName : BooleanPropertyBase
    {
        public StaticLogFileName(ReadOnlyCollection<IProperty> container)
            : base(container, "Static Log File Name:", "staticLogFileName", false, false)
        {
        }
    }
}
