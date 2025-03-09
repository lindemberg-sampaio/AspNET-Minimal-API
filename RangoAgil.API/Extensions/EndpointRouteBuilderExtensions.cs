using RangoAgil.API.EndpointFilters;
using RangoAgil.API.EndpointManipulador;

namespace RangoAgil.API.Extensions;

public static class EndpointRouteBuilderExtensions
{

    public static void RegisterRangosEndpoints(this IEndpointRouteBuilder endpointRouteBuilder)
    {

        var rangosEndpoint = endpointRouteBuilder.MapGroup("/rangos");
        var rangosComIdEndpoint = rangosEndpoint.MapGroup("/{rangoId:int}");

        var rangosComIdAndLockedFilterEndpoint = endpointRouteBuilder.MapGroup("/rangos/{rangoId:int}")
            .AddEndpointFilter(new RangoIsLockedFilter(11))
            .AddEndpointFilter(new RangoIsLockedFilter(5));

        rangosEndpoint.MapGet("", RangosHandlers.GetRangosAsync);
        rangosComIdEndpoint.MapGet("", RangosHandlers.GetRangosComIdAsync).WithName("GetRangos");
        rangosEndpoint.MapPost("", RangosHandlers.CreateRangosAsync)
            .AddEndpointFilter<ValidateAnnotationFilter>();

        rangosComIdAndLockedFilterEndpoint.MapPut("", RangosHandlers.UpdateRangosAsync);
        rangosComIdAndLockedFilterEndpoint.MapDelete("", RangosHandlers.DeleteRangosAsync)
            .AddEndpointFilter<LogNotFoundResponseFilter>();

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
