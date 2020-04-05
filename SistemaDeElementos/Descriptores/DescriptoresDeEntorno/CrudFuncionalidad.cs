using Gestor.Elementos.Entorno;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudMenus : DescriptorDeCrud<MenuDto>
    {
        public CrudMenus(ModoDescriptor modo)
        : base(controlador: "Funcionalidad", vista: "MantenimientoFuncionalidad", elemento: "Menu", modo: modo)
        {

        }

    }
}
