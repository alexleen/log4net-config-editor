// Copyright © 2018 Alex Leendertsen

using System.Xml;

namespace Editor.Interfaces
{
    /// <summary>
    /// Element that can contain "appender-ref" children (e.g. loggers and async appenders).
    /// </summary>
    public interface IAcceptAppenderRef
    {
        /// <summary>
        /// Node accepting the appender-ref.
        /// This is not the appender-ref itself.
        /// </summary>
        XmlNode Node { get; }

        /// <summary>
        /// Whether or not this location is checked in the UI.
        /// </summary>
        bool IsEnabled { get; set; }
    }
}
