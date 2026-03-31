using SmartShelf.web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// API controllers
builder.Services.AddControllers();

builder.Services.AddDbContext<SmartShelfContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("SmartShelfContext")
    ));

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();