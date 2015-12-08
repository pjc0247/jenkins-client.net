using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JenkinsClient;

namespace JenkinsClientExample.Example
{
    public class BuildWithParameters
    {
        public static async Task Execute()
        {
            var client = Client.Create(
                Config.Host, Config.User, Config.Password);

            var job = client.GetJob("test_");

            var buildTask = await job.BuildAsync(new Dictionary<string, string>()
            {
                ["Version"] = "1.0.0"
            });
            Console.WriteLine("Build Queued");

            await buildTask.WaitForBuildStart();
            Console.WriteLine("Build Started");

            await buildTask.WaitForBuildEnd();
            Console.WriteLine("Build Finished");

            Console.WriteLine($"Result : {buildTask.result}");
        }
    }
}
