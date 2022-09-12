using System;
using System.Globalization;
using System.IO;
using System.Net;
using Xamarin.Forms;

namespace YoutubeInWebView.Utils.Web
{
    public static class ImageSourceUrlConverter
    {
        public static ImageSource Convert(string url)
        {
            if (url == null)
                return null;

            using (var webClient = new WebClient())
            {
                try
                {
                    var byteArray = webClient.DownloadData(url);
                    var imageSource = ImageSource.FromStream(() => new MemoryStream(byteArray));
                    return imageSource;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    return null;
                }
            }
        }
    }
}
