using System.Collections.Generic;
using Newtonsoft.Json;

namespace YoutubeInWebView.Dtos.Api.Presets
{
    public class SegmentDto
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("video_source")]
        public VideoSourceDto VideoSource { get; set; }

        [JsonProperty("filmlibrary_source")]
        public object FilmlibrarySource { get; set; }

        [JsonProperty("filmlibrary_link")]
        public object FilmlibraryLink { get; set; }

        [JsonProperty("references")]
        public List<ReferenceInSegmentDto> References { get; set; }
    }
}
