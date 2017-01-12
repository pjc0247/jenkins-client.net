using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace JenkinsClient
{
    internal class RestProcessor
    {
        private class Path
        {
            public static readonly string GetJobs = "/api/json";
            public static readonly string GetJobData = "/job/{0}/api/json";
            public static readonly string GetBuildData = "/job/{0}/{1}/api/json";
            public static readonly string GetQueuedItem = "/queue/item/{0}/api/json";

            public static readonly string PostBuild = "/job/{0}/build?jenkins_status=1&jenkins_sleep=3";
            public static readonly string PostBuildWithParameters = "/job/{0}/buildWithParameters?jenkins_status=1&jenkins_sleep=3";
        }

        protected HttpClient http { get; set; }
        protected string host { get; set; }

        public RestProcessor(string host, string id, string password)
        {
            this.host = host;

            this.http = new HttpClient();
            this.http.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes(id + ":" + password)));
        }

        private Uri FormatUri(string src, params object[] bindings)
        {
            Contract.Requires(string.IsNullOrEmpty(src) == false);

            return new Uri(host + string.Format(src, bindings));
        }

        private async Task<RestResponse> Request(Uri uri)
        {
            var response = await http.GetAsync(uri);

            return await RestResponse.Create(response);
        }
        private async Task<RestResponse> Request(Uri uri, string data)
        {
            var response = await http.PostAsync(uri, new StringContent(data));

            return await RestResponse.Create(response);
        }
        private async Task<RestResponse> Request(Uri uri, Dictionary<string, string> data)
        {
			/*
            var content = new MultipartFormDataContent();

            foreach (var pair in data)
                content.Add(new StringContent(pair.Value), pair.Key);
			*/
			var uriString = uri.ToString();
			foreach (var pair in data)
				uriString += "&" + pair.Key + "=" + Uri.EscapeUriString(pair.Value);
			
			var response = await http.PostAsync(new Uri(uriString), new StringContent(""));

            return await RestResponse.Create(response);
        }

        public async Task<RestResponse> GetJobs()
        {
            return await Request(FormatUri(Path.GetJobs));
        }
        public async Task<RestResponse> GetJobData(string jobName)
        {
            return await Request(FormatUri(Path.GetJobData, jobName));
        }
        public async Task<RestResponse> GetBuildData(string jobName, int buildNo)
        {
            return await Request(FormatUri(Path.GetBuildData, jobName, buildNo));
        }
        public async Task<RestResponse> PostBuild(string jobName)
        {
            return await Request(FormatUri(Path.PostBuild, jobName));
        }
        public async Task<RestResponse> PostBuildWithParameters(string jobName, Dictionary<string,string> data)
        {
            return await Request(FormatUri(Path.PostBuildWithParameters, jobName), data);
        }

        public async Task<RestResponse> GetQueuedItem(int itemId)
        {
            return await Request(FormatUri(Path.GetQueuedItem, itemId));
        }
    }
}
