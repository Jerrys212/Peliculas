using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using minimalAPIPeliculas;
using minimalAPIPeliculas.Repositorios;
using minimalAPIPeliculas.Rutas;

var builder = WebApplication.CreateBuilder(args);
var origenesPermitidos = builder.Configuration.GetValue<string>("origenesPermitidos")!;

//Inicio de area de los servicios

builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseSqlServer("name=DefaultConnection")
);

builder.Services.AddCors(opciones =>
{
    opciones.AddDefaultPolicy(configuracion =>
    {
        configuracion.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
    });

    opciones.AddPolicy(
        "libre",
        configuracion =>
        {
            configuracion.WithOrigins(origenesPermitidos).AllowAnyHeader().AllowAnyMethod();
        }
    );
});

builder.Services.AddOutputCache();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRepositorioGeneros, RepositorioGeneros>();

//Fin de area se servicios
var app = builder.Build();

//Inicio de area de middleware

//if (builder.Environment.IsDevelopment())
//{

//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors();
app.UseOutputCache();

app.MapGet("/", [EnableCors(policyName: "libre")] () => "otra cosa");

app.MapGroup("/generos").MapGeneros();

//Fin area de middlewares
app.Run();
