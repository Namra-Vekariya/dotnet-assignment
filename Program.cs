using CollegeApp.Data;
using CollegeApp.MyLogging;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// clear all four default Logger 
builder.Logging.ClearProviders();

builder.Logging.AddConsole();
builder.Logging.AddDebug();



builder.Services.AddControllers().AddNewtonsoftJson();
// Add services to the container.

builder.Services.AddDbContext<CollegeDBContext> (options =>
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
        .EnableDetailedErrors()   // Enable detailed error messages
        .EnableSensitiveDataLogging() // Logs sensitive data like SQL queries (useful for debugging but should be turned off in production)
    );
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(); 

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IMyLogger,LogToFile>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("",()=>{
    return "namraDon";
});

app.MapControllers();
app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
