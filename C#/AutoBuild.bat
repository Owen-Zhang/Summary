@echo off
cd /d %~dp0
C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe ../NE.MPS.sln /p:Configuration=Release /t:Rebuild

@pause 
