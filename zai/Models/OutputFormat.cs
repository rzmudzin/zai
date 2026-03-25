using System;
using System;
using System.Text.Json.Serialization;

namespace zai.Models
{

    public readonly struct OutputFormat : IEquatable<OutputFormat>
    {
        [JsonPropertyName("mimeType")]
        public string MimeType { get; }

        [JsonPropertyName("fileExtension")]
        public string? FileExtension { get; }

        [JsonPropertyName("schema")]
        public string? Schema { get; }

        [JsonConstructor]
        public OutputFormat(string mimeType, string? fileExtension = null, string? schema = null)
        {
            if (string.IsNullOrWhiteSpace(mimeType))
                throw new ArgumentException("MIME type cannot be null or empty.", nameof(mimeType));

            if (fileExtension is { Length: 0 })
                throw new ArgumentException("File extension cannot be empty.", nameof(fileExtension));

            MimeType = mimeType;
            FileExtension = fileExtension;
            Schema = schema;
        }

        public override string ToString()
        {
            var ext = FileExtension != null ? $" .{FileExtension}" : "";
            var schema = Schema != null ? $" schema={Schema}" : "";
            return $"{MimeType}{ext}{schema}";
        }

        public bool Equals(OutputFormat other)
            => MimeType == other.MimeType &&
               FileExtension == other.FileExtension &&
               Schema == other.Schema;

        public override bool Equals(object? obj)
            => obj is OutputFormat other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(MimeType, FileExtension, Schema);

        public static bool operator ==(OutputFormat left, OutputFormat right)
            => left.Equals(right);

        public static bool operator !=(OutputFormat left, OutputFormat right)
            => !left.Equals(right);
    }

}

