var target = Argument("target", "Build");
var configuration = Argument("configuration", "Debug");

//////////////////////////////////////////////////////////////////////
// TASKS
//////////////////////////////////////////////////////////////////////


Information("Beginning cleaning application.....");
Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>
{
    CleanDirectory($"./bin/{configuration}");
});

Information("Beginning building application.....");

Task("Build")
    .IsDependentOn("Clean")
    .Does(() =>
{
    DotNetBuild("./PaperlessRestService/PaperlessRestService.sln", new DotNetBuildSettings
    {
        Configuration = configuration,
    });
});


Information("DotNetTest task .....");

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetTest("./PaperlessRestService/PaperlessRestService.sln", new DotNetTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

//////////////////////////////////////////////////////////////////////
// EXECUTION
//////////////////////////////////////////////////////////////////////

RunTarget(target);