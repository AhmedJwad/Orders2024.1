
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
            var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PostAsync(url, messageContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp); ;
        }

        public async Task<HttpResponseWrapper<TActionResponse>> PostAsync<T, TActionResponse>(string url, T model)
        {
            var messageJson = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
            var responseHttp=await _httpClient.PostAsync(url,messageContent);
            if(responseHttp.IsSuccessStatusCode)
            {
                var response=await UnserializeAnswer<TActionResponse>(responseHttp);
                return new HttpResponseWrapper<TActionResponse>(response, false, responseHttp);
            }
            return new HttpResponseWrapper<TActionResponse>(default, true, responseHttp);
        }
        public async Task<HttpResponseWrapper<object>> DeleteAsync<T>(string url)
        {
            var responseHttp = await _httpClient.DeleteAsync(url);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp); 
        }
        private async Task<T> UnserializeAnswer<T>(HttpResponseMessage messageHttp)
        {
            var response=await messageHttp.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<T>(response, _jsondefaultoption)!;
        }

        public async Task<HttpResponseWrapper<object>> PutAsync<T>(string url, T model)
        {
            
            var messageJson = JsonSerializer.Serialize(model);
            var messageContent = new StringContent(messageJson, Encoding.UTF8, "application/json");
            var responseHttp = await _httpClient.PutAsync(url, messageContent);
            return new HttpResponseWrapper<object>(null, !responseHttp.IsSuccessStatusCode, responseHttp); ;
        }

        public async Task<HttpResponseWrapper<TActionResponse>> PutAsync<T, TActionResponse>(string url, T model)
        {
            var messageJson=JsonSerializer.Serialize(model);
            var messageContent=new StringContent(messageJson, Encoding.UTF8, "application/json");   
            var responseHttp=await _httpClient.PutAsync(url,messageContent);
            if (responseHttp.IsSuccessStatusCode)
            {
                var response=await UnserializeAnswer<TActionResponse>(responseHttp);
                return new HttpResponseWrapper<TActionResponse>(response, false, responseHttp);
            }
            return new HttpResponseWrapper<TActionResponse>(default, true, responseHttp);
        }
    }
}
