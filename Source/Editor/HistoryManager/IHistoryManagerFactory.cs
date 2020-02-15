// Copyright © 2020 Alex Leendertsen

namespace Editor.HistoryManager
{
    public interface IHistoryManagerFactory
    {
        /// <summary>
        /// Creates a history manager for configuration files.
        /// </summary>
        /// <returns></returns>
        IHistoryManager CreateConfigHistoryManager();

        /// <summary>
        /// Creates a history manager for layout patterns.
        /// </summary>
        /// <returns></returns>
        IHistoryManager CreatePatternsHistoryManager();

        /// <summary>
        /// Creates a history manager for log file paths.
        /// </summary>
        /// <returns></returns>
        IHistoryManager CreateFilePathHistoryManager();
    }
}
