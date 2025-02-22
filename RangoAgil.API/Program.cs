using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.API.DBContexts;
using RangoAgil.API.Entities;
using RangoAgil.API.Models;
using System.Collections.Generic;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<RangoDbContext>(
    o => o.UseSqlite(builder.Configuration["ConnectionStrings:RangoDbConnStg"])
);

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

var app = builder.Build();

app.MapGet("/", () => "Hello World!");


var rangosEndpoint = app.MapGroup("/rangos");
var rangosComIdEndpoint = rangosEndpoint.MapGroup("/{rangoId:int}");
var ingredientesEndpoint = rangosComIdEndpoint.MapGroup("/ingredientes");


rangosEndpoint.MapGet("", async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>>
                       (RangoDbContext rangoDbContext,
                       IMapper mapper,
                       [FromQuery(Name = "name")] string? rangoNome) =>
{

                           var rangosEntity = await rangoDbContext.Rangos
                                                      .Where(x => rangoNome == null || x.Nome.ToUpper().Contains(rangoNome.ToUpper()))
                                                      .ToListAsync();

                           if (rangosEntity.Count <= 0 || rangosEntity == null)

                               return TypedResults.NoContent();

                           else

                               return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangosEntity));

});



rangosEndpoint.MapPost("", async Task<CreatedAtRoute<RangoDTO>> (RangoDbContext rangoDbContext,
                             IMapper mapper,
                             [FromBody] RangoParaCriacaoDTO rangoParaCriacaoDTO) =>
{
    var rangoEntity = mapper.Map<Rango>(rangoParaCriacaoDTO);

    rangoDbContext.Add(rangoEntity);

    await rangoDbContext.SaveChangesAsync();

    var rangoParaRetorno = mapper.Map<RangoDTO>(rangoEntity);

    return TypedResults.CreatedAtRoute(rangoParaRetorno, "GetRangos", new { rangoId = rangoParaRetorno.Id });

});


ingredientesEndpoint.MapGet("", async (
                                                       RangoDbContext rangoDbContext,
                                                       IMapper mapper,
                                                       int rangoId) =>
{

    return mapper.Map<IEnumerable<IngredienteDTO>>((await rangoDbContext.Rangos
                               .Include(rango => rango.Ingredientes)
                               .FirstOrDefaultAsync(rango => rango.Id == rangoId))?.Ingredientes);

});



rangosComIdEndpoint.MapGet("", async Task<Results<NoContent, Ok<RangoDTO>>> (
                                     RangoDbContext rangoDbContext,
                                     IMapper mapper,
                                     int rangoId) =>
{
    var rangoEntity = mapper.Map<RangoDTO>(await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId));

    if (rangoEntity == null)

        return TypedResults.NoContent();

    else

        return TypedResults.Ok(rangoEntity);

}).WithName("GetRangos");



rangosComIdEndpoint.MapPut("", async Task<Results<NotFound, Ok>>(
                                    RangoDbContext rangoDbContext
                                  , IMapper mapper
                                  , int rangoId
                                  , [FromBody] RangoParaAtualizacaoDTO rangoParaAtualizacaoDTO) =>
{

    var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

    if (rangosEntity == null)
        return TypedResults.NotFound();

    mapper.Map(rangoParaAtualizacaoDTO, rangosEntity);

    await rangoDbContext.SaveChangesAsync();

    return TypedResults.Ok();
    
});



rangosComIdEndpoint.MapDelete("", async Task<Results<NotFound, NoContent>> (
                                    RangoDbContext rangoDbContext
                                  , int rangoId) =>
{

    var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

    if (rangosEntity == null)
        return TypedResults.NotFound();

    rangoDbContext.Rangos.Remove(rangosEntity);

    await rangoDbContext.SaveChangesAsync();

    return TypedResults.NoContent();

});




app.Run();
