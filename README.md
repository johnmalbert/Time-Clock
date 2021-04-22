# Time-Clock
Almost every hourly employee in the world has had to punch in or punch out at some point in their lifetime. This project (a work in progress) attempts to limit the headache that clocking in tends to create.

This project was built using C#, ASP.NET, and MySQL. Here's how you can get this app up and running:

Create an appsettings.json file on the project level of the application.
In appsettings.json, enter the following code, substituting your password, port (commonly 3306), and database name
{
    "Logging": {
      "LogLevel": {
        "Default": "Information",
        "Microsoft": "Warning",
        "Microsoft.Hosting.Lifetime": "Information"
      }
    },
    "AllowedHosts": "*",
      "DBInfo":
      {
          "Name": "MySQLconnect",
          "ConnectionString": "server=localhost;userid=root;password=YOUR_PW;port=YOUR_PORT;database=DB_NAME;SslMode=None"
      }
  }
Save appsettings.json.

If you don't have Entity Framework Core installed, run
```
dotnet tool install --global dotnet-ef
```

Run the following commands to install the Pomelo package and Entity Framework Core, which connect the app to MySQL
```
dotnet add package Pomelo.EntityFrameworkCore.MySql --version 3.1.1
dotnet add package Microsoft.EntityFrameworkCore.Design --version 3.1.5
dotnet ef migrations add FirstMigration
dotnet ef database update
```
Finally, you are good to go! Run this on the project level (same as Program.cs)
```
dotnet run
```
Open up localhost:5000 and start logging your time!
