using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.Extensions.DependencyInjection;

namespace Microsoft.AspNetCore.Mvc
{
    /// <summary>
    /// Controller extensions
    /// </summary>
    public static class ExtensionesDeControlador
    {
        public static bool ExisteLaVista(this ControllerBase controller, string name)
        {
            var services = controller.HttpContext.RequestServices;
            var viewEngine = services.GetRequiredService<ICompositeViewEngine>();
            var result = viewEngine.GetView(null, $"{name.Replace("..", "Views")}.cshtml", true);
            //if (!result.Success)
            //    result = viewEngine.FindView(controller.ControllerContext, name, true);
            return result.Success;
        }
    }
}
