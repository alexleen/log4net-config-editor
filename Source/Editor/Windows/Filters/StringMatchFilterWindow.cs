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
        private readonly StringMatch mStringMatch;
        private readonly RegexMatch mRegexMatch;

        public StringMatchFilterWindow(Window owner, FilterModel filterModel, XmlNode appenderNode, XmlDocument configXml, Action<FilterModel> add, bool isFocused = true)
            : base(owner, filterModel, appenderNode, configXml, add)
        {
            ResizeMode = ResizeMode.CanResize;
            MinWidth = TextBoxWindowMinWidth;
            MinHeight = 147;
            MaxHeight = 147;
            mStringMatch = new StringMatch(FilterProperties, Validate) { IsFocused = isFocused };
            mRegexMatch = new RegexMatch(FilterProperties, Validate);
        }

        protected override void AddAppropriateProperties()
        {
            FilterProperties.Add(mStringMatch);
            FilterProperties.Add(mRegexMatch);
            FilterProperties.Add(new AcceptOnMatch(FilterProperties));
        }

        private bool Validate()
        {
            if (string.IsNullOrEmpty(mStringMatch.Value) && string.IsNullOrEmpty(mRegexMatch.Value))
            {
                MessageBox.Show("Either 'String to Match' or 'Regex to Match' must be specified.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}
