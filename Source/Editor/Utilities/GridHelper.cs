// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Windows;
using System.Windows.Controls;
using Editor.Interfaces;

namespace Editor.Utilities
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

        private static void BindableRowsChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            if (obj is Grid grid && e.NewValue is ReadOnlyObservableCollection<IProperty> rows)
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
                    int i = e.NewStartingIndex;
                    foreach (IProperty gridRow in e.NewItems)
                    {
                        mGrid.RowDefinitions.Insert(i++, new RowDefinition { Height = gridRow.RowHeight });
                    }
                }
                else if (e.Action == NotifyCollectionChangedAction.Remove)
                {
                    for (int i = e.OldStartingIndex; i < e.OldItems.Count + e.OldStartingIndex; i++)
                    {
                        mGrid.RowDefinitions.RemoveAt(i);
                    }
                }
            }
        }
    }
}
