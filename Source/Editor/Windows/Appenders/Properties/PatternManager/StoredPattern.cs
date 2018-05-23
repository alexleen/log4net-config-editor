// Copyright © 2018 Alex Leendertsen

using System;
using System.Xml.Serialization;

namespace Editor.Windows.Appenders.Properties.PatternManager
{
    [Serializable]
    public class StoredPattern : IEquatable<StoredPattern>
    {
        [XmlAttribute]
        public DateTime LastUsage { get; set; }

        public string Pattern { get; set; }

        public bool Equals(StoredPattern other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return string.Equals(Pattern, other.Pattern);
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

            return Equals(obj as StoredPattern);
        }

        public override int GetHashCode()
        {
            // ReSharper disable once NonReadonlyMemberInGetHashCode - unfortunately, .NET serialization requires serializable properties to be public get and set
            return Pattern.GetHashCode();
        }
    }
}
