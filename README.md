# RequestLogger.NLog

RequestLogger that can be used with NLog.

[![Build status](https://ci.appveyor.com/api/projects/status/u9c8cuaejk4921qs/branch/master?svg=true)](https://ci.appveyor.com/project/mrstebo/requestlogger-nlog/branch/master)
[![Coverage Status](https://coveralls.io/repos/github/ekmsystems/RequestLogger.NLog/badge.svg?branch=master)](https://coveralls.io/github/ekmsystems/RequestLogger.NLog?branch=master)
[![NuGet](https://img.shields.io/nuget/v/RequestLogger.NLog.svg)](https://www.nuget.org/packages/RequestLogger.NLog/)

RequestLogger.NLog is available via [NuGet](https://www.nuget.org/packages/RequestLogger.NLog/):

```powershell
Install-Package RequestLogger.NLog
```

## Quick Start

There are some example projects in this repository that will show you how to use this package:

- [ASP.NET](src/Examples/AspNetExample)
- [Owin](src/Examples/OwinExample)

## Custom Log Format

If you want to customize the log that gets sent to NLog, then you need to create a custom `ILogFormatter`. This package comes with a [default formatter](src/RequestLogger.NLog/Formatters/DefaultLogFormatter.cs)
