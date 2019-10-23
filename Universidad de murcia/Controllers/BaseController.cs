using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gestor.Errores;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
