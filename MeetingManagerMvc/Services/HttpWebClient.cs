using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MeetingManagerMvc.Services
{
    public class HttpWebClient
    {
        private readonly HttpClient client;
        private readonly string WebApiPath;
        private readonly IConfiguration _configuration;
        public HttpWebClient(IConfiguration configuration)
        {
            _configuration = configuration;
            WebApiPath = _configuration["MeetingManager:Url"];
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("ApiKey", _configuration["MeetingManager:ApiKey"]);
        }

        public string GetWebApiPath()
        {
            return WebApiPath;
        }
        public async Task<T> GetAsyncRequest<T>(string url)
        {
            HttpResponseMessage response = await client.GetAsync(WebApiPath + url);
            if (response.IsSuccessStatusCode)
            {
                T data = await response.Content.ReadAsAsync<T>();

                return data;
            }
            return default(T);
        }
        public HttpClient GetClient()
        {
            return client;
        }

        public static implicit operator HttpClient(HttpWebClient v)
        {
            throw new NotImplementedException();
        }
    }
}
