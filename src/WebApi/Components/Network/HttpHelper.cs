using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebApi.Components.Network
{
    public static class HttpHelper
    {
        public static async Task<string> GetAsync(string uri)
        {
            var client = new HttpClient();
            var response = await client.GetAsync(uri);
            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadAsStringAsync();
            else
                return string.Empty;
        }

        public static async Task<string> PostAsync(string uri, HttpContent content)
        {
            var client = new HttpClient();
            var response = await client.PostAsync(uri, content);
            if (response.StatusCode == HttpStatusCode.OK)
                return await response.Content.ReadAsStringAsync();
            else
                return string.Empty;
        }
    }
}
