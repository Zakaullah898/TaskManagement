using Microsoft.EntityFrameworkCore;
using System;
using TaskManagement.Web;
using TaskManagement.Infrastructure.Data;
using TaskManagement.Application.Automappr;
using TaskManagement.Domain.Interfaces;
using TaskManagement.Infrastructure.Utilities;
using TaskManagement.Infrastructure.Repositories;
using TaskManagement.Application.Services;
using Microsoft.AspNetCore.CookiePolicy;
var builder = WebApplication.CreateBuilder(args);
 // REQUIRED for JsonPatch

// Adding Automapper Configuration
builder.Services.AddAutoMapper(typeof(AutoMapperConfig));
// Adding dependency injection for HelpersMethods
builder.Services.AddScoped<IHelperMethods,HelperMethods>();

// Adding dependency injection for Repositories
builder.Services.AddScoped(typeof(ITaskManagementRepo<>), typeof(TaskManagementRepo<>));
builder.Services.AddScoped<IAssignUserRoleRepo, AssignUserRoleRepo>();
builder.Services.AddScoped<IDashboardServices, DashboardServices>();
builder.Services.AddScoped<IUserProfileService, UserProfileService>();

// Adding dependency injection for Services
builder.Services.AddScoped<ITaskService, TaskService>();
 builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TaskContext") ?? throw new InvalidOperationException("Connection string 'MvcMovieContext' not found.")));

// adding patch json support
builder.Services.AddControllersWithViews()
    .AddNewtonsoftJson();
// Required: Add the session services
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); // Set session timeout
    options.Cookie.HttpOnly = true; // Make the session cookie inaccessible to client-side scripts
    options.Cookie.IsEssential = true; // Make the session cookie essential for the app to function
});
// Configuration cookies policy
//builder.Services.Configure<CookiePolicyOptions>(options =>
//{
//    options.MinimumSameSitePolicy = SameSiteMode.Lax;
//    options.HttpOnly = HttpOnlyPolicy.Always;
//    options.Secure = CookieSecurePolicy.SameAsRequest;
//});

// Configuration for cookies
builder.Services.AddAuthentication("Cookies").AddCookie("Cookies", options =>
{
    options.LoginPath = "/Auth/Login";
    options.AccessDeniedPath = "/Auth/Login";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
});




builder.Services.AddControllersWithViews();


var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseRouting();
// 3. Enable the session middleware
app.UseSession();

app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();
