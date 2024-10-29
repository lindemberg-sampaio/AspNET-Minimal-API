using Microsoft.EntityFrameworkCore;
using RangoAgil.API.DBContexts;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConnStg"])
);

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
