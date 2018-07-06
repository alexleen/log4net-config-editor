// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Editor.Controls
{
    /// <summary>
    /// Interaction logic for DropDownButton.xaml
    /// </summary>
    public partial class DropDownButton
    {
        public DropDownButton()
        {
            InitializeComponent();
            ItemTemplate = (DataTemplate)Resources["DefaultDataTemplate"];
        }

        private void AddAppenderOnClick(object sender, RoutedEventArgs e)
        {
            xButton.ContextMenu.IsEnabled = true;
            xButton.ContextMenu.PlacementTarget = xButton;
            xButton.ContextMenu.Placement = PlacementMode.Bottom;
            xButton.ContextMenu.IsOpen = true;
        }

        private void AddAppenderItemOnClick(object sender, MouseButtonEventArgs e)
        {
            object dataContext = ((FrameworkElement)sender).DataContext;
            ItemClick?.Invoke(dataContext);
            Command?.Execute(dataContext);
        }

        public string ButtonName { get; set; }

        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(IEnumerable<object>), typeof(DropDownButton));

        public IEnumerable<object> ItemsSource
        {
            get => (IEnumerable<object>)GetValue(ItemsSourceProperty);
            set => SetValue(ItemsSourceProperty, value);
        }

        public event Action<object> ItemClick;

        public static readonly DependencyProperty CommandProperty = DependencyProperty.Register("Command", typeof(ICommand), typeof(DropDownButton));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(DropDownButton));

        public DataTemplate ItemTemplate
        {
            get => (DataTemplate)GetValue(ItemTemplateProperty);
            set => SetValue(ItemTemplateProperty, value);
        }

        public double ContextMenuWidth
        {
            get
            {
                if (OverrideContextMenuWidth)
                {
                    return double.NaN;
                }

                //For some reason, the context menu is 5 pixels smaller than the button
                const int contextMenuExtraWidth = 5;

                if (double.IsNaN(xButton.Width))
                {
                    return Width + contextMenuExtraWidth;
                }

                return xButton.Width + contextMenuExtraWidth;
            }
        }

        public bool OverrideContextMenuWidth { get; set; }
    }
}
