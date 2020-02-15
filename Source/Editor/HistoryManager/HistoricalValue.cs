// Copyright © 2020 Alex Leendertsen

using System;
using System.Xml.Serialization;

namespace Editor.HistoryManager
{
    [Serializable]
    public class HistoricalValue : IEquatable<HistoricalValue>
    {
        [XmlAttribute]
        public DateTime LastUsage { get; set; }

        [XmlText]
        public string Value { get; set; }

        public bool Equals(HistoricalValue other)
        {
            if (other is null)
            {
                return false;
            }

            return ReferenceEquals(this, other) || string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            return ReferenceEquals(this, obj) || Equals(obj as HistoricalValue);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode - unfortunately, .NET serialization requires serializable properties to be public get and set
            return Value.GetHashCode();
        }
    }
}
