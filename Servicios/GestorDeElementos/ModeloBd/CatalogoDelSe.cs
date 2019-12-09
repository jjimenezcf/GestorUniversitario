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
        public string Version => (string)Registros[0][3];

        public VersionSql(ContextoDeElementos contexto)
            :base(contexto, "Select * from dbo.Var_Variable where NOMBRE like 'Versión'")
        {
            Ejecutar();
        }
    }

    /*
     * CREATE TABLE [dbo].[Var_Variable](
       	[ID] int IDENTITY(1,1) NOT NULL,
       	[NOMBRE] varchar(50) NOT NULL,
       	[DESCRIPCION]  varchar(250) NOT NULL,
       	[VALOR] varchar(250) NULL,
        CONSTRAINT [PK_VARIABLE] PRIMARY KEY CLUSTERED 
       (
       	[ID] ASC
       )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
        CONSTRAINT [AK_VAR_VARIABLE_NOMBRE] UNIQUE NONCLUSTERED 
       (
       	[NOMBRE] ASC
       )WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
       ) ON [PRIMARY]
       GO
       
       insert into dbo.Var_Variable(nombre, descripcion, VALOR) values('Versión','Versión del sistema','0.1')
     * */

}
