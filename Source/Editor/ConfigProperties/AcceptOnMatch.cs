// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;

namespace Editor.ConfigProperties
{
    internal class AcceptOnMatch : BooleanPropertyBase
    {
        public AcceptOnMatch(ReadOnlyCollection<IProperty> container)
            : base(container, "Accept on Match:", "acceptOnMatch", true, true)
        {
        }
    }
}
