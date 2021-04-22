using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gestor.Errores;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Schema.Generation;
using Utilidades;

namespace Enumerados
{
    public class Parametro
    {
        public string parametro { get; set; }
        public object valor { get; set; }
    }

    public static class ParametrosJson
    {
        public static void ValidarJson(string json)
        {
            JSchemaGenerator generator = new JSchemaGenerator();
            JSchema schema = generator.Generate(typeof(List<Parametro>));
            try
            {
                JArray actualJson = JArray.Parse(json);
                bool valid = actualJson.IsValid(schema, out IList<string> errorMessages);

                if (!valid)
                {
                    var mensaje = "";
                    foreach (var me in errorMessages)
                    {
                        mensaje = $"{mensaje}{Environment.NewLine}{me}";
                    }
                    GestorDeErrores.Emitir($"Parámetros Json mal definido.{Environment.NewLine}{json}{Environment.NewLine}{mensaje}");
                }
            }
            catch (Exception exc)
            {
                GestorDeErrores.Emitir($"Json mal definido", exc);
            }
        }

        public static string ToJson(this List<Parametro> p)
        {
            return JsonConvert.SerializeObject(p);
        }

        public static List<Parametro> ToListaDeParametros(this string  json)
        {
            // ValidarJson(json);
            return JsonConvert.DeserializeObject<List<Parametro>>(json);
        }

        public static Dictionary<string, object> ToDiccionarioDeParametros(this string parametrosJson)
        {
            var parametros = new Dictionary<string, object>();
            if (!parametrosJson.IsNullOrEmpty())
            {
                var listaJson = parametrosJson.ToListaDeParametros();
                foreach (var p in listaJson)
                    parametros.Add(p.parametro, p.valor);
            }
            return parametros;
        }

        public static string ToJson(this Dictionary<string, object> dic)
        {
            var parametros = new List<Parametro>();
            foreach(var clave in dic.Keys)
            {
                var p = new Parametro();
                p.parametro = clave;
                p.valor = dic[clave];
                parametros.Add(p);
            }

            return parametros.ToJson();
        }
    }

}
