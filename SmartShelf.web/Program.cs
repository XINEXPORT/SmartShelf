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
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ISummaryService, SummaryService>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();