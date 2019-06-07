using System;
using FlubuCore.Context;
using FlubuCore.Scripting;

namespace Build
{
    public class Build : DefaultBuildScript
    {
        protected override void ConfigureBuildProperties(IBuildPropertiesContext context)
        {
            context.Properties.Set(BuildProps.CompanyName, "Flubu");
            context.Properties.Set(BuildProps.CompanyCopyright, "Copyright (C) 2010-2019 FlubuCore");
            context.Properties.Set(BuildProps.ProductId, "FlubuCore.Slack");
            context.Properties.Set(BuildProps.ProductName, "FlubuCore.Slack");
            context.Properties.Set(BuildProps.BuildDir, "output");
            context.Properties.Set(BuildProps.SolutionFileName, "FlubuCore.Slack.sln");
            context.Properties.Set(BuildProps.BuildConfiguration, "Release");
        }

        protected override void ConfigureTargets(ITaskContext context)
        {
            var buildVersion = context.CreateTarget("buildVersion")
                .SetAsHidden()
                .SetDescription("Fetches flubu version from FlubuCore.Slack.ProjectVersion.txt file.")
                .AddTask(x => x.FetchBuildVersionFromFileTask());

            var compile = context.CreateTarget("Compile")
                .AddCoreTask(x => x.Clean())
                .AddCoreTask(x => x.UpdateNetCoreVersionTask("FlubuCore.Slack/FlubuCore.Slack.csproj"))
                .AddCoreTask(x => x.Build())
                .DependsOn(buildVersion);

            var nugetPublish = context.CreateTarget("Nuget.Publish")
                .DependsOn(compile)
                .AddCoreTask(x => x.Pack().Project("FlubuCore.Slack")
                    .NoBuild()
                    .IncludeSymbols()
                    .OutputDirectory("../output"))
                .Do(PublishNuGetPackage);

            context.CreateTarget("Rebuild")
                .SetAsDefault()
                .DependsOn(compile, nugetPublish);
        }

        private static void PublishNuGetPackage(ITaskContext context)
        {
            var version = context.Properties.GetBuildVersion();
            var nugetVersion = version.ToString(3);

            context.CoreTasks().NugetPush($"output\\FlubuCore.Slack.{nugetVersion}.nupkg")
                .ForMember(x => x.ApiKey("Not provided"), "nugetKey", "Nuget api key.")
                .ServerUrl("https://www.nuget.org/api/v2/package")
                .Execute(context);
        }
    }
}
