using Newtonsoft.Json;

namespace YoutubeInWebView.Dtos.Api
{
    public class UrlDto
    {
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}
