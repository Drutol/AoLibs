@ECHO OFF
SETLOCAL
SET VERSION=%1
SET NUGET=nuget
SET DOTNET=dotnet
SET CURRENT_PATH=%cd%


cd %CURRENT_PATH%
ECHO "Shared"
FOR /r %%f IN (*.Core.nuspec) DO (
  cd %%~dpf
  call %DOTNET% pack /p:PackageVersion=%VERSION% -o %CURRENT_PATH%/Output/%Version% /p:NuspecFile=%%f
)
cd %CURRENT_PATH%
FOR /r %%f IN (*.Shared.nuspec) DO (
  cd %%~dpf
  call %DOTNET% pack /p:PackageVersion=%VERSION% -o %CURRENT_PATH%/Output/%Version% /p:NuspecFile=%%f
)

cd %CURRENT_PATH%
ECHO "Android"
FOR /r %%f IN (*.Android.nuspec) DO (
  ECHO %%f
  cd %%~dpf
  %NUGET% pack -Version %VERSION% -OutputDirectory %CURRENT_PATH%/Output/%Version% -IncludeReferencedProjects
)

cd %CURRENT_PATH%
ECHO "iOS"
FOR /r %%f IN (*.iOS.nuspec) DO (
  cd %%~dpf
  %NUGET% pack -Version %VERSION% -OutputDirectory %CURRENT_PATH%/Output/%Version% -IncludeReferencedProjects
)
