// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using Editor.Models;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public abstract class RefsBase : PropertyBase
    {
        protected RefsBase(ObservableCollection<IProperty> container, string name, string description)
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
