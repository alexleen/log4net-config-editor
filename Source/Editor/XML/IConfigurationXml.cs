using Editor.Interfaces;
using Editor.Models;
using Editor.Models.Base;
using System.Collections.ObjectModel;

namespace Editor.XML
{
    public interface IConfigurationXml : IConfiguration
    {
        /// <summary>
        /// Removes the specified child from XML and <see cref="Children"/> collection.
        /// </summary>
        /// <param name="child"></param>
        void RemoveChild(ModelBase child);

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
        /// Creates an element configuration.
        /// </summary>
        /// <param name="originalModel">Original model if editing. Null if new element.</param>
        /// <param name="newElementName">Name of new element (even if editing - in which case it'll be the same as the original)</param>
        /// <returns></returns>
        IElementConfiguration CreateElementConfigurationFor(ModelBase originalModel, string newElementName);

        /// <summary>
        /// Children of the log4net root element.
        /// </summary>
        ReadOnlyObservableCollection<ModelBase> Children { get; }
    }
}
