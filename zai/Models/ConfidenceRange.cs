using System;
using System;
using System.Text.Json.Serialization;

namespace zai.Models
{

    public readonly struct ConfidenceRange : IEquatable<ConfidenceRange>
    {
        [JsonPropertyName("min")]
        public double Min { get; }

        [JsonPropertyName("max")]
        public double Max { get; }

        [JsonConstructor]
        public ConfidenceRange(double min, double max)
        {
            if (min < 0 || min > 1)
                throw new ArgumentOutOfRangeException(nameof(min), "Min must be between 0 and 1.");

            if (max < 0 || max > 1)
                throw new ArgumentOutOfRangeException(nameof(max), "Max must be between 0 and 1.");

            if (min > max)
                throw new ArgumentException("Min cannot be greater than Max.");

            Min = min;
            Max = max;
        }

        public bool Contains(double value)
            => value >= Min && value <= Max;

        public override string ToString()
            => $"{Min:0.###}–{Max:0.###}";

        public bool Equals(ConfidenceRange other)
            => Min.Equals(other.Min) && Max.Equals(other.Max);

        public override bool Equals(object? obj)
            => obj is ConfidenceRange other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(Min, Max);

        public static bool operator ==(ConfidenceRange left, ConfidenceRange right)
            => left.Equals(right);

        public static bool operator !=(ConfidenceRange left, ConfidenceRange right)
            => !left.Equals(right);
    }

}

