// Copyright © 2018 Alex Leendertsen

using Editor.Properties;

namespace Editor.HistoryManager
{
    /// <summary>
    /// Provides access to <see cref="Settings.Default"/>.
    /// </summary>
    /// <typeparam name="TSettinType">Type of setting</typeparam>
    public interface ISettingManager<TSettinType>
    {
        /// <summary>
        /// Retrieves the value of the specified setting.
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        TSettinType Get(string settingName);

        /// <summary>
        /// Sets the specified setting to the specified value.
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="value"></param>
        void Set(string settingName, TSettinType value);

        /// <summary>
        /// Saves all settings.
        /// </summary>
        void Save();
    }
}
