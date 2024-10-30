using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using pcnintegrationservices;
using pcnintegrationservices.Data;
using pcnintegrationservices.Configuration;
using pcnintegrationservices.Services;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Annotations;

var builder = WebApplication.CreateBuilder(args);

// Configuración de Entity Framework y cadena de conexión
// builder.Services.AddDbContext<AppDbContext>(options =>
//     options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuración de Mailgun
builder.Services.Configure<MailgunSettings>(builder.Configuration.GetSection("MailgunSettings"));
builder.Services.AddScoped<EmailService>();

// Agregar servicios para controladores y Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Configuración de Swagger con anotaciones
builder.Services.AddSwaggerGen(options =>
{
    options.EnableAnnotations(); // Habilita el uso de anotaciones
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "PCN Third parties integration services",
        Version = "v1",
        Description = "Documentación de la API con descripciones detalladas de los endpoints"
    });
});

var app = builder.Build();

// Configuración de Swagger
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuración de Middleware
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
