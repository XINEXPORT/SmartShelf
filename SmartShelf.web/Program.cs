using Microsoft.EntityFrameworkCore;
using SmartShelf.web.Data;
using SmartShelf.web.Interfaces;
using SmartShelf.web.Services;

var builder = WebApplication.CreateBuilder(args);

// API controllers
builder.Services.AddControllers();

builder.Services.AddDbContext<SmartShelfContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SmartShelfContext")
    ));

//Services
builder.Services.AddScoped<ISummaryService, SummaryService>();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddHttpClient<EmailService>();

//CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
               "http://localhost:5173"
           )
                 .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        });
});

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseAuthorization();

app.MapControllers();

app.Run();