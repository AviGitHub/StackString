# StackString

Web implementation of stack data structure using backend DB
Currently running with dotnet entity framework as local database.

# Install:

Clone the reop:
git clone https://github.com/AviGitHub/StackString.git

Set the database:
dotnet ef database update --context StackContext

Test
dotnet build
dotnet run

To test the api, you can use postman with the following calls:
POST: https://localhost:5001/api/StackString/Push "test string"
POST: https://localhost:5001/api/StackString/Revert
GET: https://localhost:5001/api/StackString/Pick
DELETE: https://localhost:5001/api/StackString/Pop

# TODO list:

1. Add DB access abstraction layer for CRUD operations so DB instance could be injected via dotnet IOC DI mechanism
2. Replace hard coded connecion string with connection string from appsettings
3. Clean react client app files