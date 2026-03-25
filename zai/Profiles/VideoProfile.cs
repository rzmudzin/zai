using System;
namespace zai.Profiles
{
    public sealed class VideoProfile : StreamProfileBase
    {
        public string? Resolution { get; init; }
        public int? Fps { get; init; }
    }
}

