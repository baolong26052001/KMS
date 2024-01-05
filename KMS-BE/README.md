Install these NuGet Packages:

dotnet add package Microsoft.AspNetCore.Mvc.NewtonsoftJson --version 6.0.0
dotnet add package Microsoft.EntityFrameworkCore --version 6.0.0
dotnet add package Microsoft.EntityFrameworkCore.Design --version 6.0.0
dotnet add package Microsoft.EntityFrameworkCore.SqlServer --version 6.0.0
dotnet add package Microsoft.EntityFrameworkCore.Tools --version 6.0.0
dotnet add package Swashbuckle.AspNetCore --version 6.5.0
dotnet add package IPinfo
dotnet add package MailKit
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package System.IdentityModel.Tokens.Jwt


After update in database, run this command:
Scaffold-DbContext "Server=DESKTOP-FQK88HL\SQLEXPRESS;Database=KioskManagementSystem;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force