
```markdown
# Packaging an ASP.NET Core MVC Application as a Desktop Executable with Electron.NET

This guide demonstrates how to package an ASP.NET Core MVC application as a standalone desktop executable using Electron.NET.

## Prerequisites

- .NET SDK (5.0 or later)
- Node.js and npm installed
- Electron.NET CLI

## 1. Set Up Electron.NET in Your MVC Project

### Install Electron.NET CLI
Open a terminal and install the Electron.NET CLI globally:

```bash
dotnet tool install ElectronNET.CLI -g
dotnet new tool-manifest
```

### Add Electron.NET to Your Project
In your MVC project, add the Electron.NET API package by running:

```bash
dotnet add package ElectronNET.API
```

### Initialize Electron.NET
Open `Program.cs` and configure Electron.NET by adding setup:

```csharp
builder.WebHost.UseElectron(args);
```

### Enable Electron Startup
In the `Program.cs` before `app.Run()` add method to open a new Electron window:

```csharp
if (HybridSupport.IsElectronActive)
    Task.Run(async () => await Electron.WindowManager.CreateWindowAsync());
```

## 2. Build the Electron App

### Initialize Electron
Run the following command to initialize Electron for your app:

```bash
dotnet electronize init
```

### Start the App in Electron
To start the app in Electron during development, use:

```bash
dotnet electronize start
```

## 3. Package as an Executable

Once everything works in development, package the application as an executable:

```bash
dotnet electronize build /target win
```

Replace `/target win` with `/target mac` or `/target linux` depending on your platform. This command will create a packaged executable in the `bin/desktop` folder.

## Notes

- **Frontend UI**: While you can use MVC Views in the Electron app, consider using a frontend framework like Blazor, React, or Angular for a more interactive experience.
- **Resource Usage**: Electron applications can be resource-intensive, so optimizing your application is recommended.
- **System Permissions**: Ensure you handle file access, printing, and system interactions carefully if needed.

## 4. Automatic Database Initialization and Migration

### Program.cs Setup
Modify the `Program.cs` or wherever your app initializes dependencies to automatically create the database and apply migrations when the app starts.

```csharp
using ElectronNET.API;
using Microsoft.EntityFrameworkCore;
using WebApplication5.Data;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseElectron(args);
// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddDbContext<AppDbContext>(options=>options.UseInMemoryDatabase("InMemoryDatabase"));
builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlite("Data Source=app.db"));
builder.Services.AddCoreAdmin();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

var scope = app.Services.CreateScope();
var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
var pendingMigrations = dbContext.Database.GetPendingMigrations();
if (pendingMigrations.Any()) dbContext.Database.Migrate();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapDefaultControllerRoute();
app.UseCoreAdminCustomTitle("Admin Dashboard");
if (HybridSupport.IsElectronActive)
    Task.Run(async () => await Electron.WindowManager.CreateWindowAsync());
app.Run();
```

This code ensures that `Database.Migrate()` is called every time the app starts, which will create the database and apply any pending migrations.

## 5. Include Migration Files in the Published App

Make sure your migration files are included when you publish the app. Run the following commands to create and apply migrations locally:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

### Include Migrations in Build Output
Ensure that the migration files are included in your project’s build output by adding the following to your `.csproj`:

```xml
<ItemGroup>
    <None Update="Migrations\**\*">
        <CopyToOutputDirectory>PreserveNewest</CopyToOutputDirectory>
    </None>
</ItemGroup>
```

## 6. Electronize and Build the App

When you package the app with `electronize build`, it will include everything needed to initialize the database on the first run.

```bash
dotnet electronize build /target win
```

This ensures that your app is ready to be installed and initialized across different platforms.

## Summary

With this setup, the app will automatically create the database and apply migrations whenever it starts, ensuring users don’t need to run any database setup manually. This approach makes it straightforward to install and initialize the app across different platforms.
```

This README provides a structured guide for setting up an ASP.NET Core MVC application with Electron.NET, handling automatic database migration, and packaging the app as a desktop executable.
