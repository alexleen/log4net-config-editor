// Copyright © 2018 Alex Leendertsen

using System.Windows;

namespace Editor.ConfigProperties.Base
{
    public abstract class RefsBase : PropertyBase
    {
        protected RefsBase(string name, string description)
            : base(new GridLength(1, GridUnitType.Star))
        {
            Name = name;
            Description = description;
        }

        public string Name { get; }

        public string Description { get; }
    }
}
