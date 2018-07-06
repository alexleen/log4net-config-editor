// Copyright © 2018 Alex Leendertsen

using System.Collections.ObjectModel;

namespace Editor.Interfaces
{
    /// <summary>
    /// Defines a log4net element.
    /// </summary>
    public interface IElementDefinition
    {
        /// <summary>
        /// Display name for this element.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Window icon path for this element.
        /// </summary>
        string Icon { get; }

        /// <summary>
        /// Loads appropriate properties into <see cref="Properties"/>.
        /// </summary>
        void Initialize();

        /// <summary>
        /// Properties of this element. May change based on other properties' states.
        /// </summary>
        ReadOnlyObservableCollection<IProperty> Properties { get; }

        /// <summary>
        /// Provides access to view services.
        /// </summary>
        IMessageBoxService MessageBoxService { get; set; }
    }
}
