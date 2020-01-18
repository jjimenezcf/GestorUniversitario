using System.ComponentModel.DataAnnotations;

namespace Gestor.Elementos.ModeloBd
{

    class Literal
    {
        internal static readonly string  DebugarSqls = nameof(DebugarSqls);
        internal static readonly string version = "Versión";
        internal static readonly string usuario = "jjimenezcf@gmail.com";
        internal static readonly string esquemaBd = "dbo";
        
        public class Tabla
        {
            internal static string Variable = "Var_Variable";
        }
        public class Vista
        {
            internal static string Catalogo = "CatalogoDelSe";
        }
    }

    public class CatalogoDelSe : RegistroBase
    {
        public string Catalogo { get; set; }
        public string Esquema { get; set; }
        public string Tabla { get; set; }
    }

    public class RegistroDeVariable : RegistroBase
    {
        [Required]
        [MaxLength(50)]
        public string Nombre { get; set; }
        
        [MaxLength(250)]
        public string Descri { get; set; }

        [Required]
        [MaxLength(50)]
        public string Valor { get; set; }
    }

    public class VersionSql: ConsultaSql
    {
        public string Version => (string)Registros[0][3];

        public VersionSql(ContextoDeElementos contexto)
            :base(contexto, $"Select * from {Literal.esquemaBd}.{Literal.Tabla.Variable} where NOMBRE like '{Literal.version}'")
        {
            Ejecutar();
        }
    }


    public class ExisteTabla : ConsultaSql
    {
        public bool Existe => (int)Registros[0][0]==1;

        public ExisteTabla(ContextoDeElementos contexto, string tabla)
        : base(contexto, $"SELECT 1 FROM sysobjects WHERE type = 'U' AND name = '{tabla}'")
        {
            Ejecutar();
        }
    }


    public class DebugarSql : ConsultaSql
    {
        public bool DebugarSqls => (Registros.Count == 1 ? Registros[0][3].ToString() == "S": false);

        public DebugarSql(ContextoDeElementos contexto)
        : base(contexto, $"Select * from {Literal.esquemaBd}.{Literal.Tabla.Variable} where NOMBRE like '{Literal.DebugarSqls}'")
        {
            Ejecutar();
        }
    }

}
