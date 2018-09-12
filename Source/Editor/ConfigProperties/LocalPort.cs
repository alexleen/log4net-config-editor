// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class LocalPort : Port
    {
        internal LocalPort(ReadOnlyCollection<IProperty> container)
            : base(container, "Local Port:", "localPort")
        {
        }
    }
}
