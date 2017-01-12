using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;

namespace JenkinsClient
{
    public class Client
    {
        /// <summary>
        /// Auth with ID & Password
        /// </summary>
        /// <param name="host"></param>
        /// <param name="id"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static Client Create(
            string host,
            string id, string password)
        {
            Contract.Requires(string.IsNullOrEmpty(host) == false);
            Contract.Requires(string.IsNullOrEmpty(id) == false);
            Contract.Requires(string.IsNullOrEmpty(password) == false);

            var client = new Client(host, id, password);

            return client;
        }

        public string host { get; protected set; }
        public string id { get; protected set; }
        protected string password { get; set; }

        internal RestProcessor api { get; private set; }

        private BuildQueue buildQueue { get; set; }

        protected Client(
            string host,
            string id, string password)
        {
            host = host.TrimEnd(new char[] { '/', ' ', '\t' });

            this.host = host;
            this.id = id;
            this.password = password;

            this.api = new RestProcessor(host, id, password);

            this.buildQueue = new BuildQueue(this);
        }

        public async Task<List<Job>> GetJobsAsync()
        {
            var response = await api.GetJobs();
			if (response.code != System.Net.HttpStatusCode.OK)
				throw new InvalidOperationException(response.code.ToString());

            var data = JObject.Parse(response.body);
            
            var jobs = from job in data["jobs"]
                       select new Job(this, (string)job["name"]);

            return jobs.ToList();
        }

        /// <summary>
        /// 지정된 이름의 Job을 가져온다.
        /// 이 작업은 네트워크를 거치지 않으며, 지정된 이름의 Job이
        /// 서버에 실제 존재하는지 검사하지 않는다.
        /// </summary>
        /// <param name="name">Job의 이름</param>
        /// <returns></returns>
        public Job GetJob(string name)
        {
            Contract.Requires(string.IsNullOrEmpty(name) == false);

            return new Job(this, name);
        }

        public BuildQueue GetBuildQueue()
        {
            return buildQueue;
        }
    }
}
