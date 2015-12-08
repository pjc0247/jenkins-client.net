using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkinsClient
{
    public class BuildQueue
    {
        private Client client { get; set; }

        internal BuildQueue(Client client)
        {
            this.client = client;
        }

        public async Task<QueuedItem> GetItem(int id)
        {
            var response = await client.api.GetQueuedItem(id);

            return new QueuedItem(response.body);
        }
    }
}
