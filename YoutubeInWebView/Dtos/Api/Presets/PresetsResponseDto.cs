using System.Collections.Generic;
using Newtonsoft.Json;

namespace YoutubeInWebView.Dtos.Api.Presets
{
    public class PresetsResponseDto
    {
        [JsonProperty("items")]
        public List<PresetItemDto> Items { get; set; }
    }
}
