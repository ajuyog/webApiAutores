using Microsoft.AspNetCore.Mvc.Filters;

namespace webApiAutores.Filtros
{
    public class MiFiltroDeAccion : IActionFilter
    {

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }
    }
}
