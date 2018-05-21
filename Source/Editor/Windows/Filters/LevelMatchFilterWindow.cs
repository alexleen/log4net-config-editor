// Copyright © 2018 Alex Leendertsen

using System;
using System.Windows;
using System.Xml;
using Editor.Models;
using Editor.Windows.Filters.Properties;

namespace Editor.Windows.Filters
{
    public class LevelMatchFilterWindow : FilterWindowBase
    {
        public LevelMatchFilterWindow(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml, Action<FilterModel> add)
            : base(owner, filterModel, appenderNode, configXml, add)
        {
        }

        protected override void AddAppropriateProperties()
        {
            FilterProperties.Add(new LevelToMatch(FilterProperties));
            FilterProperties.Add(new AcceptOnMatch(FilterProperties));
        }
    }
}
