// Copyright © 2020 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Editor.HistoryManager
{
    public partial class HistoryManagerFactory
    {
        private class HistoryManager : IHistoryManager
        {
            private readonly string mSettingName;
            private readonly ISettingManager<string> mSettingManager;
            private readonly int mLimit;

            public HistoryManager(string settingName, ISettingManager<string> settingManager, int limit)
            {
                mSettingName = settingName;
                mSettingManager = settingManager;
                mLimit = limit;
            }

            public IEnumerable<string> Get()
            {
                History history = Read();

                return history != null ? history.Values.OrderByDescending(p => p.LastUsage).Select(p => p.Value) : new string[] { };
            }

            private History Read()
            {
                History history = null;

                if (!string.IsNullOrEmpty(mSettingManager.Get(mSettingName)))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(History));

                    using (TextReader reader = new StringReader(mSettingManager.Get(mSettingName)))
                    {
                        history = (History)serializer.Deserialize(reader);
                    }
                }

                return history;
            }

            public void Save(string value)
            {
                HistoricalValue newValue = new HistoricalValue { LastUsage = DateTime.UtcNow, Value = value };

                List<HistoricalValue> values = new List<HistoricalValue> { newValue };

                History history = Read();

                if (history == null)
                {
                    history = new History { Values = values.ToArray() };
                }
                else
                {
                    values.AddRange(history.Values);
                    history.Values = values.Distinct().OrderByDescending(p => p.LastUsage).Take(mLimit).ToArray();
                }

                Save(history);
            }

            private void Save(History history)
            {
                StringBuilder sb = new StringBuilder();
                XmlSerializer serializer = new XmlSerializer(typeof(History));

                using (TextWriter writer = new StringWriter(sb))
                {
                    serializer.Serialize(writer, history);
                }

                mSettingManager.Set(mSettingName, sb.ToString());
                mSettingManager.Save();
            }
        }
    }
}
