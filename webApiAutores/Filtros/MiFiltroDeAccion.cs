﻿using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace webApiAutores.Filtros
{
    public class MiFiltroDeAccion : IActionFilter
    {
        private readonly ILogger<MiFiltroDeAccion> logger;

        public MiFiltroDeAccion(ILogger<MiFiltroDeAccion> logger)
        {
            this.logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            logger.LogInformation("Antes de ejecutar acción");
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            logger.LogInformation("Despues de ejecutar acción");
        }
    }
}
