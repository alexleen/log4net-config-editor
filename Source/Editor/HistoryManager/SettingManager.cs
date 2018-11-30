// Copyright © 2018 Alex Leendertsen

using Editor.Properties;

namespace Editor.HistoryManager
{
    internal class SettingManager<TSettingType> : ISettingManager<TSettingType>
    {
        public SettingManager()
        {
            if (Settings.Default.UpgradeRequired)
            {
                Settings.Default.Upgrade();
                Settings.Default.UpgradeRequired = false;
                Settings.Default.Save();
            }
        }

        public TSettingType Get(string settingName)
        {
            return (TSettingType)Settings.Default[settingName];
        }

        public void Set(string settingName, TSettingType value)
        {
            Settings.Default[settingName] = value;
        }

        public void Save()
        {
            Settings.Default.Save();
        }
    }
}
