using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JenkinsClient
{
    public class OperationFailedException : Exception
    {
        public string body { get; private set; }

        public OperationFailedException(string msg, string body = "") :
            base(msg)
        {
            this.body = body;
        }
    }
}
