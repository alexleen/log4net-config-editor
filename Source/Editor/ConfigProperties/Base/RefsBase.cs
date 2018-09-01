// Copyright © 2018 Alex Leendertsen

using Editor.Interfaces;
using System.Collections.ObjectModel;
using System.Windows;

namespace Editor.ConfigProperties.Base
{
    public abstract class RefsBase : PropertyBase
    {
        protected RefsBase(ReadOnlyCollection<IProperty> container, string name, string description)
            : base(container, new GridLength(1, GridUnitType.Star))
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }

        public string Description { get; }
    }
}
