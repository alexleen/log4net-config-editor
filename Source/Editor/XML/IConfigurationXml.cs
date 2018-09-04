using Editor.Descriptors.Base;
using Editor.Interfaces;
using Editor.Models;
using Editor.Models.Base;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Xml;

namespace Editor.XML
{
    public interface IConfigurationXml : IConfiguration
    {
        IElementConfiguration CreateElementConfigurationFor(XmlNode originalNode, string newElementName);

        /// <summary>
        /// Loads the log4net configuration from disk.
        /// </summary>
        void Load();

        /// <summary>
        /// Loads current state of <see cref="mConfigXml"/> into view.
        /// Overwrites any existing values in the view not saved to <see cref="mConfigXml"/>.
        /// </summary>
        /// <returns>true if an unrecognized/unsupported appender was found. null if configuration can not be loaded.</returns>
        bool? Reload();

        /// <summary>
        /// Saves the current log4net configuration to disk.
        /// </summary>
        void Save();

        /// <summary>
        /// Removes all refs (appender-ref) to the specified appender.
        /// </summary>
        /// <param name="appenderModel"></param>
        void RemoveRefsTo(AppenderModel appenderModel);

        /// <summary>
        /// Children of the log4net root element.
        /// </summary>
        ObservableCollection<ModelBase> Children { get; }
    }
}
