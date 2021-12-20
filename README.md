# ProjectBank

## How to Run Program

You need a GUID password. You must generate your own however you like, and insert it in place of {password}

To run the program you must first have a docker container with a MySql database running:

```
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD={password}" -p 1433:1433 -d mcr.microsoft.com/mssql/server:2019-latest
```

Then add the connection string to the project. Do this in the /Server directory:

```
cd .\Server\
```
```
dotnet user-secrets set "ConnectionStrings:ProjectBank" "Server=localhost;Database=ProjectBank;User Id=sa;Password={password}"
```

Once this is done you can run the program with:

```
dotnet run 
```
