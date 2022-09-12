using Newtonsoft.Json;

namespace YoutubeInWebView.Dtos.Api.Search
{
    public class ReferenceInSearchDto
    {
        [JsonProperty("category")]
        public string Category { get; set; }

        [JsonProperty("count")]
        public int? Count { get; set; }

    }
}
