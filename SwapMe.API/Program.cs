using SwapMe.Application.Config;
using SwapMe.Extensions;

var builder = WebApplication.CreateBuilder(args);

var configuration = builder.Configuration;

// Add services to the container.
builder.Services
    .RegisterServices()
    .ConfigureDbContexts(configuration)
    .ConfigureAuthorization(builder);

builder.Services.AddControllers();
builder.Services.Configure<RouteOptions>(options => options.LowercaseUrls = true);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Map config
builder.Services.Configure<JwtSettings>(
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
app.UseAuthentication();

app.MapControllers();

app.Run();