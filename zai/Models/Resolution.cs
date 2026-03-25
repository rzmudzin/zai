using System;
using System;
using System.Text.Json.Serialization;

namespace zai.Models
{

    public readonly struct Resolution : IEquatable<Resolution>, IComparable<Resolution>
    {
        [JsonPropertyName("width")]
        public int Width { get; }

        [JsonPropertyName("height")]
        public int Height { get; }

        [JsonConstructor]
        public Resolution(int width, int height)
        {
            if (width <= 0)
                throw new ArgumentOutOfRangeException(nameof(width), "Width must be positive.");

            if (height <= 0)
                throw new ArgumentOutOfRangeException(nameof(height), "Height must be positive.");

            Width = width;
            Height = height;
        }

        public override string ToString()
            => $"{Width}x{Height}";

        public static bool TryParse(string? value, out Resolution resolution)
        {
            resolution = default;

            if (string.IsNullOrWhiteSpace(value))
                return false;

            var parts = value.Split('x', 'X');
            if (parts.Length != 2)
                return false;

            if (!int.TryParse(parts[0], out var w))
                return false;

            if (!int.TryParse(parts[1], out var h))
                return false;

            if (w <= 0 || h <= 0)
                return false;

            resolution = new Resolution(w, h);
            return true;
        }

        public bool Equals(Resolution other)
            => Width == other.Width && Height == other.Height;

        public override bool Equals(object? obj)
            => obj is Resolution other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(Width, Height);

        public static bool operator ==(Resolution left, Resolution right)
            => left.Equals(right);

        public static bool operator !=(Resolution left, Resolution right)
            => !left.Equals(right);

        public int CompareTo(Resolution other)
        {
            // Deterministic ordering: first by total pixels, then width, then height.
            var pixels = Width * Height;
            var otherPixels = other.Width * other.Height;

            var cmp = pixels.CompareTo(otherPixels);
            if (cmp != 0) return cmp;

            cmp = Width.CompareTo(other.Width);
            if (cmp != 0) return cmp;

            return Height.CompareTo(other.Height);
        }
    }

}

