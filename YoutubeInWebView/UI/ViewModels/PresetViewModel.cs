using Xamarin.Forms;
using YoutubeInWebView.Utils.Web;

namespace YoutubeInWebView.UI.ViewModels
{
    public class PresetViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string PreviewImageUrl { get; set; }

        public ImageSource PreviewImage => PreviewImageUrl;

        public ImageSource PreviewImageFromUrl => ImageSourceUrlConverter.Convert(PreviewImageUrl);
    }
}
