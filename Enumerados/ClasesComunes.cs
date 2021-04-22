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

namespace Enumerados
{

    public class ParametrosJson
    {
        public List<Parametro> Parametros { get; private set; }
        public ParametrosJson(string json)
        {
            try
            {
                // ValidarJson(json);
                Parametros = JsonConvert.DeserializeObject<List<Parametro>>(json);
            }
            catch (Exception e)
            {
                GestorDeErrores.Emitir("Paremetro json mal definido", e);
            }
        }

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
    }

    public class Parametro
    {
        public string parametro { get; set; }
        public object valor { get; set; }
    }
}
