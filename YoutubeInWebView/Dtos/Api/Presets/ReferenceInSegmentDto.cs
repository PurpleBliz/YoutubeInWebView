using Newtonsoft.Json;

namespace YoutubeInWebView.Dtos.Api.Presets
{
    public class ReferenceInSegmentDto
    {
        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("provider")]
        public string Provider { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("start_ts")]
        public float StartTs { get; set; }

        [JsonProperty("stop_ts")]
        public float StopTs { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
