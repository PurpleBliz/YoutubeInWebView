using System.Collections.Generic;
using Newtonsoft.Json;

namespace YoutubeInWebView.Dtos.Api.Search
{
    public class SearchResultItemDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

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

        [JsonProperty("thumbnails")]
        public ThumbnailsDto Thumbnails { get; set; }

        [JsonProperty("references")]
        public List<ReferenceInSearchDto> References { get; set; }
    }
}
