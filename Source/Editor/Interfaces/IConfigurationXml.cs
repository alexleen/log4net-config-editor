// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;
using Editor.Enums;
using Editor.Models.Base;
using Editor.Models.ConfigChildren;
using log4net.Core;

namespace Editor.Interfaces
{
    public interface IConfigurationXml : IConfiguration
    {
        /// <summary>
        /// Resets this configuration to an empty log4net configuration (only log4net root element).
        /// Does NOT save to disk.
        /// </summary>
        //void CreateNew();

        /// <summary>
        /// Loads the log4net configuration from disk.
        /// </summary>
        void Load();

        /// <summary>
        /// Loads current state of <see cref="mConfigXml"/> into view.
        /// Overwrites any existing values in the view not saved to <see cref="mConfigXml"/>.
        /// Does NOT reload from disk.
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
        /// Removes the specified child from XML and <see cref="Children"/> collection.
        /// </summary>
        /// <param name="child"></param>
        void RemoveChild(ModelBase child);

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

        /// <summary>
        /// Configuration wide threshold.
        /// </summary>
        Level Threshold { get; set; }

        /// <summary>
        /// Whether this configuration merges or overwrites an existing configuration.
        /// </summary>
        Update Update { get; set; }

        /// <summary>
        /// Whether or not log4net is running in debug mode.
        /// </summary>
        bool Debug { get; set; }
    }
}
