// Copyright © 2019 Alex Leendertsen

using System;
using System.Windows;
using System.Windows.Controls;
using Editor.ConfigProperties.Base;

namespace Editor.Windows
{
    internal class Selector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            if (IsMultiValueType(item?.GetType()))
            {
                return ((FrameworkElement)container).FindResource("MultiValueTemplate") as DataTemplate;
            }

            return base.SelectTemplate(item, container);
        }

        private static bool IsMultiValueType(Type t)
        {
            if (t is null)
            {
                return false;
            }

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(MultiValuePropertyBase<>))
            {
                return true;
            }

            return IsMultiValueType(t.BaseType);
        }
    }
}
