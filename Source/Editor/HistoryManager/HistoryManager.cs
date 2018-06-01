// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Editor.Properties;

namespace Editor.HistoryManager
{
    public class HistoryManager : IHistoryManager
    {
        private readonly string mSettingName;
        private readonly ISettingManager<string> mSettingManager;
        private const int Limit = 5;

        public HistoryManager(string settingName, ISettingManager<string> settingManager)
        {
            mSettingName = settingName;
            mSettingManager = settingManager;
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
                history.Values = values.Distinct().OrderByDescending(p => p.LastUsage).Take(Limit).ToArray();
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
