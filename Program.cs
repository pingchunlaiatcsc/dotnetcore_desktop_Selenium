using System.Diagnostics;
using System;
using Microsoft.EntityFrameworkCore;
using dotnetcore_desktop_app.Models;
using dotnetcore_desktop_app.Data;
using Newtonsoft.Json.Serialization;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
//Setup Program.cs to use DbContext to .NET core
//builder.Services.AddDbContext<DUsersHappyaimonkeySourceReposDotnetcoreDesktopAppDataMydbMdfContext>(options => options.UseSqlServer(
//    builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<DataContext>(options=>options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddDbContext<ScheduleForSQLite_DataContext>(options=>options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

//Fix return Json() capitalization problem
builder.Services.Configure<JsonOptions>(options =>
{
    options.JsonSerializerOptions.PropertyNamingPolicy = null;
});

var app = builder.Build();
var url = "http://127.0.0.1:5000";

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.Urls.Add(url);
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
    //pattern: "{controller=Schedule}/{action=Schedule}/{id?}");
    pattern: "{controller=C341TaskSimulate}/{action=Index}");
if (!app.Environment.IsDevelopment())
{
    Process.Start(new ProcessStartInfo("cmd", $"/c start {url}")
    { CreateNoWindow = true });
}

app.Run();
