using Newtonsoft.Json.Linq;

namespace JenkinsClient
{
    /// <summary>
    /// Represent an artefact that is stored once a buid is completed
    /// </summary>
    public class Artifact
    {
        protected JObject _data { get; private set; }

        public string displayPath
        {
            get
            {
                return (string)_data[nameof(displayPath)];
            }
        }
        public string fileName
        {
            get
            {
                return (string)_data[nameof(fileName)];
            }
        }
        public string relativePath
        {
            get
            {
                return (string)_data[nameof(relativePath)];
            }
        }

        public Artifact(JObject data)
        {
            _data = data;
        }
    }
}

