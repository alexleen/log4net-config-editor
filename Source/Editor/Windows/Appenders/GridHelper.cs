// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Editor.Windows.Appenders.Properties;

namespace Editor.Windows.Appenders
{
    public class GridHelper
    {
        public static readonly DependencyProperty BindableRowsProperty = DependencyProperty.RegisterAttached("BindableRows",
                                                                                                             typeof(IEnumerable<IAppenderProperty>),
                                                                                                             typeof(GridHelper),
                                                                                                             new PropertyMetadata(new IAppenderProperty[] { }, BindableRowsChanged));

        public static IEnumerable<IAppenderProperty> GetBindableRows(DependencyObject obj)
        {
            return (IEnumerable<IAppenderProperty>)obj.GetValue(BindableRowsProperty);
        }

        public static void SetBindableRows(DependencyObject obj, IEnumerable<IAppenderProperty> value)
        {
            obj.SetValue(BindableRowsProperty, value);
        }

        public static void BindableRowsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Grid grid && e.NewValue is ObservableCollection<IAppenderProperty> rows)
            {
                new GridRowManager(grid, rows);
            }
        }

        private class GridRowManager
        {
            private readonly Grid mGrid;

            public GridRowManager(Grid grid, ObservableCollection<IAppenderProperty> rows)
            {
                mGrid = grid;
                rows.CollectionChanged += RowsOnCollectionChanged;
            }

            private void RowsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (IAppenderProperty gridRow in e.NewItems)
                    {
                        mGrid.RowDefinitions.Add(new RowDefinition { Height = gridRow.RowHeight });
                    }
                }
            }
        }
    }
}
