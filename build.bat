@echo off

dotnet build CSharpWeb.Runtime\CSharpWeb.Runtime.csproj -c Release
if ERRORLEVEL 1 goto :fail

dotnet build CSharpWebApplication1\CSharpWebApplication1.csproj -c Release
if ERRORLEVEL 1 goto :fail

copy CSharpWeb.Runtime\bin\Release\netstandard1.3\CSharpWeb.Runtime.dll wwwroot\_framework

copy CSharpWebApplication1\bin\Release\netcoreapp1.0\CSharpWebApplication1.dll wwwroot\_bin

:done
@echo ---
@echo Build succeeded
@exit /b 0

:fail
@echo ---
@echo Build failed
@exit /b %ERRORLEVEL%