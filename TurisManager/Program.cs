using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using TurisManager.Data;
using TurisManager.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication("CookieAuth")
    .AddCookie("CookieAuth", options =>
    {
        options.LoginPath = "/Auth/Login";
        options.LogoutPath = "/Auth/Logout";
        options.AccessDeniedPath = "/Auth/Login";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
    });

builder.Services.AddAuthorization();
builder.Services.AddRazorPages();
builder.Services.AddDbContext<TurisManagerContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("TurisManagerContext")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.Use(async (context, next) =>
{
    if (context.Request.Path == "/" && !context.User.Identity.IsAuthenticated)
    {
        context.Response.Redirect("/Auth/Login");
        return;
    }
    await next();
});

app.MapRazorPages();

app.MapGet("/", context =>
{
    context.Response.Redirect("/Auth/Login");
    return Task.CompletedTask;
});

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<TurisManagerContext>();

    if (!context.Clientes.Any())
    {
        var clientes = new List<Cliente>
        {
            new Cliente { Nome = "ALGUM NOME", Email = "algumnome@example.com", IsDeleted = false },
            new Cliente { Nome = "OUTRO NOME", Email = "outronome@example.com", IsDeleted = false }
        };
        context.Clientes.AddRange(clientes);
        await context.SaveChangesAsync();
    }

    if (!context.PacotesTuristicos.Any())
    {
        var cidades = await context.CidadesDestinos.ToListAsync();
        var pacotes = new List<PacoteTuristico>
        {
            new PacoteTuristico
            {
                Titulo = "Pacote Paulista",
                DataInicio = DateTime.Today.AddDays(10),
                CapacidadeMaxima = 10,
                Preco = 1000,
                Destinos = new List<CidadeDestino> { cidades.FirstOrDefault(c => c.Nome == "São Paulo") }
            },
            new PacoteTuristico
            {
                Titulo = "Pacote Tokyo",
                DataInicio = DateTime.Today.AddDays(15),
                CapacidadeMaxima = 5,
                Preco = 5000,
                Destinos = new List<CidadeDestino> { cidades.FirstOrDefault(c => c.Nome == "Tóquio") }
            }
        };
        context.PacotesTuristicos.AddRange(pacotes);
        await context.SaveChangesAsync();
    }
}

app.Run();