// Copyright © 2018 Alex Leendertsen

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

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Value, other.Value);
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals(obj as HistoricalValue);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode - unfortunately, .NET serialization requires serializable properties to be public get and set
            return Value.GetHashCode();
        }
    }
}
