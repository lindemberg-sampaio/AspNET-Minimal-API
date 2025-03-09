namespace RangoAgil.API.EndpointFilters
{
    public class RangoIsLockedFilter : IEndpointFilter
    {
        public readonly int _lockedRangoId;

        public RangoIsLockedFilter(int lockedRangoId)
        {
            _lockedRangoId = lockedRangoId;
        }

        public async ValueTask<object?> InvokeAsync(EndpointFilterInvocationContext context
                                                  , EndpointFilterDelegate next)
        {
            int rangoId;

            if(context.HttpContext.Request.Method == "PUT")
            {
                rangoId = context.GetArgument<int>(2);
            }
            else if(context.HttpContext.Request.Method == "DELETE")
            {
                rangoId = context.GetArgument<int>(1);
            }
            else
            {
                throw new NotSupportedException("Este filtro não é suportado neste cenário!");
            }


            if (rangoId == _lockedRangoId)
            {
                return TypedResults.Problem(new()
                {
                    Status = 400,
                    Title = "Não mexa aí, desavisado!",
                    Detail = "Esse rango não pode ser alterarado ou excluído."
                });
            }

            var result = await next.Invoke(context);

            return result;            
        }
    }
}
