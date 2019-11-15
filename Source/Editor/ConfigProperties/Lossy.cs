// Copyright © 2019 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class Lossy : BooleanPropertyBase
    {
        public Lossy(ReadOnlyCollection<IProperty> container)
            : base(container, "Lossy:", "lossy", false, false)
        {
        }
    }
}
