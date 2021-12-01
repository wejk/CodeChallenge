# Getting Started

- Please run WebApi project first.
- This project has a Config setting `BaseUrl`, its value should be the WebApi hosted address.
- Default value is https://localhost:5001

This project is designed as User interface for the Code challenge.
It will send GET/POST requests to WebApi project to get data and show them to users.
This project is written as a consumer to the WebApi project

## You will need:

- .NET Core 3.0+
- .NET CLI

## Source Code

Clone this repo:

```Terminal
git clone https://github.com/wejk/CodeChallenge.git
cd CodeChallenge
```

## Build

```Terminal
dotnet build
```

## Test

From the solution folder

```Terminal
dotnet test
```

## Run

From the solution folder

```Terminal
dotnet run -p WebUI.csproj
```

Local dev index page -> https://localhost:5003/
