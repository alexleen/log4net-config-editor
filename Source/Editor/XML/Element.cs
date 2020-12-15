// Copyright © 2020 Alex Leendertsen

using System;
using System.Collections.Generic;
using System.Linq;
using Editor.Interfaces;

namespace Editor.XML
{
    public class Element : IElement, IEquatable<IElement>
    {
        public Element(string name, IEnumerable<(string attrName, string attrValue)> attributes)
        {
            Name = name;
            Attributes = attributes;
        }

        public string Name { get; }

        public IEnumerable<(string attrName, string attrValue)> Attributes { get; }

        public bool Equals(IElement other)
        {
            if (other == null)
            {
                return false;
            }

            return Name == other.Name && Attributes.SequenceEqual(other.Attributes);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            return Equals(obj as Element);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Name != null ? Name.GetHashCode() : 0) * 397) ^ (Attributes != null ? Attributes.GetHashCode() : 0);
            }
        }
    }
}
