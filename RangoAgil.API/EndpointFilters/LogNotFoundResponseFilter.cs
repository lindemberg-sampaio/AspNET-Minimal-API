using System.Net;

namespace RangoAgil.API.EndpointFilters;

public class LogNotFoundResponseFilter(ILogger<LogNotFoundResponseFilter> logger) : IEndpointFilter
{
    
    public readonly ILogger<LogNotFoundResponseFilter> _logger = logger;

    public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
    {
        var result = await next(context);
        var actualResults = (result is INestedHttpResult resultNested1) ? resultNested1.Result : (IResult)result;

        if (actualResults is IStatusCodeHttpResult { StatusCode: (int)HttpStatusCode.NotFound })
        {

            _logger.LogInformation($"Recurso {context.HttpContext.Request.Path} não foi encontrado!");

        }

        return result;
    }
}