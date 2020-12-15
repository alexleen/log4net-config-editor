// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;
using Editor.XML;

namespace Editor.ConfigProperties
{
    public class Target : PropertyBase
    {
        private const string ConsoleOut = "Console.Out";
        private const string ConsoleError = "Console.Error";
        private const string TargetName = "target";

        public Target()
            : base(GridLength.Auto)
        {
            Targets = new[] { ConsoleOut, ConsoleError };
            SelectedItem = ConsoleOut;
        }

        public IEnumerable<string> Targets { get; }

        public string SelectedItem { get; set; }

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(Log4NetXmlConstants.Value, out IValueResult result, TargetName) && !string.IsNullOrEmpty(result.AttributeValue) && Targets.Contains(result.AttributeValue))
            {
                SelectedItem = result.AttributeValue;
            }
        }

        public override void Save(IElementConfiguration config)
        {
            //Target is "Console.Out" by default, so we only need a target element if "Console.Error"
            if (SelectedItem == ConsoleError)
            {
                config.Save(new Element(TargetName, new[] { (Log4NetXmlConstants.Value, ConsoleError) }));
            }
        }
    }
}
