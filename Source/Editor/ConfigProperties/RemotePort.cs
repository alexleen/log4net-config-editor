// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class RemotePort : Port
    {
        internal RemotePort(ReadOnlyCollection<IProperty> container)
            : base(container, "Remote Port:", "remotePort")
        {
        }
    }
}
