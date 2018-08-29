// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class Identity : StringValueProperty
    {
        public Identity(ReadOnlyCollection<IProperty> container)
            : base(container, "Identity:", null)
        {
        }
    }
}
