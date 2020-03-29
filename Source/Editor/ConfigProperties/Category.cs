// Copyright © 2020 Alex Leendertsen

using System.Xml;
using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Utilities;
using log4net.Layout;

namespace Editor.ConfigProperties
{
    /// <summary>
    /// Formats the category parameter sent to the Debug/Trace/AspNetTrace appenders.
    /// </summary>
    /// <remarks>
    /// Category is of type <see cref="PatternLayout"/>. This means that it is represented in XML similar to a layout:
    /// <category type="log4net.Layout.PatternLayout">
    ///    <conversionPattern value="%logger %date" />
    /// </category>
    /// </remarks>
    public class Category : StringValueProperty
    {
        private const string DefaultValue = "%logger";

        public Category()
            : base("Category:", "category")
        {
            Value = DefaultValue;
            ToolTip = "Formats the category parameter. Defaults to a PatternLayout with %logger as the pattern which will use the logger name of the current LoggingEvent as the category parameter.";
        }

        public override void Load(XmlNode originalNode)
        {
            string pattern = originalNode[ElementName]?.GetValueAttributeValueFromChildElement(Log4NetXmlConstants.ConversionPattern);

            if (!string.IsNullOrEmpty(pattern))
            {
                Value = pattern;
            }
        }

        public override void Save(XmlDocument xmlDoc, XmlNode newNode)
        {
            if (string.IsNullOrWhiteSpace(Value) || Value == DefaultValue)
            {
                return;
            }

            XmlNode categoryNode = xmlDoc.CreateElementWithAttribute(ElementName, Log4NetXmlConstants.Type, LayoutDescriptor.Pattern.TypeNamespace);
            xmlDoc.CreateElementWithValueAttribute(Log4NetXmlConstants.ConversionPattern, Value).AppendTo(categoryNode);
            newNode.AppendChild(categoryNode);
        }
    }
}
