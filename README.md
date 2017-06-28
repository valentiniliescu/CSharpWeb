# CSharpWeb
**Running C# in the browser via WebAssembly**

Based on the excellent [Blazor](https://github.com/SteveSanderson/Blazor) project, I wanted to be able to run a simple C# program in the browser. 
It runs .NET code in the browser via a small, portable .NET runtime called DotNetAnywhere (DNA) compiled to WebAssembly.

## Getting started

You will need the .NET CLI Tools, they come with VS 2017. Run `build.bat` to build and deploy the binaries. 
You will need a web server to serve the static content, there is a config file and a start script for IISExpress. 
