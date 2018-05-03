// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;

namespace Editor.Windows.Appenders.Properties
{
    public interface IAppenderProperty
    {
        /// <summary>
        /// Zero-based row index of this appender property.
        /// </summary>
        int RowIndex { get; }

        /// <summary>
        /// Row's height.
        /// </summary>
        GridLength RowHeight { get; }

        /// <summary>
        /// Loads existing data into property from specified appender node.
        /// </summary>
        /// <param name="originalAppenderNode"></param>
        void Load(XmlNode originalAppenderNode);

        /// <summary>
        /// Validates property values. Will display a message box to the user describing any issues.
        /// Returns true if successfully validated. False otherwise.
        /// </summary>
        /// <returns></returns>
        bool TryValidate();

        /// <summary>
        /// Saves this property to the specified appender node.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="newAppenderNode"></param>
        void Save(XmlDocument xmlDoc, XmlNode newAppenderNode);
    }
}
