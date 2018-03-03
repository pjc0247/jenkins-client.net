using System;
using System.Threading.Tasks;
using System.Linq;
using NUnit.Framework;

using HttpMock;
using JenkinsClient;

namespace tests
{
    [TestFixture]
    public class GetBuildDetails
    {
        private Client _client { get; set; }

        [SetUp]
        public void Setup()
        {
            var http = HttpMockRepository.At(Config.Host);
            var test = Environment.CurrentDirectory;
            http.Stub(x => x.Get("/job/jobName/368/api/json"))
                .ReturnFile(@"tests\Mocks\GetBuildData.json")
                .OK();
            http.Stub(x => x.Get("/job/jobName/369/api/json"))
                .ReturnFile(@"tests\Mocks\GetBuildDataWithoutArtifacts.json")
                .OK();
            http.Stub(x => x.Get("/job/jobName/370/api/json"))
                .ReturnFile(@"tests\Mocks\GetBuildDataWithoutArtifacts2.json")
                .OK();

            _client = Client.Create(
                Config.Host, Config.User, Config.Password);
        }


        [Test]
        public void BuildDetails()
        {
            Build build = new Build(new Job(_client, "jobName"), 368, "/job/jobName/368/api/json");
            build.EnsureDataInLocal();
            Assert.That(build.artifacts, Is.Not.Null);
            Assert.That(build.artifacts.Count, Is.EqualTo(2));
            Assert.That(build.artifacts[0].displayPath, Is.EqualTo("testDisplayPath.exe"));
            Assert.That(build.artifacts[0].fileName, Is.EqualTo("testFileName.exe"));
            Assert.That(build.artifacts[0].relativePath, Is.EqualTo("testRelativePath.exe"));
        }

        [Test]
        public void BuildDetailsWithoutArtifacts()
        {
            Build build = new Build(new Job(_client, "jobName"), 369, "/job/jobName/369/api/json");
            build.EnsureDataInLocal();
            Assert.That(build.artifacts, Is.Not.Null);
            Assert.That(build.artifacts.Count, Is.EqualTo(0));
        }

        [Test]
        public void BuildDetailsWithoutArtifacts2()
        {
            Build build = new Build(new Job(_client, "jobName"), 370, "/job/jobName/370/api/json");
            build.EnsureDataInLocal();
            Assert.That(build.artifacts, Is.Not.Null);
            Assert.That(build.artifacts.Count, Is.EqualTo(0));
        }

    }
}
