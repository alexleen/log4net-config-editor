// Copyright © 2020 Alex Leendertsen

using Editor.ConfigProperties.Base;
using Editor.Descriptors;
using Editor.Interfaces;
using Editor.Utilities;
using Editor.XML;
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

        public override void Load(IElementConfiguration config)
        {
            if (config.Load(Log4NetXmlConstants.Value, out IValueResult result, ElementName, Log4NetXmlConstants.ConversionPattern) && !string.IsNullOrEmpty(result.AttributeValue))
            {
                Value = result.AttributeValue;
            }
        }

        public override void Save(IElementConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(Value) || Value == DefaultValue)
            {
                return;
            }

            config.SaveHierarchical(
                new Element(ElementName, new[] { (Log4NetXmlConstants.Type, LayoutDescriptor.Pattern.TypeNamespace) }),
                new Element(Log4NetXmlConstants.ConversionPattern, new[] { (Log4NetXmlConstants.Value, Value) })
            );
        }
    }
}
