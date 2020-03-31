using System;
using System.Collections.Generic;
using Gestor.Elementos.ModeloIu;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.Seguridad;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudMenus : DescriptorDeCrud<MenuDto>
    {
        public CrudMenus(ModoDescriptor modo)
        : base(controlador: "Funcionalidad", vista: "MantenimientoFuncionalidad", titulo: "Mantenimiento de funcionalidad", modo: modo)
        {

        }

    }
}
