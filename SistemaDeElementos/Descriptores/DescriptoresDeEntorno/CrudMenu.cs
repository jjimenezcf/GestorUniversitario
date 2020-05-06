using Gestor.Elementos.Entorno;
using MVCSistemaDeElementos.Controllers;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudMenus : DescriptorDeCrud<MenuDto>
    {
        public CrudMenus(ModoDescriptor modo)
        : base(controlador: nameof(MenusController), vista: nameof(MenusController.CrudMenu), modo: modo)
        {

        }

    }
}
