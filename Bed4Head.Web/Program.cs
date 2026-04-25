using Bed4Head.Application.Extensions;
using Bed4Head.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.WithOrigins("http://localhost:5173") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDataAccessLayer(connection!);
builder.Services.AddUnitOfWorkService();
builder.Services.AddAppServices();

var app = builder.Build();

// Ensure wwwroot and uploads exist and set as WebRootPath
string wwwrootPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot");
string uploadsPath = Path.Combine(wwwrootPath, "uploads");

if (!Directory.Exists(wwwrootPath)) Directory.CreateDirectory(wwwrootPath);
if (!Directory.Exists(uploadsPath)) Directory.CreateDirectory(uploadsPath);

app.Environment.WebRootPath = wwwrootPath;

if (app.Environment.IsDevelopment())
{
    await app.Services.EnsureDatabaseCreatedAndMigratedAsync(connection!);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");

app.UseStaticFiles(); // Serve everything from wwwroot, including uploads

app.MapControllers();
app.Run();


