using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace webApiAutores.Utilidades
{
    public class SwaggerAgrupaPorVersion : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var nameSpaceControlador = controller.ControllerType.Namespace;

            var versionApi = nameSpaceControlador.Split('.').Last().ToLower();

            controller.ApiExplorer.GroupName = versionApi;
        }
    }
}
