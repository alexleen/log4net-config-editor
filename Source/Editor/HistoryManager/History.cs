// Copyright © 2018 Alex Leendertsen

using System;
using System.Xml.Serialization;

namespace Editor.HistoryManager
{
    [Serializable]
    public class History
    {
        [XmlElement(nameof(HistoricalValue))] //Gets rid of the "Values" child element
        public HistoricalValue[] Values { get; set; }
    }
}
