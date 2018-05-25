// Copyright © 2018 Alex Leendertsen

using System.Collections.Generic;

namespace Editor.Windows.Appenders.Properties.PatternManager
{
    public interface IHistoricalPatternManager
    {
        /// <summary>
        /// Retrieves an ordered set of patterns (descending by date).
        /// Empty if no previous patterns exist.
        /// </summary>
        /// <returns></returns>
        IEnumerable<string> GetPatterns();

        /// <summary>
        /// Saves the specified pattern.
        /// </summary>
        /// <param name="pattern"></param>
        void SavePattern(string pattern);
    }
}
