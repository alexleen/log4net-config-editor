// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows;
using System.Xml;
using Editor.Models;
using Editor.Windows.Filters.Properties;

namespace Editor.Windows.Filters
{
    public class StringMatchFilterWindow : FilterWindowBase
    {
        public StringMatchFilterWindow(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml, Action<FilterModel> add)
            : base(owner, filterModel, appenderNode, configXml, add)
        {
            ResizeMode = ResizeMode.CanResize;
            MinWidth = TextBoxWindowMinWidth;
            MinHeight = 147;
            MaxHeight = 147;
        }

        protected override void AddAppropriateProperties()
        {
            FilterProperties.Add(new StringMatch(FilterProperties) { IsFocused = true });
            FilterProperties.Add(new RegexMatch(FilterProperties));
            FilterProperties.Add(new AcceptOnMatch(FilterProperties));
        }
    }
}
