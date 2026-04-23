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

if (app.Environment.IsDevelopment())
{
    await app.Services.EnsureDatabaseCreatedAndMigratedAsync(connection!);
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseCors("AllowAll");

app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();


