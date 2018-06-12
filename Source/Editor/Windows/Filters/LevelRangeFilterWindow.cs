// Copyright © 2018 Alex Leendertsen

using System;
using System.Xml;
using Editor.Models;
using Editor.Windows.Filters.Properties;

namespace Editor.Windows.Filters
{
    public class LevelRangeFilterWindow : FilterWindowBase
    {
        public LevelRangeFilterWindow(FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml, Action<FilterModel> add)
            : base(filterModel, appenderNode, configXml, add)
        {
        }

        protected override void AddAppropriateProperties()
        {
            FilterProperties.Add(new MinLevel(FilterProperties));
            FilterProperties.Add(new MaxLevel(FilterProperties));
            FilterProperties.Add(new AcceptOnMatch(FilterProperties));
        }
    }
}
