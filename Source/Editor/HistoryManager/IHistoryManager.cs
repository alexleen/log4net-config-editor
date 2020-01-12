// Copyright © 2020 Alex Leendertsen

using System.Collections.Generic;

namespace Editor.HistoryManager
{
    public interface IHistoryManager
    {
        /// <summary>
        /// Retrieves an ordered set of historical values (descending by date).
        /// Empty if no previous values exist.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> Get();

        /// <summary>
        /// Saves the specified value.
        /// Will not save duplicate values.
        /// </summary>
        /// <param name="value"></param>
        void Save(string value);
    }
}
