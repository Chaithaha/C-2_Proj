using CreativeColab.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddControllers(); // Add this for API controllers

// Register the application's DbContext for MySQL (XAMPP)
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("DefaultConnection"))
    ));

// Register API controllers
builder.Services.AddScoped<CreativeColab.Controllers.GamesAPIController>();
builder.Services.AddScoped<CreativeColab.Controllers.ProjectsAPIController>();
builder.Services.AddScoped<CreativeColab.Controllers.PaymentsAPIController>();
builder.Services.AddScoped<CreativeColab.Controllers.UsersAPIController>();

// Register services
builder.Services.AddScoped<CreativeColab.Services.PriceTrackerService>();
builder.Services.AddScoped<CreativeColab.Services.NotificationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
