// Copyright © 2018 Alex Leendertsen


namespace Editor.Interfaces
{
    internal class Size
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
    }
}
