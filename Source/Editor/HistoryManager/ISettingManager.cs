// Copyright © 2020 Alex Leendertsen

using Editor.Properties;

namespace Editor.HistoryManager
{
    /// <summary>
    /// Provides access to <see cref="Settings.Default"/>.
    /// </summary>
    /// <typeparam name="TSettingType">Type of setting</typeparam>
    public interface ISettingManager<TSettingType>
    {
        /// <summary>
        /// Retrieves the value of the specified setting.
        /// </summary>
        /// <param name="settingName"></param>
        /// <returns></returns>
        TSettingType Get(string settingName);

        /// <summary>
        /// Sets the specified setting to the specified value.
        /// </summary>
        /// <param name="settingName"></param>
        /// <param name="value"></param>
        void Set(string settingName, TSettingType value);

        /// <summary>
        /// Saves all settings.
        /// </summary>
        void Save();
    }
}
