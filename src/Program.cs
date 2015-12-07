using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace jenkins_client
{
    class Program
    {
        static async void Foo()
        {
            /*
            var client = Client.Create("http://192.168.0.221:8080", "pjc", "nzin5858");

            foreach (var jobs in await client.GetJobsAsync())
            {
                Console.WriteLine(jobs.name);

                Console.WriteLine(jobs.lastBuild.duration);
            }
            */
            var client = Client.Create("http://192.168.0.221:8080", "pjc", "nzin5858");

   
            
            var item = await client.GetJob("zinny_unity").BuildAsync(new Dictionary<string, string>()
            {
                ["Version"] = "9.9.9"
            });

            await item.WaitForBuildStart();
            Console.WriteLine("Build Started");
            await item.WaitForBuildEnd();
            Console.WriteLine("Build Ended");
        }

        static void Main(string[] args)
        {
            Foo();

            Console.Read();
        }
    }
}
