namespace Gestor.Elementos.ModeloBd
{
    public class RegistroDelCatalogoDelSe: RegistroBase
    {
        public string Catalogo { get; set; }
        public string Esquema { get; set; }
        public string Tabla { get; set; }
    }

    public class ObjetoValor
    {
        public string Valor { get; set; }
    }

    /*
     TABLE_CATALOG	    TABLE_SCHEMA	   TABLE_NAME	            TABLE_TYPE
     SistemaDeElementos	dbo	               __EFMigrationsHistory	BASE TABLE
     SistemaDeElementos	dbo	               Curso	                BASE TABLE
     SistemaDeElementos	dbo	               Estudiante	            BASE TABLE
     SistemaDeElementos	dbo	               Inscripcion	            BASE TABLE 
     */
}
