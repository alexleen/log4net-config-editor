// Copyright © 2020 Alex Leendertsen

using System.Xml;

namespace Editor.Interfaces
{
    /// <summary>
    /// Encapsulates two nodes to facilitate the loading and saving of new elements.
    /// Typically, the UI is loaded with the <see cref="OriginalNode"/> and values
    /// are saved to the <see cref="NewNode"/>.
    /// </summary>
    public interface IElementConfiguration : IConfiguration
    {
        /// <summary>
        /// The original node, if editing. If not, this is null.
        /// </summary>
        XmlNode OriginalNode { get; }

        /// <summary>
        /// Attempts to load the specified attribute from the original node (e.g. when loading an existing element).
        /// Case insensitive. Should not be called if no original node exists.
        /// </summary>
        /// <param name="attributeName">Name of the desired attribute</param>
        /// <param name="result">The value of the desired attribute as well as it's original name</param>
        /// <returns>False if not found, true otherwise</returns>
        bool Load(string attributeName, out IValueResult result);

        /// <summary>
        /// Attempts to load the specified attribute value from the specified child element of the original node (appender).
        /// Should not be called if no original node exists.
        /// </summary>
        /// <param name="elementName">Name of the child element where the desired attribute resides</param>
        /// <param name="attributeName">Name of the desired attribute</param>
        /// <param name="result">The value of the desired attribute as well as it's original name</param>
        /// <returns>False if not found, true otherwise</returns>
        bool Load(string elementName, string attributeName, out IValueResult result);

        /// <summary>
        /// The new node that will replace the original (if it exists).
        /// </summary>
        XmlNode NewNode { get; }

        /// <summary>
        /// Saves the specified value as an attribute with the specified name to the new node (the root node - not a child).
        /// </summary>
        /// <param name="attributeName">Name of the desired attribute</param>
        /// <param name="value">Attribute value</param>
        void Save(string attributeName, string value);

        /// <summary>
        /// Saves the specified value as an attribute with the specified name to a child element specified by element name.
        /// Child element with attribute is automatically created and appended to the new node.
        /// </summary>
        /// <param name="elementName">Name of the desired child element</param>
        /// <param name="attributeName">Name of the desired attribute</param>
        /// <param name="value">Attribute value</param>
        void Save(string elementName, string attributeName, string value);
    }
}
