using System.Collections.Generic;
using TurisManager.Models;
using TurisManager.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<TurisManagerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TurisManagerContext")));
builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Auth/Login"; 
        options.AccessDeniedPath = "/Auth/Login";
    });
builder.Services.AddAuthorization();

var connectionString = builder.Configuration.GetConnectionString("TurisManagerContext");
Console.WriteLine(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapGet("/", async context =>
{
    context.Response.Redirect("/Auth/Login");
    await context.Response.CompleteAsync();
});

app.MapRazorPages();

app.Run();