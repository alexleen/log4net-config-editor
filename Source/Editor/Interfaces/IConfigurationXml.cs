// Copyright © 2019 Alex Leendertsen

using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Xml;
using Editor.Enums;
using Editor.Models.Base;
using Editor.Models.ConfigChildren;
using log4net.Core;

namespace Editor.Interfaces
{
    /// <summary>
    /// Encapsulation of a log4net XML configuration file.
    /// log4net configuration files are loaded from disk into an <see cref="XmlDocument"/> in RAM.
    /// Runtime changes to the configuration are saved to the <see cref="XmlDocument"/> and reloaded (via <see cref="Reload"/>) into view.
    /// When changes are complete, <see cref="XmlDocument"/> is saved (via <see cref="SaveAsync"/>) to disk as XML.
    /// </summary>
    public interface IConfigurationXml : IConfiguration, INotifyPropertyChanged
    {
        /// <summary>
        /// The current state of the XML compared to disk.
        /// </summary>
        SaveState SaveState { get; }

        /// <summary>
        /// Loads the log4net configuration from disk.
        /// </summary>
        void Load();

        /// <summary>
        /// Loads current state of the <see cref="XmlDocument"/> into view.
        /// </summary>
        /// <returns>true if an unrecognized/unsupported appender was found. null if no log4net element can be found.</returns>
        bool? Reload();

        /// <summary>
        /// Saves the current log4net configuration to disk.
        /// </summary>
        Task SaveAsync();

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
