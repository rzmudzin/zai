using System;
using System;
using System.Text.Json.Serialization;

namespace zai
{

    public readonly struct ErrorInfo : IEquatable<ErrorInfo>
    {
        /// <summary>
        /// A stable, machine-friendly error code.
        /// Example: "invalid-input", "stream-failed", "timeout".
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; }

        /// <summary>
        /// Human-readable explanation of the error.
        /// Safe for logs and UI.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; }

        /// <summary>
        /// Optional detailed information for debugging.
        /// Not required for normal operation.
        /// </summary>
        [JsonPropertyName("details")]
        public string? Details { get; }

        [JsonConstructor]
        public ErrorInfo(string code, string message, string? details = null)
        {
            if (string.IsNullOrWhiteSpace(code))
                throw new ArgumentException("Error code cannot be null or empty.", nameof(code));

            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Error message cannot be null or empty.", nameof(message));

            Code = code;
            Message = message;
            Details = details;
        }

        public override string ToString()
            => Details == null
                ? $"{Code}: {Message}"
                : $"{Code}: {Message} ({Details})";

        public bool Equals(ErrorInfo other)
            => Code == other.Code &&
               Message == other.Message &&
               Details == other.Details;

        public override bool Equals(object? obj)
            => obj is ErrorInfo other && Equals(other);

        public override int GetHashCode()
            => HashCode.Combine(Code, Message, Details);

        public static bool operator ==(ErrorInfo left, ErrorInfo right)
            => left.Equals(right);

        public static bool operator !=(ErrorInfo left, ErrorInfo right)
            => !left.Equals(right);
    }

}

