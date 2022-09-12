using System;
using System.Net;
using System.Net.Http;

namespace YoutubeInWebView.Utils.Web
{
    public class HttpException : Exception
    {
        public string requestUrl;
        public string requestBodyJson;
        public bool? success;
        public HttpStatusCode statusCode;
        public HttpResponseMessage response;
        public string responseContentJson;
        public byte[] responseContentBytes;

        public HttpException()
            : base()
        {
        }

        public HttpException(string message)
            : base(message)
        {
        }

        public HttpException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}