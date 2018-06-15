@ECHO OFF
SETLOCAL
SET VERSION=%1
SET NUGET=nuget
SET DOTNET=dotnet
SET CURRENT_PATH=%cd%


ECHO "Shared"
FOR /r %%f IN (*.Core.%VERSION%.nupkg) DO (
  copy "%%f" "%CURRENT_PATH%/Output/%Version%"
)

cd %CURRENT_PATH%
FOR /r %%f IN (*.Shared.%VERSION%.nupkg) DO (
  copy "%%f" "%CURRENT_PATH%/Output/%Version%"
)

cd %CURRENT_PATH%
ECHO "Android"
FOR /r %%f IN (*.Android.nuspec) DO (
  ECHO %%f
  cd %%~dpf
  %NUGET% pack -Version %VERSION% -OutputDirectory %CURRENT_PATH%/Output/%Version%
)

cd %CURRENT_PATH%
ECHO "iOS"
FOR /r %%f IN (*.iOS.nuspec) DO (
  cd %%~dpf
  %NUGET% pack -Version %VERSION% -OutputDirectory %CURRENT_PATH%/Output/%Version%
)
