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
// Initial Setup
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

/////////////////////////////////////
// Building
/////////////////////////////////////

Task("Build-Navigation")
	.IsDependentOn("Restore-NuGet")
	.Does(() =>
	{
		DotNetCoreBuild("../AoLibs.Navigation.Test/AoLibs.Navigation.Test.csproj", new DotNetCoreBuildSettings
		{
			Configuration = "Release",
		});

		Build(
			"../AoLibs.Navigation.Android/AoLibs.Navigation.Android.csproj",
			"../AoLibs.Navigation.iOS/AoLibs.Navigation.iOS.csproj",
			"../AoLibs.Navigation.Core/AoLibs.Navigation.Core.csproj"
		);
	});

Task("Build-Adapters")
	.IsDependentOn("Restore-NuGet")
	.Does(() =>
	{
		DotNetCoreBuild("../AoLibs.Adapters.Test/AoLibs.Adapters.Test.csproj", new DotNetCoreBuildSettings
		{
			Configuration = "Release",
		});

		Build(
			"../AoLibs.Adapters.Android/AoLibs.Adapters.Android.csproj",
			"../AoLibs.Adapters.iOS/AoLibs.Adapters.iOS.csproj",
			"../AoLibs.Adapters.Core/AoLibs.Adapters.Core.csproj"
		);
	});

Task("Build-Utilities")
	.IsDependentOn("Restore-NuGet")
	.Does(() =>
	{
		Build(
			"../AoLibs.Utilities.Android/AoLibs.Utilities.Android.csproj",
			"../AoLibs.Utilities.iOS/AoLibs.Utilities.iOS.csproj",
			"../AoLibs.Utilities.Shared/AoLibs.Utilities.Shared.csproj"
		);
	});

/////////////////////////////////////
// Testing
/////////////////////////////////////

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

/////////////////////////////////////
// Packing
/////////////////////////////////////

Task("Pack-Navigation")
	//.IsDependentOn("Build-Navigation")
	.Does(() =>
	{
		Pack(
			"../AoLibs.Navigation.Android/AoLibs.Navigation.Android.csproj",
			"../AoLibs.Navigation.iOS/AoLibs.Navigation.iOS.csproj",
			"../AoLibs.Navigation.Core/AoLibs.Navigation.Core.csproj"
		);
	});

Task("Pack-Adapters")
	.IsDependentOn("Restore-NuGet")
	.Does(() =>
	{
		Pack(
			"../AoLibs.Adapters.Android/AoLibs.Adapters.Android.csproj",
			"../AoLibs.Adapters.iOS/AoLibs.Adapters.iOS.csproj",
			"../AoLibs.Adapters.Core/AoLibs.Adapters.Core.csproj"
		);
	});

Task("Pack-Utilities")
	.IsDependentOn("Restore-NuGet")
	.Does(() =>
	{
		Pack(
			"../AoLibs.Utilities.Android/AoLibs.Utilities.Android.csproj",
			"../AoLibs.Utilities.iOS/AoLibs.Utilities.iOS.csproj",
			"../AoLibs.Utilities.Shared/AoLibs.Utilities.Shared.csproj"
		);
	});

/////////////////////////////////////
// Aggregates
/////////////////////////////////////

Task("Build-All")
	.IsDependentOn("Build-Navigation")
	.IsDependentOn("Build-Adapters")
    .IsDependentOn("Build-Utilities");

Task("Test-All")
	.IsDependentOn("Test-Navigation")
	.IsDependentOn("Test-Adapters");

Task("Pack-All")
	.IsDependentOn("Pack-Navigation")
	.IsDependentOn("Pack-Adapters")
    .IsDependentOn("Pack-Utilities");

/////////////////////////////////////
// Publishing
/////////////////////////////////////


Task("Gather-Packages")
	.IsDependentOn("Build-All")
	.IsDependentOn("Test-All")
	.IsDependentOn("Pack-All")
	.Does(() => 
	{
		if(DirectoryExists($"publish/{version}"))
			DeleteDirectory($"publish/{version}", true);

		CreateDirectory($"publish/{version}");
		MoveFiles("*.nupkg", $"publish/{version}/");
		MoveFiles("../AoLibs.Adapters.Core/bin/Release/**/*.nupkg", $"publish/{version}/");
		MoveFiles("../AoLibs.Navigation.Core/bin/Release/**/*.nupkg", $"publish/{version}/");
		MoveFiles("../AoLibs.Utilities.Shared/bin/Release/**/*.nupkg", $"publish/{version}/");
	});



Task("Publish-Packages")
	.IsDependentOn("Gather-Packages")
	.Does(() => 
	{	
		foreach(var file in GetFiles($"publish/{version}/*.nupkg"))
		{
			DotNetCoreNuGetPush(file.FullPath ,new DotNetCoreNuGetPushSettings()
			{
				ApiKey = "$ENV:NuGetApiKey",
				Source = EnvironmentVariable("NuGetFeed"),
			});
		}
	});


/////////////////////////////////////
// Utilities
/////////////////////////////////////

private void Pack(string pathAndroid,string pathiOS,string pathShared)
{
	NuGetPack(pathAndroid, new NuGetPackSettings() 
	{
		Version = version,
		Properties = {
			["Configuration"] = "Release"
		}
	});
	NuGetPack(pathiOS, new NuGetPackSettings() 
	{
		Version = version,
		Properties = {
			["Configuration"] = "Release"
		}
	});
	DotNetCorePack(pathShared, new DotNetCorePackSettings()
	{
		ArgumentCustomization = (args) => 
		{
			return args.Append($"/p:Version={version}");
		},
		NoBuild = true,
		IncludeSymbols = true,
		Configuration = "Release"
	});
}

private void Build(string pathAndroid,string pathiOS,string pathShared)
{
	DotNetCoreBuild(pathShared, new DotNetCoreBuildSettings
		{
			Configuration = "Release",
		});
	MSBuild(pathAndroid, settings => settings.SetConfiguration("Release"));
	MSBuild(pathiOS, settings => settings
								.SetConfiguration("Release")
								.SetMSBuildPlatform(MSBuildPlatform.x86));
}

/////////////////////////////////////
// Bootstrap
/////////////////////////////////////

Task("Default")
	.Does(() => 
	{
		throw new Exception("Please specify correct build target.");
	});

Information(buildTarget);
RunTarget(buildTarget);
