// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;

namespace Editor.Windows.PropertyCommon
{
    public class GridHelper
    {
        public static readonly DependencyProperty BindableRowsProperty = DependencyProperty.RegisterAttached("BindableRows",
                                                                                                             typeof(IEnumerable<IProperty>),
                                                                                                             typeof(GridHelper),
                                                                                                             new PropertyMetadata(new IProperty[] { }, BindableRowsChanged));

        public static IEnumerable<IProperty> GetBindableRows(DependencyObject obj)
        {
            return (IEnumerable<IProperty>)obj.GetValue(BindableRowsProperty);
        }

        public static void SetBindableRows(DependencyObject obj, IEnumerable<IProperty> value)
        {
            obj.SetValue(BindableRowsProperty, value);
        }

        public static void BindableRowsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Grid grid && e.NewValue is ObservableCollection<IProperty> rows)
            {
                new GridRowManager(grid, rows);
            }
        }

        private class GridRowManager
        {
            private readonly Grid mGrid;

            public GridRowManager(Grid grid, INotifyCollectionChanged rows)
            {
                mGrid = grid;
                rows.CollectionChanged += RowsOnCollectionChanged;
            }

            private void RowsOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
            {
                if (e.Action == NotifyCollectionChangedAction.Add)
                {
                    foreach (IProperty gridRow in e.NewItems)
                    {
                        mGrid.RowDefinitions.Add(new RowDefinition { Height = gridRow.RowHeight });
                    }
                }
            }
        }
    }
}
