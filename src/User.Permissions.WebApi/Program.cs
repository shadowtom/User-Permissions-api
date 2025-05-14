using Confluent.Kafka;
using Microsoft.OpenApi.Models;
using User.Permissions.WebApi.Extentions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddCors(o => o.AddPolicy("CorsPolicy", builder =>
{
    builder.AllowAnyOrigin()
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
Console.WriteLine($"Environment: {environment}");

if (string.IsNullOrEmpty(environment))
{
    Console.WriteLine("INFO: Environment variable not setted (ASPNETCORE_ENVIRONMENT)");
    Console.WriteLine("Default Environment: Development");
    environment = "Development";
}

builder.Configuration.AddJsonFile("appsettings.json", optional: true);
builder.Configuration.AddJsonFile($"appsettings.{environment}.json", optional: true);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();

    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Permissions API",
        Version = "v1",
        Description = "API for requesting, modifying, and retrieving permissions",
        Contact = new OpenApiContact
        {
            Name = "Tomas Felipe Castrillon Loaiza",
            Url = new Uri("https://www.linkedin.com/in/tomloaiza/"),
            Email = "tomasfelipeloaiza@gmail.com"
        }
    });
});
builder.Services.AddProjectDependencies(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Permissions API v1");
});

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
