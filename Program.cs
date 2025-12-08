using MongoDB.Driver;
using CrudApp.Services;

var builder = WebApplication.CreateBuilder(args);

var mongoConnectionString = Environment.GetEnvironmentVariable("ConnectionStrings__MongoDb") 
    ?? builder.Configuration["ConnectionStrings:MongoDb"]
    ?? throw new InvalidOperationException("MongoDB connection string is not configured!");

builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));
builder.Services.AddScoped<MongoDbService>();
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

var app = builder.Build();

// Serve static files (HTML, CSS, JS)
app.UseStaticFiles();

app.UseCors("AllowAll");
app.MapControllers();

// Fallback to index.html for SPA routing
app.MapFallbackToFile("index.html");

app.Run();
