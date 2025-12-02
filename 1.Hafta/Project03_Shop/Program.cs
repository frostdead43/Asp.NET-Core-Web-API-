using Microsoft.OpenApi;
using Project03_Shop.Middleware;
using Project03_Shop.Services;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
  opt.SwaggerDoc("v1", new OpenApiInfo {
    Title = "Eshop",
    Version = "V1",
    Description = "Test",
  });
});

builder.Services.AddScoped<IProductService, ProductService>();

var app = builder.Build();

app.UseRequestLogging();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
