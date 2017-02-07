@echo off
cd /d %~dp0
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe ../NE.MPS.sln /p:Configuration=Release /t:Rebuild

:: "WebProjectOutputDir=$(OutputPath_Version_Temp)\; OutDir=$(OutputPath_Version_Temp)\bin\;Configuration=$(Configuration)"
@pause 

::/v:m
::On 32-bit machines they can be found in: C:\Program Files\MSBuild\12.0\bin On 64-bit machines the 32-bit tools will be under: C:\Program Files (x86)\MSBuild\12.0\bin and the 64-bit tools under: C:\Program Files (x86)\MSBuild\12.0\bin\amd64
::You can find MSBuild locations by typing in Developer Command Prompt for Visual Studio:

::where msbuild
::Should output both the legacy (4.0) and the newest binary (12.0,14.0+) locations:

::C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe
::C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe
