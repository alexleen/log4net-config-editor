// Copyright © 2018 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Editor.Properties;

namespace Editor.Windows.Appenders.Properties.PatternManager
{
    public class HistoricalPatternManager : IHistoricalPatternManager
    {
        private const int Limit = 5;

        public IEnumerable<string> GetPatterns()
        {
            PatternsXml patternsXml = Read();

            return patternsXml != null ? patternsXml.Patterns.OrderByDescending(p => p.LastUsage).Select(p => p.Pattern) : new string[] { };
        }

        private static PatternsXml Read()
        {
            PatternsXml patternsXml = null;

            if (!string.IsNullOrEmpty(Settings.Default.HistoricalPatterns))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(PatternsXml));

                using (TextReader reader = new StringReader(Settings.Default.HistoricalPatterns))
                {
                    patternsXml = (PatternsXml)serializer.Deserialize(reader);
                }
            }

            return patternsXml;
        }

        public void SavePattern(string pattern)
        {
            StoredPattern newPattern = new StoredPattern { LastUsage = DateTime.UtcNow, Pattern = pattern };

            List<StoredPattern> patterns = new List<StoredPattern> { newPattern };

            PatternsXml patternsXml = Read();

            if (patternsXml == null)
            {
                patternsXml = new PatternsXml { Patterns = patterns.ToArray() };
            }
            else
            {
                patterns.AddRange(patternsXml.Patterns);
                patternsXml.Patterns = patterns.Distinct().OrderByDescending(p => p.LastUsage).Take(Limit).ToArray();
            }

            Save(patternsXml);
        }

        private static void Save(PatternsXml patternsXml)
        {
            StringBuilder sb = new StringBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(PatternsXml));

            using (TextWriter writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, patternsXml);
            }

            Settings.Default.HistoricalPatterns = sb.ToString();
            Settings.Default.Save();
        }
    }
}
