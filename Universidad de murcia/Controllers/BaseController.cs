using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;


namespace UniversidadDeMurcia.Controllers
{
    public class BaseController : Controller
    {

        protected GestorDeErrores GestorDeErrores { get; }

        public BaseController(GestorDeErrores gestorDeErrores)
        {
            GestorDeErrores = gestorDeErrores;
        }
    }
}
