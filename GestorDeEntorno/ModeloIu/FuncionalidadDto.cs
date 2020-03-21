using Gestor.Elementos.ModeloIu;
using System;
using System.Collections.Generic;

namespace Gestor.Elementos.Entorno
{
    public class FuncionalidadDto: Elemento
    {
        public string Nombre { get; set; }
        public List<FuncionalidadDto> Opciones { get; set; }
        public AccionDto Accion { get; set; }
    }
}
