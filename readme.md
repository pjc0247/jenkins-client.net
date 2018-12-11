jenkins-client.net
====

__Jenkins REST API client for .Net__
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

/* Waits for the actual build starts from the pending queue. */
await item.WaitForBuildStart();
Console.WriteLine("Build Started");

/* Waits for the build to finish or failure. */
await item.WaitForBuildEnd();
Console.WriteLine("Build Finished");
```

Releases
----
__[Github Release Page](https://github.com/pjc0247/jenkins-client.net/releases)__<br><br>
or... You can install 'JenkinsClient' via NuGet.
```
PM> Install-Package JenkinsClient
```

Minimum Requirements
----
* C# 6.0
* .Net 4.5
* jenkins 1.519 (Version that jenkins started to include queued build info in build response)
