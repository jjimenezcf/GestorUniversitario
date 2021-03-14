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


        /*
         SELECT distinct T1.ID, T1.NOMBRE, T1.DESCRIPCION, T1.ICONO, T1.ACTIVO, T1.IDPADRE,  T1.IDVISTA_MVC, T1.ORDEN, t4.NOMBRE as permiso, t5.NOMBRE as tipo, t6.NOMBRE as clase
         from ENTORNO.MENU t1
         inner join ENTORNO.VISTA_MVC t2 on t2.id = t1.IDVISTA_MVC
         inner join ENTORNO.USU_PERMISO t3 on t3.IDUSUA = 11 and t2.IDPERMISO = t3.IDPERMISO
         inner join SEGURIDAD.PERMISO t4 on t4.ID = t3.IDPERMISO
         inner join SEGURIDAD.TIPO_PERMISO t5 on t5.ID = t4.IDTIPO
         inner join SEGURIDAD.CLASE_PERMISO t6 on t6.ID = t4.IDCLASE
         where t1.IDVISTA_MVC is not null
           and t1.ACTIVO = 1
         */


        public List<ArbolDeMenuDto> LeerArbolDeMenu(string usuario)
        {

            var gestor = GestorDeUsuarios.Gestor(Contexto, Mapeador);
            var usuarioDtm = gestor.LeerRegistro(nameof(UsuarioDtm.Login),usuario,false,false,false, false);
            if (usuarioDtm == null)
                GestorDeErrores.Emitir($"Usuario {usuario} no válido");

            var  CacheArbolDeMenu = ServicioDeCaches.Obtener(nameof(this.LeerArbolDeMenu));

            if (!CacheArbolDeMenu.ContainsKey(usuarioDtm.Id.ToString()))
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
                                     FROM ENTORNO.ARBOL_MENU_POR_USUARIO({usuarioDtm.Id}) AS T1
                                     LEFT JOIN ENTORNO.MENU T2 ON T2.ID = T1.IDPADRE
                                     LEFT JOIN ENTORNO.VISTA_MVC T3 ON T3.ID = T1.IDVISTA_MVC
                                     order by t1.IDPADRE, T1.ORDEN, T1.NOMBRE").ToList();
                
                //arbolDeMenu = LeerRegistros(0, -1);
                var resultadoDto = new List<ArbolDeMenuDto>();
                ProcesarSubMenus(resultadoDto, arbolDeMenu, padre: null);
                CacheArbolDeMenu[usuarioDtm.Id.ToString()] = resultadoDto;
            }
            return (List<ArbolDeMenuDto>)CacheArbolDeMenu[usuarioDtm.Id.ToString()];
        }


        internal void LimpiarCacheDeArbolDeMenu()
        {
            ServicioDeCaches.EliminarCache(nameof(this.LeerArbolDeMenu));
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
