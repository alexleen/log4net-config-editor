// Copyright © 2018 Alex Leendertsen

using Editor.Properties;

namespace Editor.HistoryManager
{
    internal class SettingManager<TSettinType> : ISettingManager<TSettinType>
    {
        public TSettinType Get(string settingName)
        {
            return (TSettinType)Settings.Default[settingName];
        }

        public void Set(string settingName, TSettinType value)
        {
            Settings.Default[settingName] = value;
        }

        public void Save()
        {
            Settings.Default.Save();
        }
    }
}
