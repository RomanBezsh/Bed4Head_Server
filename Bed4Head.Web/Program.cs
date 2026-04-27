using Bed4Head.Application.Extensions;
using Bed4Head.Infrastructure.Extensions;
using Bed4Head.Web.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

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

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],

            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!)
            ),

            RoleClaimType = ClaimTypes.Role // 
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
        policy.RequireRole("Admin"));
});

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

await app.Services.EnsureAdminUsersAsync(app.Configuration);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();
app.UseCors("AllowAll");

app.UseStaticFiles();

app.UseAuthentication(); // важно перед Authorization
app.UseAuthorization();

app.MapControllers();

app.Run();