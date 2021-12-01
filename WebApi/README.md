# Getting Started
* This project has been rename and refactor to a .Net Core 3.1 WebApi project
* The products.db has also been modified
* The idea of refactor the project to WebApi. So that it can be used for multiple UIs
* It can be extened, modified and scale with limited impact to UI.

## You will need:
* .NET Core 3.0+
* .NET CLI

## Project Dependencies
This project reference
	* KenTan.API - This project can be export as Nuget
	* KenTan.DataLayer - This project design to interact with DataLayer, the sqlite can be swabbed easily with other DataStorage.


## Source Code
Clone this repo:

````Terminal
git clone https://github.com/wejk/CodeChallenge.git
cd CodeChallenge
````

## Build
````Terminal
dotnet build
````

## Test
From the solution folder
```Terminal
dotnet test
```
## Run
From the solution folder
```Terminal
dotnet run -p WebApi.csproj
```

## Swagger UI
The local swagger URL is: http://localhost:5001/swagger/index.html

