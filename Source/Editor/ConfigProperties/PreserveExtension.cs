// Copyright © 2018 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using System.Collections.ObjectModel;

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
