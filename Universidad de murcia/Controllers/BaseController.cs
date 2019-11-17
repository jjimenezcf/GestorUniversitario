using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;


namespace UniversidadDeMurcia.Controllers
{
    public class BaseController : Controller
    {

        protected Errores GestorDeErrores { get; }

        public BaseController(Errores gestorDeErrores)
        {
            GestorDeErrores = gestorDeErrores;
        }
    }
}
