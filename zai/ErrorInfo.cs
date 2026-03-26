using System;
using System;
using System.Text.Json.Serialization;

namespace zai
{

    public readonly struct ErrorInfo
    {
        public string Code { get; }
        public string Message { get; }
        public string? Details { get; }

        public ErrorInfo(string code, string message, string? details = null)
        {
            Code = code;
            Message = message;
            Details = details;
        }
    }

}

