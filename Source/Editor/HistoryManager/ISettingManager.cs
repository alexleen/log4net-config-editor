// Copyright © 2018 Alex Leendertsen

using Editor.Properties;

namespace Editor.HistoryManager
{
    public interface ISettingManager<TSettinType>
    {
        TSettinType Get(string settingName);

        void Set(string settingName, TSettinType value);

        void Save();
    }

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
