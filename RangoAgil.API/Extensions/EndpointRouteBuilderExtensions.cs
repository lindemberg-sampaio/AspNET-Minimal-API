using RangoAgil.API.EndpointManipulador;

namespace RangoAgil.API.Extensions;

public static class EndpointRouteBuilderExtensions
{

    public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {

        var rangosEndpoint = endpointRouteBuilder.MapGroup("/rangos");
        var rangosComIdEndpoint = rangosEndpoint.MapGroup("/{rangoId:int}");

        rangosEndpoint.MapGet("", RangosHandlers.GetRangosAsync);
        rangosComIdEndpoint.MapGet("", RangosHandlers.GetRangosComIdAsync).WithName("GetRangos");
        rangosEndpoint.MapPost("", RangosHandlers.PostRangosAsync);
        rangosComIdEndpoint.MapPut("", RangosHandlers.PutRangosAsync);
        rangosComIdEndpoint.MapDelete("", RangosHandlers.DeleteRangosAsync);

    }



    public static void RegisterIngredientesEndpoints(this IEndpointRouteBuilder endpointRouteBuilder) 
    {

        var ingredientesEndpoint = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}/ingredientes");

        ingredientesEndpoint.MapGet("", IngredientesHandlers.GetIngredientesAsync);

        ingredientesEndpoint.MapPost("", () => 
        {
            throw new NotImplementedException();
        });

    }
}
