using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;


namespace UniversidadDeMurcia.Controllers
{
    public class BaseController : Controller
    {

        protected Errores GestorErrores { get; }

        public BaseController(Errores gestorErrores)
        {
            GestorErrores = gestorErrores;
        }
    }
}
