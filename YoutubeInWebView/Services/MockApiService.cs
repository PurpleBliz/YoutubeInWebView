using System.Threading.Tasks;
using Newtonsoft.Json;
using YoutubeInWebView.Dtos.Api.Presets;
using YoutubeInWebView.Dtos.Api.Search;

namespace YoutubeInWebView.Services
{
    internal class MockApiService : IApiService
    {
        public Task<SearchResponseDto> SearchAsync(string q, string owner, int pageSize, string pageToken)
        {
            var response = ExtractFileContents<SearchResponseDto>("search-response.json");
            return Task.FromResult(response);
        }

        public Task<PresetsResponseDto> PresetsAsync(params string[] id)
        {
            var response = ExtractFileContents<PresetsResponseDto>("presets.json");
            return Task.FromResult(response);
        }

        private T ExtractFileContents<T>(string filename)
        {
            var fileHelper = new FileHelper();
            var path = fileHelper.GetPathToResource(filename);
            var json = fileHelper.ReadResourceFile(path);
            var deserializedObject = JsonConvert.DeserializeObject<T>(json);
            return deserializedObject;
        }
    }
}
