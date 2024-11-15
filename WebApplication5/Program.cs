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
Task.Run(async () => await Electron.WindowManager.CreateWindowAsync());
app.Run();