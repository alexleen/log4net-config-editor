// Copyright © 2020 Alex Leendertsen

using System.Linq;
using System.Windows;
using Editor.Interfaces;
using Editor.Utilities;
using Editor.XML;
using log4net.Core;

namespace Editor.ConfigProperties.Base
{
    internal class LevelPropertyBase : MultiValuePropertyBase<string>
    {
        private readonly string mElementName;

        internal LevelPropertyBase(string name, string elementName, bool prependEmpty = false)
            : base(GridLength.Auto, name, prependEmpty ? new[] { string.Empty }.Concat(Log4NetUtilities.LevelsByName.Keys) : Log4NetUtilities.LevelsByName.Keys, 100)
        {
            mElementName = elementName;
            SelectedValue = Values.First();
        }

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(Log4NetXmlConstants.Value, out IValueResult result, mElementName) && Log4NetUtilities.TryParseLevel(result.AttributeValue, out Level match))
            {
                SelectedValue = match.Name;
            }
        }

        public override void Save(IElementConfiguration config)
        {
            if (!string.IsNullOrEmpty(SelectedValue))
            {
                config.Save(new Element(mElementName, new[] { (Log4NetXmlConstants.Value, SelectedValue) }));
            }
        }
    }
}
