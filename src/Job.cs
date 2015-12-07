using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace jenkins_client
{
    public class Job : LazyObject<JObject>
    {
        internal Client client { get; private set; }

        public string name { get; private set; }
        public string description
        {
            get
            {
                EnsureDataInLocal();

                return (string)data[nameof(description)];
            }
        }
        public string displayName
        {
            get
            {
                EnsureDataInLocal();

                return (string)data[nameof(displayName)];
            }
        }
        public int nextBuildNumber
        {
            get
            {
                EnsureDataInLocal();

                return (int)data[nameof(nextBuildNumber)];
            }
        }
        public bool buildable
        {
            get
            {
                EnsureDataInLocal();

                return (bool)data[nameof(buildable)];
            }
        }
        public string url
        {
            get
            {
                EnsureDataInLocal();

                return (string)data[nameof(url)];
            }
        }

        #region LAST_BUILDS
        public Build lastBuild
        {
            get
            {
                EnsureDataInLocal();

                return GetBuild(nameof(lastBuild));
            }
        }
        public Build lastCompleteBuild
        {
            get
            {
                EnsureDataInLocal();

                return GetBuild(nameof(lastCompleteBuild));
            }
        }
        public Build lastFailedBuild
        {
            get
            {
                EnsureDataInLocal();

                return GetBuild(nameof(lastFailedBuild));
            }
        }
        public Build lastSuccessfulBuild
        {
            get
            {
                EnsureDataInLocal();

                return GetBuild(nameof(lastSuccessfulBuild));
            }
        }
        public Build lastStableBuild
        {
            get
            {
                EnsureDataInLocal();

                return GetBuild(nameof(lastStableBuild));
            }
        }
        public Build lastUnstableBuild
        {
            get
            {
                EnsureDataInLocal();

                return GetBuild(nameof(lastUnstableBuild));
            }
        }
        #endregion

        internal Job(Client client, string name)
        {
            this.client = client;
            this.name = name;
        }

        public override async Task<JObject> Fetch()
        {
            var response = await client.api.GetJobData(name);

            return JObject.Parse(response.body);
        }

        public async Task<QueuedBuild> BuildAsync(Dictionary<string, string> param = null)
        {
            var response = await client.api.PostBuildWithParameters(
                name, 
                param ?? new Dictionary<string, string>());

            if (response.code == System.Net.HttpStatusCode.Created)
            {
                var md = Regex.Match(
                    response.headers.Location.PathAndQuery,
                    $"item/([0-9]+)");
                var itemId = int.Parse(md.Groups[1].Value);

                return new QueuedBuild(itemId, this);
            }
            else
                return null;
        }

        private Build GetBuild(string tag)
        {
            var build = data[tag];
            if (!build.HasValues)
                return null;

            return new Build(this,
                (int)build["number"],
                (string)build["url"]);
        }
    }
}
