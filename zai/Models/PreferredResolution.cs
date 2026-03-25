using System;
using System;
using System.Text.Json.Serialization;

namespace zai.Models
{

    public readonly struct PreferredResolution : IEquatable<PreferredResolution>
    {
        [JsonPropertyName("resolution")]
        public Resolution Resolution { get; }

        [JsonPropertyName("weight")]
        public double Weight { get; }

        [JsonConstructor]
        public PreferredResolution(Resolution resolution, double weight = 1.0)
        {
            if (weight <= 0)
                throw new ArgumentOutOfRangeException(nameof(weight), "Weight must be positive.");

            Resolution = resolution;
            Weight = weight;
        }

        public override string ToString()
            => $"{Resolution} (w={Weight:0.###})";

        public bool Equals(PreferredResolution other)
            => Resolution.Equals(other.Resolution) &&
               Weight.Equals(other.Weight);

        public override bool Equals(object? obj)
            => obj is PreferredResolution other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(Resolution, Weight);

        public static bool operator ==(PreferredResolution left, PreferredResolution right)
            => left.Equals(right);

        public static bool operator !=(PreferredResolution left, PreferredResolution right)
            => !left.Equals(right);
    }

}

