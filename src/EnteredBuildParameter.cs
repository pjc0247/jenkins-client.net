using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace jenkins_client
{
    public class EnteredBuildParameter
    {
        protected JObject data { get; private set; }

        public object value
        {
            get
            {
                return data["value"];
            }
        }
        public string name
        {
            get
            {
                return (string)data[nameof(name)];
            }
        }

        internal EnteredBuildParameter(JObject data)
        {
            this.data = data;
        }
    }
}
