using Microsoft.EntityFrameworkCore;

namespace webApiAutores.Utilidades
{
    public static class HttpContextExtensions
    {
        public async static Task InsertarParametrosDePaginacionEnCabecera<T>(this HttpContext httpContext, 
            IQueryable<T> queryable)
        {
            if (httpContext == null) { throw new ArgumentNullException(nameof(httpContext)); }

            double cantidad = await queryable.CountAsync();

            httpContext.Response.Headers.Add("CantidadTotalRegistros", cantidad.ToString());
        }
    }
}
