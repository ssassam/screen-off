@echo off
"%WINDIR%\Microsoft.NET\Framework64\v4.0.30319\csc.exe" /nologo /codepage:65001 /target:winexe /win32icon:"%~dp0ScreenOff.ico" /out:"%~dp0ScreenOff.exe" "%~dp0ScreenOff.cs"
