Pre reqs:
.NET 10.0 SDK
EF Core Tools: dotnet tool install --global dotnet-ef

How to Run:
dotnet restore
dotnet add package Microsoft.EntityFrameworkCore.Sqlite
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Swashbuckle.AspNetCore

Setup the Database:

dotnet ef migrations add InitialCreate
dotnet ef database update

Launch:

dotnet run
