using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JenkinsClient
{
    public class BuildParameter
    {
        protected JObject data { get; private set; }

        public object defaultValue
        {
            get
            {
                if (!data["defaultParameterValue"].HasValues)
                    return null;

                return data["defaultParameterValue"]["value"];
            }
        }
        
        public string description
        {
            get
            {
                return (string)data[nameof(description)];
            }
        }
        public string name
        {
            get
            {
                return (string)data[nameof(name)];
            }
        }
        public string type
        {
            get
            {
                return (string)data[nameof(type)];
            }
        }

        internal BuildParameter(JObject data)
        {
            this.data = data;
        }
    }
}
