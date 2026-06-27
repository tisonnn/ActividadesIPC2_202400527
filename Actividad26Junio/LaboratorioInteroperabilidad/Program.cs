using Microsoft.EntityFrameworkCore;
using LaboratorioInteroperabilidad.Data;
using LaboratorioInteroperabilidad.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregar el Contexto de Base de Datos en Memoria
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseInMemoryDatabase("UsacAcademicoDb"));

// 2. Registrar el Servicio Academico con HttpClient
builder.Services.AddHttpClient<AcademicoService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();