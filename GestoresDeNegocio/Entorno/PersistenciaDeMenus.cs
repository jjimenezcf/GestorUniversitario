using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto;
using ServicioDeDatos.Entorno;

namespace GestoresDeNegocio.Entorno
{
    public static class PersistenciaDeMenus
    {
        public static void PersistirMenus(GestorDeMenus gestor)
        {
            gestor.Contexto.IniciarTraza(nameof(PersistirMenus));
            try
            {
                gestor.Contexto.DatosDeConexion.CreandoModelo = true;
                CrearMenuSiNoExiste(gestor, nombre: "Municipios", descripcion: "Mantenimiento de Municipios", icono: "municipio2.svg", padre: "Maestros.Callejero", vista: "Municipios", orden: 30);
            }
            finally
            {
                gestor.Contexto.DatosDeConexion.CreandoModelo = false;
            }
        }

        private static void CrearMenuSiNoExiste(GestorDeMenus gestorDeMenu, string nombre, string descripcion, string icono, string padre, string vista, int orden)
        {
            var menus = padre.Split(".");
            int idPadre = 0;
            foreach (var menu in menus)
            {
                var padresDtm = BuscarMenu(gestorDeMenu, menu, idPadre);
                if (padresDtm.Count == 0)
                    throw new Exception($"No está definido el padre {menu}, para el idPadre {idPadre}");

                idPadre = padresDtm[0].Id;
            }

            List<MenuDtm> menusDtm = BuscarMenu(gestorDeMenu, nombre, idPadre);

            if (menusDtm.Count == 0)
            {
                var menuDtm = new MenuDtm();
                menuDtm.Nombre = nombre;
                menuDtm.Descripcion = descripcion;
                menuDtm.Icono = icono;
                menuDtm.Orden = orden;
                menuDtm.Activo = true;


                menuDtm.IdPadre = idPadre;

                var gestorDeVista = GestorDeVistaMvc.Gestor(gestorDeMenu.Contexto, gestorDeMenu.Contexto.Mapeador);
                var vistaDtm = gestorDeVista.LeerRegistro(nameof(VistaMvcDtm.Nombre), vista, true, true, false, false);
                menuDtm.IdVistaMvc = vistaDtm.Id;

                gestorDeMenu.PersistirRegistro(menuDtm, new ParametrosDeNegocio(enumTipoOperacion.Insertar));
            }
        }

        private static List<MenuDtm> BuscarMenu(GestorDeMenus gestorDeMenu, string nombre, int idPadre)
        {
            var filtros = new List<ClausulaDeFiltrado>();
            var filtro1 = new ClausulaDeFiltrado(nameof(MenuDtm.Nombre), CriteriosDeFiltrado.igual, nombre);
            var filtro2 = new ClausulaDeFiltrado(nameof(MenuDtm.IdPadre), CriteriosDeFiltrado.esNulo);
            var filtro3 = new ClausulaDeFiltrado(nameof(MenuDtm.IdPadre), CriteriosDeFiltrado.igual, idPadre.ToString());
            filtros.Add(filtro1);
            filtros.Add(idPadre == 0 ? filtro2 : filtro3);
            List<MenuDtm> menusDtm = gestorDeMenu.LeerRegistros(0, -1, filtros);
            return menusDtm;
        }
    }
}
