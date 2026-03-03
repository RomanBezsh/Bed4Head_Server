using Bed4Head.BLL.Extensions;
using Bed4Head.BLL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// 1. Добавляем сервисы Swagger в контейнер
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDataAccessLayer(connection);
builder.Services.AddUnitOfWorkService();
builder.Services.AddScoped<IUserService, UserService>();
var app = builder.Build();

// 2. Включаем Swagger только в режиме разработки (Development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); // Это создаст красивую страницу
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();