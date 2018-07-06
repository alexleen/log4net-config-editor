// Copyright © 2018 Alex Leendertsen

namespace Editor.Interfaces
{
    /// <summary>
    /// Strategy for saving an element.
    /// </summary>
    internal interface ISaveStrategy
    {
        /// <summary>
        /// Perform save.
        /// </summary>
        void Execute();
    }
}
