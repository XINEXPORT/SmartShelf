using SmartShelf.web.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// SmartShelf DB Context
builder.Services.AddDbContext<SmartShelfContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SmartShelfContext")));

var app = builder.Build();