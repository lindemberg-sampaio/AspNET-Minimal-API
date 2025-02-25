using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RangoAgil.API.DBContexts;
using RangoAgil.API.Entities;
using RangoAgil.API.Models;


namespace RangoAgil.API.EndpointManipulador;

public static class RangosHandlers
{
    public static async Task<Results<NoContent, Ok<IEnumerable<RangoDTO>>>> GetRangosAsync
                            (RangoDbContext rangoDbContext
                           , IMapper mapper
                           , [FromQuery(Name = "name")] string? rangoNome)
    {


        var rangosEntity = await rangoDbContext.Rangos
                                .Where(x => rangoNome == null || x.Nome.ToUpper().Contains(rangoNome.ToUpper()))
                                .ToListAsync();

        if (rangosEntity.Count <= 0 || rangosEntity == null)
            return TypedResults.NoContent();

        else
            return TypedResults.Ok(mapper.Map<IEnumerable<RangoDTO>>(rangosEntity));

    }



    public static async Task<Results<NoContent, Ok<RangoDTO>>> GetRangosComIdAsync
                            (RangoDbContext rangoDbContext
                           , IMapper mapper
                           , int rangoId)
    {
        var rangoEntity = mapper.Map<RangoDTO>(await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId));

        if (rangoEntity == null)
            return TypedResults.NoContent();

        else
            return TypedResults.Ok(rangoEntity);

    }



    public static async Task<CreatedAtRoute<RangoDTO>> PostRangosAsync
                            (RangoDbContext rangoDbContext
                           , IMapper mapper
                           , [FromBody] RangoParaCriacaoDTO rangoParaCriacaoDTO)
    {
        var rangoEntity = mapper.Map<Rango>(rangoParaCriacaoDTO);

        rangoDbContext.Add(rangoEntity);

        await rangoDbContext.SaveChangesAsync();

        var rangoParaRetorno = mapper.Map<RangoDTO>(rangoEntity);

        return TypedResults.CreatedAtRoute(rangoParaRetorno, "GetRangos", new { rangoId = rangoParaRetorno.Id });

    }



    public static async Task<Results<NotFound, Ok>> PutRangosAsync
                            (RangoDbContext rangoDbContext
                           , IMapper mapper
                           , int rangoId
                           , [FromBody] RangoParaAtualizacaoDTO rangoParaAtualizacaoDTO)
    {

        var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

        if (rangosEntity == null)
            return TypedResults.NotFound();

        mapper.Map(rangoParaAtualizacaoDTO, rangosEntity);

        await rangoDbContext.SaveChangesAsync();

        return TypedResults.Ok();
    
    }



    public static async Task<Results<NotFound, NoContent>> DeleteRangosAsync
                            (RangoDbContext rangoDbContext
                           , int rangoId)
    {

        var rangosEntity = await rangoDbContext.Rangos.FirstOrDefaultAsync(x => x.Id == rangoId);

        if (rangosEntity == null)
            return TypedResults.NotFound();

        rangoDbContext.Rangos.Remove(rangosEntity);

        await rangoDbContext.SaveChangesAsync();

        return TypedResults.NoContent();

    }
}
