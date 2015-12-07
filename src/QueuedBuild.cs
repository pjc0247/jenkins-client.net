using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jenkins_client
{
    public class QueuedBuild : Build
    {
        public int itemId { get; private set; }

        internal QueuedBuild(int itemId, Job job) : 
            base(job, -1, null)
        {
            this.itemId = itemId;
        }

        public async Task WaitForBuildStart()
        {
            while (true)
            {
                var item = await client.GetBuildQueue().GetItem(itemId);

                if (item.buildNumber.HasValue)
                {
                    number = item.buildNumber.Value;

                    return;
                }

                await Task.Delay(PollingInterval);
            }
        }
    }
}
