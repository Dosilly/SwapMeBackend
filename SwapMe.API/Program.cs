using Microsoft.EntityFrameworkCore;
using SwapMe.Application.Config;
using SwapMe.Extensions;
using SwapMe.Infrastructure.Sql.Contexts;
using SwapMe.Middleware;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.RegisterServices();
builder.Services.AddDbContext<UsersContext>(options => options.UseSqlServer(configuration.GetConnectionString("SwapMeUsers"),
    b => b.MigrationsAssembly(configuration.GetValue<string>("MigrationsAssembly"))));

builder.Services.AddControllers();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true); 
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Map config
builder.Services.Configure<AppSettingsConfig>(
    builder.Configuration.GetSection("Jwt"));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseAuthorization();

app.UseMiddleware<JwtMiddleware>();

app.MapControllers();

app.Run();