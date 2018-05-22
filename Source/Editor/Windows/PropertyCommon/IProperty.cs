// Copyright © 2018 Alex Leendertsen

using System.Windows;
using System.Xml;

namespace Editor.Windows.PropertyCommon
{
    /// <summary>
    /// Represents a log4net grid row used in either a Filter or Appender.
    /// </summary>
    public interface IProperty
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
        /// <param name="originalNode"></param>
        void Load(XmlNode originalNode);

        /// <summary>
        /// Validates property values. Will display a message box to the user describing any issues.
        /// Returns true if successfully validated. False otherwise.
        /// </summary>
        /// <param name="messageBoxService"></param>
        /// <returns></returns>
        bool TryValidate(IMessageBoxService messageBoxService);

        /// <summary>
        /// Saves this property to the specified appender node.
        /// </summary>
        /// <param name="xmlDoc"></param>
        /// <param name="newNode"></param>
        void Save(XmlDocument xmlDoc, XmlNode newNode);
    }
}
