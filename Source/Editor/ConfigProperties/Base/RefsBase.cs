// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using Editor.Interfaces;
using Editor.Models;

namespace Editor.ConfigProperties.Base
{
    public abstract class RefsBase : PropertyBase
    {
        protected RefsBase(ReadOnlyCollection<IProperty> container, string name, string description)
            : base(container, new GridLength(1, GridUnitType.Star))
        {
            Name = name;
            Description = description;
            RefsCollection = new ObservableCollection<LoggerModel>();
        }

        public string Name { get; }

        public string Description { get; }

        public ObservableCollection<LoggerModel> RefsCollection { get; set; }
    }
}
