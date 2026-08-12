using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using Transfors.Clientes.Api.Data;
using Transfors.Clientes.Api.Middleware;
using Transfors.Clientes.Api.Services;

var builder = WebApplication.CreateBuilder(args);

const string CorsPolicy = "AngularFrontend";

// --- Base de datos (EF Core + SQL Server) ---
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServer")));

// --- Servicios de aplicación ---
builder.Services.AddScoped<IClienteService, ClienteService>();

// --- Controllers + serialización de enums como texto ---
builder.Services.AddControllers()
    .AddJsonOptions(o => o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// --- CORS para el frontend Angular ---
builder.Services.AddCors(options =>
{
    options.AddPolicy(CorsPolicy, policy =>
        policy.WithOrigins(
                builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                    ?? new[] { "http://localhost:4200" })
              .AllowAnyHeader()
              .AllowAnyMethod());
});

var app = builder.Build();

// Aplica migraciones pendientes automáticamente al arrancar (cómodo para la prueba/demo).
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
}

app.UseMiddleware<ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(CorsPolicy);
app.UseAuthorization();
app.MapControllers();

app.Run();
