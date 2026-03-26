using Common;
using Common.Contracts;
using System.Composition.Hosting;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();


// MEF Specific initialization
var assemblies = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "back.Component.*.dll")
    .Select(Assembly.LoadFrom)
    .ToList();

var config = new ContainerConfiguration()
    .WithAssemblies(assemblies);

using var container = config.CreateContainer();

var components = container.GetExports<IMachineComponent>();

builder.Services.AddSingleton<IEnumerable<IMachineComponent>>(components.ToList());


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

app.UseAuthorization();

app.MapControllers();

app.Run();
