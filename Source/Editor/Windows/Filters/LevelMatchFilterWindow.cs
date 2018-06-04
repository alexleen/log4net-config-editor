// Copyright © 2018 Alex Leendertsen

using System;
using System.Xml;
using Editor.Models;
using Editor.Windows.Filters.Properties;

namespace Editor.Windows.Filters
{
    public class LevelMatchFilterWindow : FilterWindowBase
    {
        public LevelMatchFilterWindow(FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml, Action<FilterModel> add)
            : base(filterModel, appenderNode, configXml, add)
        {
        }

        protected override void AddAppropriateProperties()
        {
            FilterProperties.Add(new LevelToMatch(FilterProperties));
            FilterProperties.Add(new AcceptOnMatch(FilterProperties));
        }
    }
}
