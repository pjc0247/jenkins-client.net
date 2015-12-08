using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JenkinsClient;

namespace JenkinsClientExample.Example
{
    public class PrintAllJobs
    {
        public static async Task Execute()
        {
            var client = Client.Create(
                Config.Host, Config.User, Config.Password);

            foreach (var job in await client.GetJobsAsync())
            {
                await job.EnsureDataInLocalAsync();
                Console.WriteLine($"{job.name}");
                Console.WriteLine($"   URL : {job.url}");
                Console.WriteLine($"   NextBuildNumber : {job.nextBuildNumber}");
                Console.WriteLine($"   State : {job.color}");

                Console.WriteLine($"   BuildParameters");
                foreach (var param in job.parameters)
                {
                    Console.WriteLine($"      {param.name}");
                    Console.WriteLine($"         Default : {param.defaultValue}");
                    Console.WriteLine($"         Type : {param.type}");
                }

                await job.lastBuild.EnsureDataInLocalAsync();
                Console.WriteLine($"   LastBuild");
                Console.WriteLine($"      No : #{job.lastBuild.number}");
                Console.WriteLine($"      Result : {job.lastBuild.result}");
                Console.WriteLine($"      Duration : {job.lastBuild.duration}");

                Console.WriteLine();
            }
        }
    }
}
