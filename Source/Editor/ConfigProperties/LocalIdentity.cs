// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    internal class LocalIdentity : StringValueProperty
    {
        public LocalIdentity(ReadOnlyCollection<IProperty> container)
            : base(container, "Identity:", Log4NetXmlConstants.Identity)
        {
        }
    }
}
