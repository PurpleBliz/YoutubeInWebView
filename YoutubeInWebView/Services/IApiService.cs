using System.Threading.Tasks;
using YoutubeInWebView.Dtos.Api.Presets;
using YoutubeInWebView.Dtos.Api.Search;

namespace YoutubeInWebView.Services
{
    public interface IApiService
    {
        Task<SearchResponseDto> SearchAsync(string q, string owner, int pageSize, string pageToken);

        Task<PresetsResponseDto> PresetsAsync(params string[] id);
    }
}
