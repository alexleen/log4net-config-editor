using Editor.Interfaces;
using System.Collections.ObjectModel;

namespace Editor.ConfigProperties
{
    internal class LocalPort : Port
    {
        internal LocalPort(ReadOnlyCollection<IProperty> container)
            : base(container, "Local Port:")
        {
        }
    }
}
