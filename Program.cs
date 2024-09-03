using System.Reflection;
using FilmesApi.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<FilmeContext>(options => options.UseMySql(builder.Configuration.GetConnectionString("FilmeConnection"), ServerVersion.AutoDetect(builder.Configuration.GetConnectionString("FilmeConnection"))));

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()); // Adiciona o AutoMapper.

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(); // Adiciona os serviços de controladores.
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
        c.SwaggerDoc("v1", new OpenApiInfo { Title = "FilmesApi", Version = "v1" });
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    }
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization(); // Isso é opcional, dependendo da sua necessidade de autorização.

app.MapControllers(); // Mapeia os controladores para rotas.

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
