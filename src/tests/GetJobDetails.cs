using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using HttpMock;
using JenkinsClient;

namespace tests
{
    [TestClass]
    public class GetJobDetails
    {
        private Client client { get; set; }

        [TestInitialize]
        public void Setup()
        {
            var http = HttpMockRepository.At(Config.Host);

            http.Stub(x => x.Get("/api/json"))
                .ReturnFile("./Mocks/GetJobs.json")
                .OK();
            http.Stub(x => x.Get("/job/test_1/api/json"))
                .ReturnFile("./Mocks/GetJobData_test_1.json")
                .OK();

            client = Client.Create(
                Config.Host, Config.User, Config.Password);
        }


        [TestMethod]
        public async Task JobDetails()
        {
            var test_1 = client.GetJob("test_1");

            Assert.AreEqual(
                test_1.color, "red");
        }

        [TestMethod]
        public async Task JobParameters()
        {
            var test_1 = client.GetJob("test_1");

            Assert.AreEqual(
                test_1.parameters.Count, 3);
        }
    }
}
