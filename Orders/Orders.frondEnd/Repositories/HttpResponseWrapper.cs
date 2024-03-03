using System.Reflection.Metadata;

namespace Orders.frondEnd.Repositories
{
    public class HttpResponseWrapper<T>
    {
        public HttpResponseWrapper(T? response, bool error, HttpResponseMessage httpResponseMessage)
        {
            Response = response;
            Error = error;
            HttpResponseMessage = httpResponseMessage;
        }

        public T? Response { get; }
        public bool Error { get; }
        public HttpResponseMessage HttpResponseMessage { get; } 
        public async Task<string?> GetErrorMessageAsync()
        {
            if(!Error)
            {
                return null;
            }
            var statuscode = HttpResponseMessage.StatusCode;

            if(statuscode==System.Net.HttpStatusCode.NotFound)
            {
                return "Resource not found.";
            }
            if (statuscode == System.Net.HttpStatusCode.BadRequest)
            {
                return await HttpResponseMessage.Content.ReadAsStringAsync();
            }
            if (statuscode == System.Net.HttpStatusCode.Unauthorized)
            {
                return "You have to be logged in to execute this operation.";
            }
            if (statuscode == System.Net.HttpStatusCode.Forbidden)
            {
                return "You do not have permissions to do this operation.";
            }
            return "An unexpected error has occurred.";
        }
    }
}
