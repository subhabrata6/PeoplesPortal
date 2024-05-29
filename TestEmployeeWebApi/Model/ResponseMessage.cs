using System.Net;

namespace TestEmployeeWebApi.Model
{
    public class ResponseMessage
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
        public object Response { get; set; }
    }
}
