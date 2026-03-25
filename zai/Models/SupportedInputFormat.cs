using System;
using System;
using System.Text.Json.Serialization;
namespace zai.Models
{

    public readonly struct SupportedInputFormat : IEquatable<SupportedInputFormat>
    {
        [JsonPropertyName("mimeType")]
        public string MimeType { get; }

        [JsonPropertyName("maxResolution")]
        public Resolution? MaxResolution { get; }

        [JsonPropertyName("maxBytes")]
        public long? MaxBytes { get; }

        [JsonConstructor]
        public SupportedInputFormat(string mimeType, Resolution? maxResolution = null, long? maxBytes = null)
        {
            if (string.IsNullOrWhiteSpace(mimeType))
                throw new ArgumentException("MIME type cannot be null or empty.", nameof(mimeType));

            if (maxBytes is < 0)
                throw new ArgumentOutOfRangeException(nameof(maxBytes), "MaxBytes must be non-negative.");

            MimeType = mimeType;
            MaxResolution = maxResolution;
            MaxBytes = maxBytes;
        }

        public override string ToString()
        {
            var res = MaxResolution.HasValue ? $" maxRes={MaxResolution}" : "";
            var bytes = MaxBytes.HasValue ? $" maxBytes={MaxBytes}" : "";
            return $"{MimeType}{res}{bytes}";
        }

        public bool Equals(SupportedInputFormat other)
            => MimeType == other.MimeType &&
               Nullable.Equals(MaxResolution, other.MaxResolution) &&
               MaxBytes == other.MaxBytes;

        public override bool Equals(object? obj)
            => obj is SupportedInputFormat other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(MimeType, MaxResolution, MaxBytes);

        public static bool operator ==(SupportedInputFormat left, SupportedInputFormat right)
            => left.Equals(right);

        public static bool operator !=(SupportedInputFormat left, SupportedInputFormat right)
            => !left.Equals(right);
    }

}

