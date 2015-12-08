using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HttpMock;
using JenkinsClient;

namespace tests
{
    [TestClass]
    public class GetJobsAsyncTest
    {
        private Client client { get; set; }

        [TestInitialize]
        public void Setup()
        {
            var http = HttpMockRepository.At(Config.Host);

            http.Stub(x => x.Get("/api/json"))
                .ReturnFile("./Mocks/GetJobs.json")
                .OK();

            client = Client.Create(
                Config.Host, Config.User, Config.Password);
        }


        [TestMethod]
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
