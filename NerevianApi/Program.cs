using Microsoft.EntityFrameworkCore; // <--- AÑADE ESTA LÍNEA (1)
using NerevianApi.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddKeyPerFile(directoryPath: Directory.GetCurrentDirectory(), optional: true);
// O mejor aún, usa esta lógica simple:
if (File.Exists(".env"))
{
    foreach (var line in File.ReadAllLines(".env"))
    {
        var parts = line.Split('=', 2);
        if (parts.Length == 2) Environment.SetEnvironmentVariable(parts[0].Trim(), parts[1].Trim().Trim('"'));
    }
}
// ----------------------------

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                       ?? Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");

builder.Services.AddDbContext<NerevianDbContext>(options =>
    options.UseSqlServer(connectionString));

// 1. Add CORS policy (In English, of course)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// 2. Use the policy
app.UseCors("AllowAll");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();