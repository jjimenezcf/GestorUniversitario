using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Controllers;
using ServicioDeDatos;

namespace SistemaDeElementos.Controllers.Seguridad
{
    public class AccesoController : HomeController
    {
        public AccesoController(ContextoSe contexto, GestorDeErrores gestorDeErrores) : base(contexto, gestorDeErrores)
        {
        }

        public IActionResult Login(string email, string password)
        {
            return PanelDeControl();
        }
    }
}
