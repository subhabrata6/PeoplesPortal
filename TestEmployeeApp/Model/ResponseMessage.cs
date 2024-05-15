using System.Net;

namespace TestEmployeeApp.Model
{
    public class ResponseMessage
    {
        public HttpStatusCode Status { get; set; }
        public string Message { get; set; }
        public object Response { get; set; }
    }
}
