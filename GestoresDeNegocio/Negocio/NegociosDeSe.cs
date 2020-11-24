using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestoresDeNegocio.Negocio
{
    public enum enumNegocio
    {
        No_Definido,
        Usuarios,
        Vistas,
        Permisos,
        Menus
    }

    public static class NegociosDeSe
    {
        public static string Parsear(enumNegocio negocio)
        {
            switch (negocio)
            {
                case enumNegocio.Usuarios: return "Usuarios";
            }

            return negocio.ToString();
        }

        public static enumNegocio Parsear(string negocio)
        {
            switch (negocio)
            {
                case "Usuarios": return enumNegocio.Usuarios;
            }

            throw new Exception($"No está definido el negocio {negocio}");
        }
    }
}
