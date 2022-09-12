using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YoutubeInWebView.Dtos.Api.Presets;
using YoutubeInWebView.Dtos.Api.Search;
using YoutubeInWebView.Utils.Web;

namespace YoutubeInWebView.Services
{
    public class ApiService : IApiService
    {
        private const int DefaultPageSize = 10;

        private const string ApiPath = "https://api2.comexp.net/demo/v1/";
        private readonly Uri baseUri = new Uri(ApiPath);

        public async Task<SearchResponseDto> SearchAsync(
            string q, string owner = "http://yt.comexp.net", int pageSize = DefaultPageSize, string pageToken = null)
        {
            // https://api2.comexp.net/demo/v1/search
            var relativeUri = $"search?q={q}&owner={owner}&max_results={pageSize}";
            if (!string.IsNullOrEmpty(pageToken))
                relativeUri += $"&page_token={pageToken}";
            var uri = new Uri(baseUri, relativeUri);
            var response = await GetAsync(uri.ToString());
            var json = response.JsonString;
            var result = JsonConvert.DeserializeObject<SearchResponseDto>(json);
            return result;
        }

        public async Task<PresetsResponseDto> PresetsAsync(params string[] idParams)
        {
            // https://api2.comexp.net/demo/v1/presets?id=4XDhlJhJx60
            var ids = string.Join(",", idParams);
            var relativeUri = $"presets?id={ids}";
            var uri = new Uri(baseUri, relativeUri);
            var response = await GetAsync(uri.ToString());
            var json = response.JsonString;
            var result = JsonConvert.DeserializeObject<PresetsResponseDto>(json);
            return result;
        }

        private async Task<BaseHttpResponse> GetAsync(string url)
        {
            HttpResponseMessage response;

            using (var handler = new HttpClientHandler { UseCookies = false })
            using (var client = new HttpClient(handler))
            {
                try
                {
                    // Logger.Info("GET ", url);
                    client.Timeout = TimeSpan.FromSeconds(100);
                    response = await client.GetAsync(url);
                }
                catch (Exception ex)
                {
                    throw new HttpException($"Got error on client.GetAsync({url})", ex)
                    {
                        requestUrl = url,
                        success = false,
                    };
                }
            }

            string json;
            byte[] bytes = null;
            try
            {
                json = await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new HttpException("Got error on response.Content.ReadAsStringAsync()", ex)
                {
                    requestUrl = url,
                    success = false,
                    statusCode = response.StatusCode,
                    response = response,
                };
            }

            if (response.StatusCode != HttpStatusCode.OK)
            {
                throw new HttpException("response.StatusCode != HttpStatusCode.OK")
                {
                    requestUrl = url,
                    success = false,
                    statusCode = response.StatusCode,
                    response = response,
                    responseContentJson = json,
                    responseContentBytes = bytes,
                };
            }

            return new BaseHttpResponse(json, response);
        }
    }
}
