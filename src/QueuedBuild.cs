using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JenkinsClient
{
    public class QueuedBuild : Build
    {
        public int itemId { get; private set; }

        internal QueuedBuild(int itemId, Job job) : 
            base(job, -1, null)
        {
            this.itemId = itemId;
        }

        private async Task<bool> WaitForBuildStart(Timeout timeout)
        {
            if (PollingInterval > timeout.remaining)
                Console.WriteLine("");

            while (!timeout.isExpired)
            {
                var item = await client.GetBuildQueue().GetItem(itemId);

                if (item.buildNumber.HasValue)
                {
                    number = item.buildNumber.Value;

                    return true;
                }

                await Task.Delay(PollingInterval);
            }

            return false;
        }
        public async Task<bool> WaitForBuildStart(int timeout)
        {
            return await WaitForBuildStart(new Timeout(timeout));
        }
        public async Task WaitForBuildStart()
        {
            await WaitForBuildStart(new Timeout(Timeout.Infinite));
        }
        public async Task<bool> WaitForBuildStart(CancellationToken ct)
        {
            return await WaitForBuildStart(new Timeout(ct));
        }
    }
}
