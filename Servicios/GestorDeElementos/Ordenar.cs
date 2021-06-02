using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Gestor.Errores;
using Newtonsoft.Json;
using ServicioDeDatos.Elemento;

namespace GestorDeElementos
{
    public enum ModoDeOrdenancion { ascendente, descendente }

    public static class OrdenSql
    {
        public static string toSql(this ModoDeOrdenancion modo)
        {
            if (modo == ModoDeOrdenancion.ascendente)
                return "asc";

            return "desc";
        }
    }

    public class ClausulaDeOrdenacion
    {
        public string OrdenarPor { get; set; }
        public ModoDeOrdenancion Modo { get; set; }
        public ClausulaDeOrdenacion()
        {

        }
        public ClausulaDeOrdenacion(string ordenarPor, ModoDeOrdenancion modo)
        {
            OrdenarPor = ordenarPor;
            Modo = modo;
        }
    }

    public static class Ordenar
    {
        private static string AplicarThenBy<TRegistro>(this IQueryable<TRegistro> registros, ModoDeOrdenancion modo, string propiedad)
        {
            switch (modo)
            {
                case ModoDeOrdenancion.ascendente:
                    return $"ThenBy(\"x => x.{propiedad}\")";
                case ModoDeOrdenancion.descendente:
                    return $"ThenBy(\"x => x.{propiedad}\", \"descending\")";
            }
            return "";
        }

        private static IQueryable<TRegistro> AplicarOrderBy<TRegistro>(this IQueryable<TRegistro> registros, ClausulaDeOrdenacion orden, PropertyInfo propiedad)
        {
            switch (orden.Modo)
            {
                case ModoDeOrdenancion.ascendente:
                    return registros.OrderBy($"x => x.{propiedad.Name}");
                case ModoDeOrdenancion.descendente:
                    return registros.OrderBy($"x => x.{propiedad.Name}", "descending");
            }
            return registros;
        }


        public static IQueryable<TRegistro> AplicarOrdenesBasicos<TRegistro>(this IQueryable<TRegistro> registros, List<ClausulaDeOrdenacion> ordenacion) where TRegistro : IRegistro
        {
            if (ordenacion.Count == 0)
                return registros;
            try
            {
                if (ordenacion.Count == 1)
                    return registros.OrderBy($"{ordenacion[0].OrdenarPor} {ordenacion[0].Modo.toSql()}");

                if (ordenacion.Count == 2)
                    return registros.OrderBy($"{ordenacion[0].OrdenarPor} {ordenacion[0].Modo.toSql()}")
                        .ThenBy($"{ordenacion[1].OrdenarPor} {ordenacion[1].Modo.toSql()}");

                if (ordenacion.Count == 3)
                    return registros.OrderBy($"{ordenacion[0].OrdenarPor} {ordenacion[0].Modo.toSql()}")
                        .ThenBy($"{ordenacion[1].OrdenarPor} {ordenacion[1].Modo.toSql()}")
                        .ThenBy($"{ordenacion[2].OrdenarPor} {ordenacion[2].Modo.toSql()}");

                if (ordenacion.Count == 4)
                    return registros.OrderBy($"{ordenacion[0].OrdenarPor} {ordenacion[0].Modo.toSql()}")
                        .ThenBy($"{ordenacion[1].OrdenarPor} {ordenacion[1].Modo.toSql()}")
                        .ThenBy($"{ordenacion[2].OrdenarPor} {ordenacion[2].Modo.toSql()}")
                        .ThenBy($"{ordenacion[3].OrdenarPor} {ordenacion[3].Modo.toSql()}");
            }
            catch (Exception e)
            {
                GestorDeErrores.Emitir($"Una de las propiedades de ordenación está mal definida. {JsonConvert.SerializeObject(ordenacion)}", e);
            }


            return registros;
        }
    }
}
