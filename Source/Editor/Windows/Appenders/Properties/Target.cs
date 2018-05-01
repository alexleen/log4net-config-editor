// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;

namespace Editor.Windows.Appenders.Properties
{
    public class Target : AppenderPropertyBase
    {
        private const string ConsoleOut = "Console.Out";
        private const string ConsoleError = "Console.Error";

        public Target(ObservableCollection<IAppenderProperty> container, GridLength rowHeight)
            : base(container, rowHeight)
        {
            Targets = new[] { ConsoleOut, ConsoleError };
        }

        public IEnumerable<string> Targets { get; }

        public string SelectedItem { get; set; }

        public override void Load(XmlNode appenderNode)
        {
            string target = appenderNode["target"]?.Attributes["value"]?.Value;
            SelectedItem = target ?? ConsoleOut;
        }

        public override void Save(XmlDocument xmlDoc, XmlNode appenderNode)
        {
            //Target is "Console.Out" by default, so we only need a target element if "Console.Error"
            if (SelectedItem == ConsoleError)
            {
                xmlDoc.CreateElementWithAttribute("target", "value", ConsoleError).AppendTo(appenderNode);
            }
        }
    }
}
