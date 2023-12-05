using Newtonsoft.Json;
using System.Net.Mime;
using System.Text;

namespace GT.Trace.Common.Infra.HttpApi
{
    //NOTE: ESTUDIAR ESTA PARTE, QUE SE VE COMPLEJA.
    public class HttpApiClient
    {
        private static readonly HttpClient _client = new();

        private readonly string _baseUrl;

        public HttpApiClient(string baseUrl)
        {
            _baseUrl = baseUrl;
        }

        public async Task<HttpApiJsonResponse?> PutAsync<T>(string endPoint, T data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var httpResponseMessage = await _client.PutAsync($"{_baseUrl}{endPoint}", content);

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HttpApiJsonResponse>(responseContent);
        }

        public async Task<HttpApiJsonResponse?> PutAsync(string endPoint)
        {
            var httpResponseMessage = await _client.PutAsync($"{_baseUrl}{endPoint}", null);

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HttpApiJsonResponse>(responseContent);
        }

        public async Task<HttpApiJsonResponse?> PostAsync<T>(string endPoint, T data)
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var httpResponseMessage = await _client.PostAsync($"{_baseUrl}{endPoint}", content);

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return new HttpApiJsonResponse(false, httpResponseMessage.ReasonPhrase, DateTime.UtcNow);
            }
            try
            {
                return JsonConvert.DeserializeObject<HttpApiJsonResponse>(responseContent);
            }
            catch
            {
                return new HttpApiJsonResponse(false, responseContent, DateTime.UtcNow);
            }
        }

        public async Task<HttpApiJsonResponse?> PostAsync(string endPoint)
        {
            var httpResponseMessage = await _client.PostAsync($"{_baseUrl}{endPoint}", null);

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HttpApiJsonResponse>(responseContent);
        }

        public async Task<T?> GetAsync<T>(string endPoint)
        {
            var stream = await _client.GetStringAsync($"{_baseUrl}{endPoint}").ConfigureAwait(false);
            return JsonConvert.DeserializeObject<T>(stream);
        }

        //public async Task<ApiJsonResponse?> DeleteAsync<T>(string endPoint, T data)
        //{
        //	var httpResponseMessage = await _client.DeleteAsync($"{_baseUrl}{endPoint}", );
        //	if (!httpResponseMessage.IsSuccessStatusCode) httpResponseMessage.Dump();
        //	var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
        //	return JsonConvert.DeserializeObject<ApiJsonResponse>(responseContent);
        //}

        public async Task<HttpApiJsonResponse?> DeleteAsync(string endPoint, Dictionary<string, object>? args = null)
        {
            var queryString = args == null ? "" : $"?{args.Keys.Select(k => $"{k}={args[k]}").Aggregate((x, y) => $"{x}&{y}")}";
            var httpResponseMessage = await _client.DeleteAsync($"{_baseUrl}{endPoint}{queryString}");

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<HttpApiJsonResponse>(responseContent);
        }

        public async Task<HttpApiJsonResponse<TResponse>?> PostAsync<TRequest, TResponse>(string endPoint, TRequest data)
        where TResponse : class
        {
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var httpResponseMessage = await _client.PostAsync($"{_baseUrl}{endPoint}", content);

            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return new HttpApiJsonResponse<TResponse>(false, httpResponseMessage.ReasonPhrase, default, DateTime.UtcNow);
            }

            try
            {
                return JsonConvert.DeserializeObject<HttpApiJsonResponse<TResponse>>(responseContent);
            }
            catch
            {
                return new HttpApiJsonResponse<TResponse>(false, responseContent, default, DateTime.UtcNow);
            }
        }

        public async Task<HttpApiJsonResponse<T>> GetJsonAsync<T>(string endPoint, object? requestBody = null)
        {
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(_baseUrl + endPoint),
                Content = requestBody == null
                    ? null
                    : new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, MediaTypeNames.Application.Json),
            };

            using var httpClient = new HttpClient();
            var httpResponseMessage = await httpClient.SendAsync(request).ConfigureAwait(false);
            return await ParseResponseAsync<T>(httpResponseMessage).ConfigureAwait(false);
        }

        public async Task<HttpApiJsonResponse<T>> PostJsonAsync<T>(string endPoint, object data)
        {
            using var httpClient = new HttpClient();
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var httpResponseMessage = await httpClient.PostAsync($"{_baseUrl}{endPoint}", content);
            return await ParseResponseAsync<T>(httpResponseMessage).ConfigureAwait(false);
        }

        private static async Task<HttpApiJsonResponse<T>> ParseResponseAsync<T>(HttpResponseMessage httpResponseMessage)
        {
            //if (httpResponseMessage.IsSuccessStatusCode)
            //{
            var responseContent = await httpResponseMessage.Content.ReadAsStringAsync();
            if (string.IsNullOrWhiteSpace(responseContent))
            {
                return new HttpApiJsonResponse<T>(false, httpResponseMessage.ReasonPhrase, default, DateTime.UtcNow);
            }
            else
            {
                return JsonConvert.DeserializeObject<HttpApiJsonResponse<T>>(responseContent)
                    ?? throw new Exception("Ocurrió un error al intentar deserealizar la respuesta de la API.");
            }
            //}
            //else
            //{
            //    Console.WriteLine(httpResponseMessage);
            //    return new HttpApiJsonResponse<T>(httpResponseMessage.IsSuccessStatusCode, httpResponseMessage.ReasonPhrase, default, DateTime.UtcNow);
            //}
        }
    }
}