// Copyright © 2018 Alex Leendertsen

using SystemInterface.Xml;

namespace Editor.Interfaces
{
    /// <summary>
    /// I can load and even save XML!
    /// </summary>
    internal interface ICanLoadAndSaveXml
    {
        /// <summary>
        /// Loads the XML file from disk into an instance of an <see cref="IXmlDocument"/>.
        /// </summary>
        /// <returns></returns>
        IXmlDocument Load();

        /// <summary>
        /// Saves the XML to disk.
        /// </summary>
        void Save();
    }
}
