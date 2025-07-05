using CRUD.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using TodoList.Services;

var builder = WebApplication.CreateBuilder(args);

Env.Load();

var connectionString = DatabaseConfig.MySqlStringConnection();
builder.Configuration["ConnectionStrings:AppDbConnectionString"] = connectionString;

//var connectionString = builder.Configuration.GetConnectionString("AppDbConnectionString");

builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql
    (connectionString, ServerVersion.AutoDetect(connectionString)));

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
