// Copyright © 2020 Alex Leendertsen

namespace Editor.HistoryManager
{
    public partial class HistoryManagerFactory : IHistoryManagerFactory
    {
        private readonly ISettingManager<string> mSettingManager;

        public HistoryManagerFactory(ISettingManager<string> settingManager)
        {
            mSettingManager = settingManager;
        }

        public IHistoryManager CreateConfigHistoryManager()
        {
            return new HistoryManager("HistoricalConfigs", mSettingManager, 5);
        }

        public IHistoryManager CreatePatternsHistoryManager()
        {
            return new HistoryManager("HistoricalPatterns", mSettingManager, 5);
        }

        public IHistoryManager CreateFilePathHistoryManager()
        {
            return new HistoryManager("HistoricalFiles", mSettingManager, 5);
        }
    }
}
