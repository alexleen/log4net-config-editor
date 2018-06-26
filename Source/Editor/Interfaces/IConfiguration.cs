// Copyright © 2018 Alex Leendertsen

using System.Xml;

namespace Editor.Interfaces
{
    /// <summary>
    /// Provides access to various XML configuration items.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        /// Current configuration file as an XmlDocument.
        /// </summary>
        XmlDocument ConfigXml { get; }

        /// <summary>
        /// The log4net root configuration node.
        /// </summary>
        XmlNode Log4NetNode { get; }
    }
}
