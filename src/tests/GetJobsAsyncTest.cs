using System;
using System.Threading.Tasks;
using System.Linq;
using NUnit.Framework;

using HttpMock;
using JenkinsClient;

namespace tests
{
    [TestFixture]
    public class GetJobsAsyncTest
    {
        private Client client { get; set; }

        [SetUp]
        public void Setup()
        {
            var http = HttpMockRepository.At(Config.Host);

            http.Stub(x => x.Get("/api/json"))
                .ReturnFile("./Mocks/GetJobs.json")
                .OK();

            client = Client.Create(
                Config.Host, Config.User, Config.Password);
        }


        [Test]
        public async Task GetJobsAsync()
        {
            var jobs = await client.GetJobsAsync();

            Assert.IsTrue(
                jobs.Where(m => m.name == "test_1").Any());
            Assert.IsTrue(
                jobs.Where(m => m.name == "test_2").Any());
            Assert.IsTrue(
                jobs.Where(m => m.name == "test_3").Any());

            Assert.AreEqual(
                jobs.Count, 3);
        }
    }
}
