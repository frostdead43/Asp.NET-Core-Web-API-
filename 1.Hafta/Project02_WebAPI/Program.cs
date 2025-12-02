using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

//DI Container
builder.Services.AddControllers();
builder.Services.AddOpenApi();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opt =>
{
  opt.SwaggerDoc("v1", new OpenApiInfo
  {
    Title = "DENEME",
    Version = "v1",
    Description = "Deneme Desc",
    Contact = new OpenApiContact
    {
      Name = "Makif Kyilmaz"
    },
  }); 
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();


app.Run();


// MVC