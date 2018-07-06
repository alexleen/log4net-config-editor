// Copyright © 2018 Alex Leendertsen

using System;

namespace Editor.Windows.SizeLocation
{
    public class Size : IEquatable<Size>
    {
        public Size(double value = double.NaN, double max = double.PositiveInfinity, double min = 0.0)
        {
            Value = value;
            Max = max;
            Min = min;
        }

        public double Value { get; }

        public double Max { get; }

        public double Min { get; }

        public override string ToString()
        {
            return $"{Min} <- {Value} -> {Max}";
        }

        public bool Equals(Size other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Equals(Value, other.Value) && Equals(Max, other.Max) && Equals(Min, other.Min);
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

            return obj is Size size && Equals(size);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hashCode = Value.GetHashCode();
                hashCode = (hashCode * 397) ^ Max.GetHashCode();
                hashCode = (hashCode * 397) ^ Min.GetHashCode();
                return hashCode;
            }
        }
    }
}
