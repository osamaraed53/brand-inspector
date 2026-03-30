using BrandInspector;
using BrandInspector.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddOpenApi();

builder.Services.AddServices(builder.Configuration);

builder.Services.AddExceptionHandler<CustomExceptionHandler>();

var app = builder.Build();

app.UseServices();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
