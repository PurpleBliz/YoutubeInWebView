using Newtonsoft.Json;

namespace YoutubeInWebView.Dtos.Api
{
    public class ThumbnailsDto
    {
        [JsonProperty("default")]
        public UrlDto Default { get; set; }

        [JsonProperty("animated")]
        public UrlDto Animated { get; set; }

        [JsonProperty("imdb")]
        public UrlDto Imdb { get; set; }

        [JsonProperty("kinopoisk")]
        public UrlDto Kinopoisk { get; set; }

        [JsonProperty("storyboard")]
        public UrlDto Storyboard { get; set; }
    }
}
