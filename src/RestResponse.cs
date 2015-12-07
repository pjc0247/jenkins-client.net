using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace jenkins_client
{
    internal class RestResponse
    {
        public HttpResponseHeaders headers { get; set; }
        public string body { get; set; }
        public HttpStatusCode code { get; set; }

        internal RestResponse(HttpResponseMessage response)
        {
            // block wait
            var task = response.Content.ReadAsStringAsync();
            task.Wait();

            this.body = task.Result;
            this.code = response.StatusCode;
            this.headers = response.Headers;
        }
    }
}
