#addin "Cake.FileHelpers"
#addin "Cake.Xamarin"
#tool "nuget:?package=xunit.runner.console"

// Arguments
var buildTarget = Argument("buildTarget", "Build-Packages");
var outputPath = Argument("outputPath", "output/");
var version = Argument<string>("libVersion", null);

// Directories&Files
var solutionFile = File("../AoLibs.sln");


// Variables


/////////////////////////////////////
// Main Targets
/////////////////////////////////////

Task("Clean")
	.Does(() =>
	{
        foreach (var project in ParseSolution(solutionFile).Projects) 
        {
            {
                var path = project.Path.ToString();
                if(!path.EndsWith(".csproj"))
                    continue;
		
                var tokens = path.Split('/').ToList();
                var folderPath = string.Join("/",tokens.Take(tokens.Count-1));

                CleanDirectory($"{folderPath}/bin");
            }
        }
	});

Task("Restore-NuGet")
	.IsDependentOn("Clean")
	.Does(() =>
	{
		NuGetRestore(solutionFile);
	});

////////////////////////////////

Task("Build-Navigation")
	.IsDependentOn("Restore-NuGet")
	.Does(() =>
	{
		DotNetCoreBuild("../AoLibs.Navigation.Core/AoLibs.Navigation.Core.csproj", new DotNetCoreBuildSettings
		{
			Configuration = "Release",
			ArgumentCustomization = args => args.Append($"/p:Version=\"{version}\"")
		});
		DotNetCoreBuild("../AoLibs.Navigation.Test/AoLibs.Navigation.Test.csproj", new DotNetCoreBuildSettings
		{
			Configuration = "Release",
		});

		MSBuild("../AoLibs.Navigation.Android/AoLibs.Navigation.Android.csproj", settings => settings.SetConfiguration("Release"));
		MSBuild("../AoLibs.Navigation.iOS/AoLibs.Navigation.iOS.csproj", settings => settings
																		.SetConfiguration("Release")
																		.SetMSBuildPlatform(MSBuildPlatform.x86));
	});

Task("Build-Adapters")
	.IsDependentOn("Restore-NuGet")
	.Does(() =>
	{
		DotNetCoreBuild("../AoLibs.Adapters.Core/AoLibs.Adapters.Core.csproj", new DotNetCoreBuildSettings
		{
			Configuration = "Release",
			ArgumentCustomization = args => args.Append($"/p:Version=\"{version}\"")
		});
		DotNetCoreBuild("../AoLibs.Adapters.Test/AoLibs.Adapters.Test.csproj", new DotNetCoreBuildSettings
		{
			Configuration = "Release",
		});

		MSBuild("../AoLibs.Adapters.Android/AoLibs.Adapters.Android.csproj", settings => settings.SetConfiguration("Release"));
		MSBuild("../AoLibs.Adapters.iOS/AoLibs.Adapters.iOS.csproj", settings => settings
																	.SetConfiguration("Release")
																	.SetMSBuildPlatform(MSBuildPlatform.x86));
	});

Task("Build-Utilities")
	.IsDependentOn("Restore-NuGet")
	.Does(() =>
	{
		DotNetCoreBuild("../AoLibs.Utilities.Shared/AoLibs.Utilities.Shared.csproj", new DotNetCoreBuildSettings
		{
			Configuration = "Release",
			ArgumentCustomization = args => args.Append($"/p:Version=\"{version}\"")
		});
		MSBuild("../AoLibs.Utilities.Android/AoLibs.Utilities.Android.csproj", settings => settings
																				.SetConfiguration("Release")
																				.SetMSBuildPlatform(MSBuildPlatform.x86));
	});

////////////////////////////////

Task("Test-Navigation")
	.IsDependentOn("Build-Navigation")
	.Does(() =>
	{
		DotNetCoreTest("../AoLibs.Navigation.Test/AoLibs.Navigation.Test.csproj", new DotNetCoreTestSettings
		{
			Configuration = "Release",
		});
	});

Task("Test-Adapters")
	.IsDependentOn("Build-Adapters")
	.Does(() =>
	{
		DotNetCoreTest("../AoLibs.Adapters.Test/AoLibs.Adapters.Test.csproj", new DotNetCoreTestSettings
		{
			Configuration = "Release",
		});
	});

////////////////////////////////

Task("Pack-Navigation")
	.IsDependentOn("Restore-NuGet")
	.Does(() =>
	{

	});

Task("Pack-Adapters")
	.IsDependentOn("Restore-NuGet")
	.Does(() =>
	{

	});

Task("Pack-Utilities")
	.IsDependentOn("Restore-NuGet")
	.Does(() =>
	{

	});

////////////////////////////////

Task("Build-All")
	.IsDependentOn("Build-Navigation")
	.IsDependentOn("Build-Adapters")
    .IsDependentOn("Build-Utilities");

Task("Test-All")
	.IsDependentOn("Test-Navigation")
	.IsDependentOn("Test-Adapters");


// Task("Gather-Packages")
// 	.IsDependentOn("Build-Packages")
// 	.Does(() => 
// 	{

// 	});



// Task("Publish-Packages")
// 	.IsDependentOn("Gather-Packages")
// 	.Does(() => 
// 	{

// 	});

/////////////////////////////////////
// Timestamping
/////////////////////////////////////

Task("Default")
	.Does(() => 
	{
		throw new Exception("Please specify correct build target.");
	});

Information(buildTarget);
RunTarget(buildTarget);
