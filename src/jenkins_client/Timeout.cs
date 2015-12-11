using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics.Contracts;

namespace JenkinsClient
{
    internal class Timeout
    {
        public static readonly int Infinite = -1;

        protected int start { get; set; }
        protected int timeout { get; set; }

        protected CancellationToken ct { get; set; }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="timeout">
        /// 타임아웃 시간(ms),
        /// 음수가 들어올경우 항상 유효함
        /// </param>
        public Timeout(int timeout)
        {
            Contract.Requires(timeout > 0);

            this.timeout = timeout;
            this.start = Environment.TickCount;
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="ct">취소 토큰</param>
        public Timeout(CancellationToken ct)
        {
            Contract.Requires(ct != null);

            this.ct = ct;
        }

        public int remaining
        {
            get
            {
                if (ct == null)
                    return Math.Max(timeout - (Environment.TickCount - start), 0);
                else
                    return Int32.MaxValue;
            }
        }
        public bool isExpired
        {
            get
            {
                if(ct == null)
                {
                    if (timeout < 0)
                        return false;

                    return  remaining == 0;
                }
                else
                {
                    return ct.IsCancellationRequested;
                }
            }
        }
    }
}
