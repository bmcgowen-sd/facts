using AwesomeFacts.Data;
using AwesomeFacts.Models;
using AwesomeFacts.Services;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Awesome Facts API", Version = "v1" });
});

// Configure SQLite repository
var dbPath = Path.Combine(builder.Environment.ContentRootPath, "facts.db");
builder.Services.AddSingleton<IFactRepository>(_ => new SqliteFactRepository(dbPath));
builder.Services.AddScoped<IFactService, FactService>();
builder.Services.AddCors();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
app.UseAuthorization();

app.MapControllers();

// Set the port explicitly
app.Urls.Add("http://localhost:5000");
app.Urls.Add("https://localhost:5001");

app.Run();
