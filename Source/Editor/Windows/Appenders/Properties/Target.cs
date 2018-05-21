// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Xml;
using Editor.Utilities;
using Editor.Windows.PropertyCommon;

namespace Editor.Windows.Appenders.Properties
{
    public class Target : PropertyBase
    {
        private const string ConsoleOut = "Console.Out";
        private const string ConsoleError = "Console.Error";
        private const string TargetName = "target";

        public Target(ObservableCollection<IProperty> container)
            : base(container, GridLength.Auto)
        {
            Targets = new[] { ConsoleOut, ConsoleError };
            SelectedItem = ConsoleOut;
        }

        public IEnumerable<string> Targets { get; }

        public string SelectedItem { get; set; }

        public override void Load(XmlNode originalNode)
        {
            SelectedItem = originalNode.GetValueAttributeValueFromChildElement(TargetName) ?? ConsoleOut;
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            //Target is "Console.Out" by default, so we only need a target element if "Console.Error"
            if (SelectedItem == ConsoleError)
            {
                xmlDoc.CreateElementWithValueAttribute(TargetName, ConsoleError).AppendTo(newNode);
            }
        }
    }
}
