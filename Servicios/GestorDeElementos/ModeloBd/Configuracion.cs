using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Gestor.Elementos.ModeloBd
{

    public class CatalogoDelSe : Registro
    {
        public string Catalogo { get; set; }
        public string Esquema { get; set; }
        public string Tabla { get; set; }
    }



    public class ExisteTabla : ConsultaSql
    {
        public bool Existe => Leidos == 0 ? false : (int)Registros[0][0] == 1;


        public ExisteTabla(ContextoDeElementos contexto, string tabla)
        : base(contexto, $"SELECT 1 FROM sysobjects WHERE type = 'U' AND name = '{tabla}'")
        {
            Ejecutar();
        }
    }



}
