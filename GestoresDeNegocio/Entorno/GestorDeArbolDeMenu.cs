using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Gestor.Errores;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore;
using ModeloDeDto.Entorno;
using ServicioDeDatos;
using ServicioDeDatos.Entorno;

namespace GestoresDeNegocio.Entorno
{
    public class GestorDeArbolDeMenu : GestorDeElementos<ContextoSe, ArbolDeMenuDtm, ArbolDeMenuDto>
    {
        public class MapearMenus : Profile
        {
            public MapearMenus()
            {
                CreateMap<ArbolDeMenuDtm, ArbolDeMenuDto>();                  
            }
        }

        public GestorDeArbolDeMenu(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        //internal static GestorDeArbolDeMenu Gestor(IMapper mapeador)
        //{
        //    var contexto = ContextoSe.ObtenerContexto();
        //    return new GestorDeArbolDeMenu(contexto, mapeador);
        //}


        protected override void DespuesDeMapearElemento(ArbolDeMenuDtm registro, ArbolDeMenuDto elemento, ParametrosDeMapeo parametros)
        {
            base.DespuesDeMapearElemento(registro, elemento, parametros);
            elemento.Submenus = new List<ArbolDeMenuDto>();
            elemento.VistaMvc = new VistaMvcDto
            {
                Id = registro.IdVistaMvc.GetValueOrDefault(),
                Nombre = registro.Vista,
                Controlador = registro.Controlador,
                Accion = registro.accion,
                Parametros = registro.parametros
            };
        }

        private static ConcurrentDictionary<int, List<ArbolDeMenuDto>> CacheArbolDeMenu = new ConcurrentDictionary<int, List<ArbolDeMenuDto>>();

        public List<ArbolDeMenuDto> LeerArbolDeMenu(string usuario)
        {

            var gestor = GestorDeUsuarios.Gestor(Contexto, Mapeador);
            var filtro = new ClausulaDeFiltrado { Clausula = nameof(UsuarioDtm.Login), Criterio = CriteriosDeFiltrado.igual, Valor = usuario };
            var filtros = new List<ClausulaDeFiltrado> { filtro };
            var r = gestor.LeerRegistros(0, 1, filtros);
            if (r.Count == 0 || r.Count > 1)
                GestorDeErrores.Emitir($"Usuario {usuario} no válido");

            if (!CacheArbolDeMenu.ContainsKey(r[0].Id))
            {
                var arbolDeMenu = Contexto
                .MenuSe
                .FromSqlInterpolated($@"
                                     SELECT T1.ID, 
                                            T1.NOMBRE, 
                                            T1.DESCRIPCION, 
                                            T1.ICONO, 
                                            T1.ACTIVO, 
                                            T1.IDPADRE, 
                                            T1.IDVISTA_MVC, 
                                            T1.ORDEN, 
                                            T2.NOMBRE AS PADRE, 
                                            T3.NOMBRE AS VISTA, 
                                            T3.CONTROLADOR, 
                                            T3.ACCION, 
                                            T3.PARAMETROS
                                     FROM ENTORNO.ARBOL_MENU_POR_USUARIO({r[0].Id}) AS T1
                                     LEFT JOIN ENTORNO.MENU T2 ON T2.ID = T1.IDPADRE
                                     LEFT JOIN ENTORNO.VISTA_MVC T3 ON T3.ID = T1.IDVISTA_MVC
                                     order by t1.IDPADRE, T1.ORDEN, T1.NOMBRE").ToList();
                
                //arbolDeMenu = LeerRegistros(0, -1);
                var resultadoDto = new List<ArbolDeMenuDto>();
                ProcesarSubMenus(resultadoDto, arbolDeMenu, padre: null);
                CacheArbolDeMenu[r[0].Id] = resultadoDto;
            }
            return CacheArbolDeMenu[r[0].Id];
        }


        internal void LimpiarCacheDeArbolDeMenu()
        {
            CacheArbolDeMenu = null;
        }

        private void ProcesarSubMenus(List<ArbolDeMenuDto> resultadoDto, List<ArbolDeMenuDtm> arbolDeMenu, ArbolDeMenuDto padre)
        {
            List<ArbolDeMenuDtm> procesarMenus = MenusParaProcesar(arbolDeMenu, padre);
            if (procesarMenus.Count == 0)
                return;

            foreach (var menuDtm in procesarMenus)
            {
                var menuDto = MapearElemento(menuDtm);
                if (padre != null)
                    padre.Submenus.Add(menuDto);

                resultadoDto.Add(menuDto);
                if (menuDtm.IdVistaMvc == null)
                {
                    ProcesarSubMenus(resultadoDto, arbolDeMenu, padre: menuDto);
                }
            }
        }


        private List<ArbolDeMenuDtm> MenusParaProcesar(List<ArbolDeMenuDtm> arbolDeMenu, ArbolDeMenuDto padre)
        {
            var resultado = new List<ArbolDeMenuDtm>();
            var procesar = new List<ArbolDeMenuDtm>();

            foreach (var nodo in arbolDeMenu)
                if ((nodo.IdPadre == null && padre == null) || (padre != null && nodo.IdPadre == padre.Id))
                    procesar.Add(nodo);

            if (procesar.Count == 0)
                return resultado;

            while (procesar.Count > 0)
            {
                var orden = procesar[0].Orden;
                var quitar = 0;
                for (var i = 0; i < procesar.Count; i++)
                {
                    if (procesar[i].Orden <= orden)
                    {
                        orden = procesar[i].Orden;
                        quitar = i;
                    }
                }
                resultado.Add(procesar[quitar]);
                procesar.RemoveAt(quitar);
            }

            return resultado;
        }

    }
}
