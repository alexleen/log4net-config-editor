// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Windows;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    public class CountDirection : PropertyBase
    {
        private const string Lower = "Lower";
        private const string Higher = "Higher";
        private const string CountDirectionName = "countDirection";

        public CountDirection()
            : base(GridLength.Auto)
        {
            Directions = new[] { Lower, Higher };
            SelectedDirection = Lower;
        }

        public IEnumerable<string> Directions { get; }

        public string SelectedDirection { get; set; }

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(Log4NetXmlConstants.Value, out IValueResult result, CountDirectionName) && int.TryParse(result.AttributeValue, out int value))
            {
                SelectedDirection = value >= 0 ? Higher : Lower;
            }
        }

        public override void Save(IElementConfiguration config)
        {
            if (SelectedDirection == Higher)
            {
                config.Save((CountDirectionName, Log4NetXmlConstants.Value, 0.ToString()));
            }
        }
    }
}
