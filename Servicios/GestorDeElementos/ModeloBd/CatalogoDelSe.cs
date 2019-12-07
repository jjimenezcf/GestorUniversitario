using System.Collections.Generic;

namespace Gestor.Elementos.ModeloBd
{
    public class CatalogoDelSe : RegistroBase
    {
        public string Catalogo { get; set; }
        public string Esquema { get; set; }
        public string Tabla { get; set; }
    }

    public class VersionSql: ConsultaSql
    {
        public string Version => (string)Registros[0][2];

        public VersionSql(ContextoDeElementos contexto)
            :base(contexto, "Select * from dbo.Var_Variable where variable like 'Version'")
        {
            Ejecutar();
        }
    }
}
