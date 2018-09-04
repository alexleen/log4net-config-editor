using System.Xml;

namespace Editor.Interfaces
{
    /// <summary>
    /// Marker interface for elements that can contain "appender-ref" children (e.g. loggers and async appenders).
    /// </summary>
    public interface IAcceptAppenderRef
    {
        /// <summary>
        /// Node for the appender-ref location.
        /// </summary>
        XmlNode Node { get; }

        /// <summary>
        /// Whether or not this location is checked in the UI.
        /// </summary>
        bool IsEnabled { get; set; }
    }
}
