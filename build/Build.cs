using System.Collections.Generic;
using Nuke.Common;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
// ReSharper disable AllUnderscoreLocalParameterName

class Build : NukeBuild
{
    public static int Main() => Execute<Build>(x => x.Default);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? global::Configuration.Debug : global::Configuration.Release;

    [Solution] readonly Solution Solution;

    static AbsolutePath SourceDirectory => RootDirectory / "src";
    static AbsolutePath TestsDirectory => RootDirectory / "tests";

    static AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean => _ => _
        .Description("Cleans the output, bin and obj directories.")
        .Before(Restore)
        .Executes(() =>
        {
            ArtifactsDirectory.CreateOrCleanDirectory();
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(AbsolutePathExtensions.DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(AbsolutePathExtensions.DeleteDirectory);
        });

    Target Restore => _ => _
        .Description("Restores NuGet packages.")
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .Description("Compile the solution.")
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetNoRestore(InvokedTargets.Contains(Restore))
                .SetConfiguration(Configuration)
                .EnableNoLogo());
        });

    static AbsolutePath TestResultDirectory => ArtifactsDirectory / "test-results";

    static AbsolutePath CoverageResultDirectory => ArtifactsDirectory / "test-coverage";
    IEnumerable<Project> TestProjects => Partition.GetCurrent(Solution.GetAllProjects("*.Test"));

    Target Test => _ => _
        .Description("Runs unit tests and outputs test results to the output directory.")
        .DependsOn(Compile)
        .Partition(2)
        .Executes(() =>
        {
            DotNetTest(_ => _
                .SetConfiguration(Configuration)
                .SetNoBuild(InvokedTargets.Contains(Compile))
                .ResetVerbosity()
                .SetResultsDirectory(TestResultDirectory)
                .EnableCollectCoverage()
                .SetCoverletOutput(CoverageResultDirectory / "coverage")
                .SetCoverletOutputFormat("json%2copencover")
                .AddProperty("MergeWith", CoverageResultDirectory / "coverage.json")
                .CombineWith(TestProjects, (_, v) => _
                    .SetProjectFile(v)
                    .SetLoggers($"trx;LogFileName={v.Name}.trx")));
        });

    Target Default => _ => _.Description("Cleans, restores NuGet packages, compile the solution, runs unit tests.")
        .DependsOn(Clean)
        .DependsOn(Compile)
        .DependsOn(Test);

}
