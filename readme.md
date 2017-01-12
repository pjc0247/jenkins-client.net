jenkins-client.net
====

__Jenkins REST API client for .Net__  written in C#
<br>
[examples](https://github.com/pjc0247/jenkins-client.net/tree/master/src/examples/Example)

Usage
----
```c#
var client = Client.Create("http://your_jenkins_addr", "username", "password");

foreach(var job in await client.GetJobsAsync())
{
    Console.WriteLine(job.name);
    Console.WriteLine(job.lastBuild.result);
}
```

```c#
var job = client.GetJob("job_name");

var item = await job.BuildAsync(new Dictionary<string, string>() {
    ["param1"] = "value1"
    ["param2"] = "value2"
});
Console.WriteLine("Build Queued");

/* 요청한 빌드가 jenkins 빌드 큐에서 실제 빌드로 옮겨질 때 까지 대기한다. */
await item.WaitForBuildStart();
Console.WriteLine("Build Started");

/* 요청한 빌드가 완료, 혹은 중지될때까지 대기한다. */
await item.WaitForBuildEnd();
Console.WriteLine("Build Finished");
```

Releases
----
__[Github Release Page](https://github.com/pjc0247/jenkins-client.net/releases)__<br><br>
or... You can install 'JenkinsClient' via NuGet now.
```
PM> Install-Package JenkinsClient
```

Minimum Requirements
----
* C# 6.0
* .Net 4.5
* jenkins 1.519 (Version that jenkins started to include queued build info in build response)
