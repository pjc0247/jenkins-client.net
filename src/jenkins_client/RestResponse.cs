using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace JenkinsClient
{
    internal class RestResponse
    {
        public HttpResponseMessage response { get; set; }
        public HttpResponseHeaders headers { get; set; }
        public string body { get; set; }
        public HttpStatusCode code { get; set; }

        public static async Task<RestResponse> Create(HttpResponseMessage response)
        {
            var restResponse = new RestResponse(response);
            await restResponse.FetchContentAsync();

            return restResponse;
        }

        private RestResponse(HttpResponseMessage response)
        {
            this.response = response;
            this.code = response.StatusCode;
            this.headers = response.Headers;
        }
        private async Task FetchContentAsync()
        {
            this.body = await response.Content.ReadAsStringAsync();
        }
    }
}
