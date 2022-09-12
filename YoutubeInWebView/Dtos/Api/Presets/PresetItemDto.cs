using System.Collections.Generic;
using Newtonsoft.Json;

namespace YoutubeInWebView.Dtos.Api.Presets
{
    public class PresetItemDto
    {
        [JsonProperty("ext_id")]
        public string ExtId { get; set; }

        [JsonProperty("preset_id")]
        public string PresetId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonProperty("progress")]
        public object Progress { get; set; }

        [JsonProperty("group")]
        public string Group { get; set; }

        [JsonProperty("segments")]
        public List<SegmentDto> Segments { get; set; }
    }
}