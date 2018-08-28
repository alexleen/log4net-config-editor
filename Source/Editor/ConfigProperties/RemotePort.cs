using Editor.Interfaces;
using System.Collections.ObjectModel;

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
