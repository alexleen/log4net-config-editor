// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows;
using System.Xml;
using Editor.Models;
using Editor.Windows.Filters.Properties;

namespace Editor.Windows.Filters
{
    public class LoggerMatchFilterWindow : FilterWindowBase
    {
        public LoggerMatchFilterWindow(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml, Action<FilterModel> add)
            : base(owner, filterModel, appenderNode, configXml, add)
        {
            ResizeMode = ResizeMode.CanResize;
            MinWidth = TextBoxWindowMinWidth;
            MinHeight = 121;
            MaxHeight = 121;
        }

        protected override void AddAppropriateProperties()
        {
            FilterProperties.Add(new LoggerToMatch(FilterProperties) { IsFocused = true });
            FilterProperties.Add(new AcceptOnMatch(FilterProperties));
        }
    }
}
