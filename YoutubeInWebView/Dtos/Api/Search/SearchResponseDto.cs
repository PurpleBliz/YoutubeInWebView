using System.Collections.Generic;
using Newtonsoft.Json;

namespace YoutubeInWebView.Dtos.Api.Search
{
    public class SearchResponseDto
    {
        [JsonProperty("items")]
        public List<SearchResultItemDto> Items { get; set; }

        [JsonProperty("page_info")]
        public SearchResultPageInfoDto PageInfo { get; set; }
    }
}
