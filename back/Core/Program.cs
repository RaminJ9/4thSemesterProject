using Common;
using Common.Contracts;
using Core.Repositories;
using Core.Services;
using System.Composition.Hosting;
using System.Reflection;


var builder = WebApplication.CreateBuilder(args);

// Allows the front end to send http requests -------------------------
builder.Services.AddCors(options => {
    options.AddDefaultPolicy(policy => {
        policy.WithOrigins("http://localhost:5173")  // React port
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});
// --------------------------------------------------------------------

builder.Services.AddSingleton<ProductionService>();
builder.Services.AddSingleton<MachineService>();
builder.Services.AddSingleton<IMachineRepository, MachineRepository>();
builder.Services.AddSingleton<IProductionRepository, ProductionRepository>();

// Add services to the container.
builder.Services.AddControllers();


// MEF Specific initialization
var assemblies = Directory.EnumerateFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll")
    .Select(Assembly.LoadFrom)
    .ToList();

var types = assemblies
    .SelectMany(a => a.GetTypes())
    .Where(t => typeof(MachineComponentBase).IsAssignableFrom(t) && !t.IsAbstract)
    .ToList();

//var config = new ContainerConfiguration()
//    .WithAssemblies(assemblies);

//using var container = config.CreateContainer();

//var components = container.GetExports<MachineComponentBase>();

builder.Services.AddSingleton<IEnumerable<Type>>(types);


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseCors();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
