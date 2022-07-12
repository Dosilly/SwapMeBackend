using Microsoft.EntityFrameworkCore;
using SwapMe.Extensions;
using SwapMe.Infrastructure.Sql.Contexts;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services.RegisterServices();
builder.Services.AddDbContext<UsersContext>(options => options.UseSqlServer(configuration.GetConnectionString("SwapMeUsers"),
    b => b.MigrationsAssembly(configuration.GetValue<string>("MigrationsAssembly"))));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();