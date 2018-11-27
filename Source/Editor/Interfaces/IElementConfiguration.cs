// Copyright © 2018 Alex Leendertsen

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
        /// The new node that will replace the original (if it exists).
        /// </summary>
        XmlNode NewNode { get; }
    }
}
