// Copyright © 2020 Alex Leendertsen

using System.Windows;

namespace Editor.Interfaces
{
    /// <summary>
    /// Represents a log4net grid row used in either a Filter or Appender.
    /// </summary>
    public interface IProperty
    {
        /// <summary>
        /// Zero-based row index of this appender property.
        /// </summary>
        int RowIndex { get; set; }

        /// <summary>
        /// Row's height.
        /// </summary>
        GridLength RowHeight { get; }

        /// <summary>
        /// Validates property values. Will display a message box to the user describing any issues.
        /// Returns true if successfully validated. False otherwise.
        /// </summary>
        /// <param name="messageBoxService"></param>
        /// <returns></returns>
        bool TryValidate(IMessageBoxService messageBoxService);

        void Load(IElementConfiguration config);

        void Save(IElementConfiguration config);
    }
}
