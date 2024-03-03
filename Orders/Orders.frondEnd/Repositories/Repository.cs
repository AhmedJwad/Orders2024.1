
using System.Text;
using System.Text.Json;

namespace Orders.frondEnd.Repositories
{
    public class Repository : IRepository
    {
        private readonly HttpClient _httpClient;

        private JsonSerializerOptions _jsondefaultoption = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
        };
        public Repository(HttpClient httpClient)
        {
           _httpClient = httpClient;
        }
        public async Task<HttpResponseWrapper<T>> GetASync<T>(string url)
        {
           var messageHttp=await _httpClient.GetAsync(url);
            if(messageHttp.IsSuccessStatusCode)
            {
                var response = await UnserializeAnswer<T>(messageHttp);
                return new HttpResponseWrapper<T>(response, false, messageHttp);
            }
            return new HttpResponseWrapper<T>(default, true, messageHttp);
        }



        public async Task<HttpResponseWrapper<object>> PostAsync<T>(string url, T model)
        {
            var messageJson = JsonSerializer.Serialize(model);
            var messagecontent = new StringContent(messageJson, Encoding.UTF8, "application/json");
            var responsehttp = await _httpClient.PostAsync(url, messagecontent);
            return new HttpResponseWrapper<object>(null, responsehttp!.IsSuccessStatusCode, responsehttp!)
        }

        public async Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model)
        {
            var messagejson = JsonSerializer.Serialize(model);
            var messagecontent = new StringContent(messagejson, Encoding.UTF8, "application/json");
            var responsehttp=await _httpClient.PostAsync(url,messagecontent);
            if(responsehttp.IsSuccessStatusCode)
            {
                var response=await UnserializeAnswer<TActionResponse>(responsehttp);
                return new HttpResponseWrapper<TActionResponse>(response, false, responsehttp);
            }
            return new HttpResponseWrapper<TActionResponse>(default, true, responsehttp);
        }
        private async Task<T> UnserializeAnswer<T>(HttpResponseMessage messageHttp)
        {
            var response=await messageHttp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(response, _jsondefaultoption)!;
        }
    }
}
