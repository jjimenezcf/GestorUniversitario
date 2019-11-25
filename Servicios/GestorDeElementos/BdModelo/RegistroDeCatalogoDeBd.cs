using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GestorDeElementos.BdModelo
{
    public class RegistroDeCatalogoDeBd: RegistroBase
    {
        public string Catalogo { get; set; }
        public string Esquema { get; set; }
        public string Tabla { get; set; }
    }

    /*
     TABLE_CATALOG	    TABLE_SCHEMA	   TABLE_NAME	            TABLE_TYPE
     SistemaDeElementos	dbo	               __EFMigrationsHistory	BASE TABLE
     SistemaDeElementos	dbo	               Curso	                BASE TABLE
     SistemaDeElementos	dbo	               Estudiante	            BASE TABLE
     SistemaDeElementos	dbo	               Inscripcion	            BASE TABLE 
     */
}
