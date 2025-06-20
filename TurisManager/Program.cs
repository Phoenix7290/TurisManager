using System.Collections.Generic;
using System.Globalization;
using TurisManager.Models;
using TurisManager.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    var supportedCultures = new[] { "pt-BR" };
    options.SetDefaultCulture(supportedCultures[0])
        .AddSupportedCultures(supportedCultures)
        .AddSupportedUICultures(supportedCultures);
});

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

var localizationOptions = app.Services.GetService<IOptions<RequestLocalizationOptions>>()!.Value;
app.UseRequestLocalization(localizationOptions);

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

app.MapGet("/", async context =>
{
    context.Response.Redirect("/Auth/Login");
    await context.Response.CompleteAsync();
});

app.MapRazorPages();

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