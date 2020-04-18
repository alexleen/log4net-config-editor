// Copyright © 2020 Alex Leendertsen

using System.Linq;
using System.Windows;
using Editor.ConfigProperties.Base;
using Editor.Interfaces;
using Editor.Utilities;

namespace Editor.ConfigProperties
{
    internal class Encoding : MultiValuePropertyBase<string>
    {
        private static readonly string[] sValues =
        {
            string.Empty,
            System.Text.Encoding.ASCII.WebName,
            System.Text.Encoding.Unicode.WebName,
            System.Text.Encoding.BigEndianUnicode.WebName,
            System.Text.Encoding.UTF7.WebName,
            System.Text.Encoding.UTF8.WebName,
            System.Text.Encoding.UTF32.WebName
        };

        private readonly string mElementName;

        public Encoding(string name, string elementName)
            : base(GridLength.Auto, name, sValues, 75)
        {
            SelectedValue = sValues[0];
            mElementName = elementName;
        }

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(mElementName, Log4NetXmlConstants.Value, out IValueResult result) && sValues.Contains(result.AttributeValue))
            {
                SelectedValue = result.AttributeValue;
            }
        }

        public override void Save(IElementConfiguration config)
        {
            if (!string.IsNullOrEmpty(SelectedValue))
            {
                config.Save(mElementName, Log4NetXmlConstants.Value, SelectedValue);
            }
        }
    }
}
