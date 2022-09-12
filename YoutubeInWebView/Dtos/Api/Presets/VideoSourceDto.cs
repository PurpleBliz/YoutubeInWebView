using System.Collections.Generic;
using Newtonsoft.Json;

namespace YoutubeInWebView.Dtos.Api.Presets
{
    public class VideoSourceDto
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("start_ts")]
        public float StartTs { get; set; }

        [JsonProperty("stop_ts")]
        public float StopTs { get; set; }

        [JsonProperty("thumbnails")]
        public ThumbnailsDto Thumbnails { get; set; }
    }
}
