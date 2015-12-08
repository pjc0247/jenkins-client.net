using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using JenkinsClient;

namespace JenkinsClientExample
{
    using Example;

    class Program
    {
        static async void ExecuteAll()
        {
            await PrintAllJobs.Execute();
            await BuildWithParameters.Execute();
        }

        static void Main(string[] args)
        {
            ExecuteAll();

            Console.Read();
        }
    }
}
