using CRUD.Data;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;
using TodoList.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddCors(options =>
{
    options.AddPolicy("DesenvolvimentoAngular", policy =>
    {
        policy.WithOrigins( "http://localhost:4200", "http://localhost:5213/") // URL padrão do Angular
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

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

app.UseCors("DesenvolvimentoAngular"); 
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
