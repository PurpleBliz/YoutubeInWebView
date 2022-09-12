using Newtonsoft.Json;

namespace YoutubeInWebView.Dtos.Api.Search
{
    public class SearchResultPageInfoDto
    {
        [JsonProperty("total_results")]
        public int TotalResults { get; set; }

        [JsonProperty("results_per_page")]
        public int ResultsPerPage { get; set; }
    }
}
