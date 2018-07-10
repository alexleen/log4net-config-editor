// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class PreserveExtension : BooleanPropertyBase
    {
        public PreserveExtension(ReadOnlyCollection<IProperty> container)
            : base(container, "Preserve Extension:", "preserveLogFileNameExtension", false, false)
        {
        }
    }
}
