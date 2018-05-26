// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;

namespace Editor.HistoryManager
{
    public interface IHistoryManager
    {
        /// <summary>
        /// Retrieves an ordered set of patterns (descending by date).
        /// Empty if no previous patterns exist.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> Get();

        /// <summary>
        /// Saves the specified pattern.
        /// </summary>
        /// <param name="value"></param>
        void Save(string value);
    }
}
