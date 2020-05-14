﻿using Gestor.Elementos.ModeloIu;
using System;
using System.Collections.Generic;

namespace Gestor.Elementos.Entorno
{
    public class ArbolDeMenuDto : Elemento
    {
        public string Padre { get; set; }

        public string Nombre { get; set; }

        public string Descripcion { get; set; }

        public string Icono { get; set; }

        public int Orden { get; set; }

        public bool Activo { get; set; }

        public int? IdPadre { get; set; }

        public List<ArbolDeMenuDto> Submenus { get; set; }

        public VistaMvcDto VistaMvc { get; set; }

        public int? IdVistaMvc { get; set; }

    }
}
