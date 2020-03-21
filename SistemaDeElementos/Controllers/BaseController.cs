using Gestor.Elementos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;


namespace MVCSistemaDeElementos.Controllers
{
    public class BaseController : Controller
    {
        protected GestorDeErrores GestorDeErrores { get; }

        protected DatosDeConexion DatosDeConexion { get; set; }
        
        public BaseController(GestorDeErrores gestorDeErrores)
        {
            GestorDeErrores = gestorDeErrores;
        }

    }
}
