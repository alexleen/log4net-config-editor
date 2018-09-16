// Copyright © 2018 Alex Leendertsen

namespace Editor.Interfaces
{
    public interface IConfigurationFactory
    {
        /// <summary>
        /// Creates an <see cref="IConfigurationXml"/> from the specified file.
        /// </summary>
        /// <param name="filename">Path to file</param>
        /// <returns></returns>
        IConfigurationXml Create(string filename);
    }
}
