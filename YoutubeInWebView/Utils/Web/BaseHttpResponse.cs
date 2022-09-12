using System.Net.Http;

namespace YoutubeInWebView.Utils.Web
{
    public class BaseHttpResponse
    {
        public string JsonString { get; set; }

        public byte[] Bytes { get; set; }

        public HttpResponseMessage Response { get; set; }

        public BaseHttpResponse()
        {

        }

        public BaseHttpResponse(string json, HttpResponseMessage response)
        {
            JsonString = json;
            Response = response;
        }

        public BaseHttpResponse(byte[] bytes, HttpResponseMessage response)
        {
            Bytes = bytes;
            Response = response;
        }
    }
}
