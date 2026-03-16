using Bed4Head.BLL.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

string? connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDataAccessLayer(connection);
builder.Services.AddUnitOfWorkService();
builder.Services.AddAppServices();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(); 
}

app.UseStaticFiles();
app.UseHttpsRedirection();
app.MapControllers();
app.Run();