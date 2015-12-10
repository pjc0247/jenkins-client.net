using System;
using System.Threading;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace JenkinsClient
{
    public class LazyJObject : LazyObject<JObject>
    {
        public async Task EnsureDataInLocalAsync()
        {
            await FetchAsync();
        }
        public void EnsureDataInLocal()
        {
            FetchAsync().Wait();
        }
    }
    public class LazyObject<T>
    {
        private class DataStatus
        {
            public const int Preparing = 0;
            public const int Fetching = 1;
            public const int InLocal = 2;
        }

        private ManualResetEventSlim dataCompletionEvent { get; set; }
        private int dataStatus;

        protected T data { get; private set; }

        public LazyObject()
        {
            this.dataStatus = DataStatus.Preparing;
            this.dataCompletionEvent = new ManualResetEventSlim(false);
        }

        protected virtual Task<T> Fetch()
        {
            /* place a fetch implementation here */

            throw new NotImplementedException("You must override this method.");
        }

        protected Task FetchAsync()
        {
            return Task.Run(async () =>
            {
                Interlocked.MemoryBarrier();
                if (dataStatus == DataStatus.InLocal)
                    return;
                if (dataStatus == DataStatus.Fetching)
                    dataCompletionEvent.Wait();

                switch (Interlocked.Exchange(ref dataStatus, DataStatus.Fetching))
                {
                    case DataStatus.Fetching:
                        await FetchAsync();
                        return;
                    case DataStatus.InLocal:
                        return;
                }

                data = await Fetch();

                if (Interlocked.Exchange(ref dataStatus, DataStatus.InLocal) != DataStatus.Fetching)
                    throw new InvalidOperationException();

                dataCompletionEvent.Set();
            });
        }
        
        /* moved to LazyJObject
        public virtual async Task EnsureDataInLocalAsync()
        {
            await FetchAsync();
        }
        public virtual void EnsureDataInLocal()
        {
            FetchAsync().Wait();
        }
        */

        public void Invalidate()
        {
            if (Interlocked.CompareExchange(ref dataStatus, DataStatus.Preparing, DataStatus.InLocal)
                == DataStatus.InLocal)
            {
                dataCompletionEvent.Reset();
            }
        }
    }
}
