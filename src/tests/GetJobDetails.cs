using System;
using System.Threading.Tasks;
using System.Linq;
using NUnit.Framework;

using HttpMock;
using JenkinsClient;

namespace tests
{
    [TestFixture]
    public class GetJobDetails
    {
        private Client client { get; set; }

        [SetUp]
        public void Setup()
        {
            var baseDir = AppDomain.CurrentDomain.BaseDirectory;
            var http = HttpMockRepository.At(Config.Host);

            http.Stub(x => x.Get("/api/json"))
                .ReturnFile(baseDir + @"\Mocks\GetJobs.json")
                .OK();
            http.Stub(x => x.Get("/job/test_1/api/json"))
                .ReturnFile(baseDir + @"\Mocks\GetJobData_test_1.json")
                .OK();

            client = Client.Create(
                Config.Host, Config.User, Config.Password);
        }


        [Test]
        public async Task JobDetails()
        {
            var test_1 = client.GetJob("test_1");

            Assert.AreEqual(
                test_1.color, "red");
        }

        [Test]
        public async Task JobParameters()
        {
            var test_1 = client.GetJob("test_1");

            Assert.AreEqual(
                test_1.parameters.Count, 3);
        }
    }
}
