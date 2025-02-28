using Microsoft.EntityFrameworkCore;
using RangoAgil.API.DBContexts;
using RangoAgil.API.Extensions;
using System.Net;


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConnStg"])
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddProblemDetails();

var app = builder.Build();


if(!app.Environment.IsDevelopment())
    app.UseExceptionHandler();


app.RegisterRangosEndpoints();
app.RegisterIngredientesEndpoints();



app.Run();
