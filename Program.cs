using MongoDB.Driver;
using CrudApp.Services;

var builder = WebApplication.CreateBuilder(args);

// MongoDB
var mongoConnectionString = builder.Configuration["mongodb+srv://JakubUzar:HasloDB123>@clusteruzar.8ukgqsw.mongodb.net/?appName=ClusterUzar"];
builder.Services.AddSingleton<IMongoClient>(new MongoClient(mongoConnectionString));
builder.Services.AddScoped<MongoDbService>();

// Serwisy
builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
    );
});

var app = builder.Build();

app.UseCors("AllowAll");
app.MapControllers();
app.Run();
