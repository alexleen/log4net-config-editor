// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using System.Windows;
using Editor.Models;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public abstract class RefsBase : PropertyBase
    {
        protected RefsBase(ObservableCollection<IProperty> container, GridLength rowHeight)
            : base(container, rowHeight)
        {
            RefsCollection = new ObservableCollection<LoggerModel>();
        }

        public ObservableCollection<LoggerModel> RefsCollection { get; set; }
    }
}
