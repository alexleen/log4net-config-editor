// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows;
using System.Xml;
using Editor.Models;
using Editor.Windows.Filters.Properties;

namespace Editor.Windows.Filters
{
    public class PropertyFilterWindow : StringMatchFilterWindow
    {
        public PropertyFilterWindow(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml, Action<FilterModel> add)
            : base(owner, filterModel, appenderNode, configXml, add, false)
        {
            ResizeMode = ResizeMode.CanResize;
            MinWidth = TextBoxWindowMinWidth;
            MinHeight = 173;
            MaxHeight = 173;
        }

        protected override void AddAppropriateProperties()
        {
            FilterProperties.Add(new Key(FilterProperties) { IsFocused = true });
            base.AddAppropriateProperties();
        }
    }
}
