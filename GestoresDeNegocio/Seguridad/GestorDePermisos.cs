using AutoMapper;
using System.Linq;
using System.Collections.Generic;
using Utilidades;
using Microsoft.EntityFrameworkCore;
using System;
using Gestor.Errores;
using ServicioDeDatos.Seguridad;
using ServicioDeDatos;
using ModeloDeDto.Seguridad;
using GestorDeElementos;
using Microsoft.EntityFrameworkCore.Internal;

namespace GestoresDeNegocio.Seguridad
{
    public class GestorDePermisos : GestorDeElementos<ContextoSe, PermisoDtm, PermisoDto>
    {
        public class MapearPermiso : Profile
        {
            public MapearPermiso()
            {
                CreateMap<PermisoDtm, PermisoDto>()
                .ForMember(dto => dto.Clase, dtm => dtm.MapFrom(dtm => dtm.Clase.Nombre))
                .ForMember(dto => dto.Tipo, dtm => dtm.MapFrom(dtm => dtm.Tipo.Nombre));

                CreateMap<PermisoDto, PermisoDtm>();

                CreateMap<ClasePermisoDtm, ClasePermisoDto>();

            }
        }

        public GestorDePermisos(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        internal static GestorDePermisos Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePermisos(contexto, mapeador);
        }


        internal static List<PermisoDto> Leer(GestorDePermisos gestor, int posicion, int cantidad, string filtro)
        {
            var filtros = new List<ClausulaDeFiltrado>();
            if (!filtro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(PermisoDto.Nombre), Valor = filtro });

            var permisosDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(permisosDtm).ToList();
        }

        public static PermisoDtm CrearObtener(ContextoSe contexto, IMapper mapeador, string nombre, enumClaseDePermiso clase, enumTipoDePermiso tipo)
        {
            var nombreDelPermiso = ComponerNombre(nombre, clase, tipo);
            var gestorDePermiso = Gestor(contexto, mapeador);
            var permiso = gestorDePermiso.LeerRegistroCacheado(nameof(PermisoDtm.Nombre), nombreDelPermiso, false, false);
            if (permiso == null)
                permiso = Crear(gestorDePermiso, nombreDelPermiso, clase, tipo);
            return permiso;
        }

        public static void Modificar(ContextoSe contexto, IMapper mapeador, int idPermiso, string nombre, enumClaseDePermiso clase, enumTipoDePermiso tipo)
        {
            var gestorDePermiso = Gestor(contexto, mapeador);
            var permiso = gestorDePermiso.LeerRegistroPorId(idPermiso);
            if (permiso != null)
            {
                permiso.Nombre = ComponerNombre(nombre, clase, tipo);
                gestorDePermiso.Modificar(permiso);
            }
        }
        public static void Eliminar(ContextoSe contexto, IMapper mapeador, int idPermiso)
        {
            var gestorDePermiso = Gestor(contexto, mapeador);
            var permiso = gestorDePermiso.LeerRegistroPorId(idPermiso);
            if (permiso != null)
                gestorDePermiso.Eliminar(permiso);
        }

        private static string ComponerNombre(string nombre, enumClaseDePermiso clase, enumTipoDePermiso tipo)
        {
            if (tipo.Equals(enumTipoDePermiso.Acceso))
                return $"{clase.ToString().ToUpper()}: {nombre}";
            else
                return $"{clase.ToString().ToUpper()} ({tipo}): {nombre}";
        }

        private static PermisoDtm Crear(GestorDePermisos gestorDePermiso, string nombreDelPermiso, enumClaseDePermiso clase, enumTipoDePermiso tipo)
        {
            PermisoDtm permiso;
            var gestorDeClase = GestorDeClaseDePermisos.Gestor(gestorDePermiso.Contexto, gestorDePermiso.Mapeador);
            var claseDePermiso = gestorDeClase.LeerRegistroCacheado(nameof(ClasePermisoDtm.Nombre), enumClaseDePermiso.Vista.ToString(), false, false);
            if (claseDePermiso == null)
                claseDePermiso = gestorDeClase.Crear(clase);


            var gestorDeTipo = GestorDeTipoPermiso.Gestor(gestorDePermiso.Contexto, gestorDePermiso.Mapeador);
            var tipoDePermiso = gestorDeTipo.LeerRegistroCacheado(nameof(TipoPermisoDtm.Nombre), enumTipoDePermiso.Acceso.ToString(), false, false);
            if (tipoDePermiso == null)
                tipoDePermiso = gestorDeTipo.Crear(tipo);

            permiso = gestorDePermiso.Crear(nombreDelPermiso, tipoDePermiso, claseDePermiso);
            return permiso;
        }

        protected override IQueryable<PermisoDtm> AplicarFiltros(IQueryable<PermisoDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (hayFiltroPorId)
                return registros;

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == PermisoPor.PermisosDeUnUsuario)
                {
                    var listaIds = filtro.Valor.ListaEnteros();
                    foreach (int id in listaIds)
                        registros = registros.Where(p => p.Usuarios.Any(up => up.IdUsuario == id && up.IdPermiso == p.Id));
                }

                if (filtro.Clausula.ToLower() == PermisoPor.PermisoDeUnRol)
                {
                    var listaIds = filtro.Valor.ListaEnteros();
                    foreach (int id in listaIds)
                        registros = registros.Where(x => x.Roles.Any(i => i.IdPermiso == id));
                }

                if (filtro.Clausula.ToLower() == nameof(PermisosDeUnRolDtm.IdRol).ToLower() &&
                    filtro.Criterio == CriteriosDeFiltrado.diferente)
                    registros = registros.Where(i => !i.Roles.Any(r => r.IdRol == filtro.Valor.Entero()));

                if (filtro.Clausula.ToLower() == nameof(PermisoDtm.Clase).ToLower())
                    registros = registros.Where(x => x.IdClase == filtro.Valor.Entero());

                if (filtro.Clausula.ToLower() == nameof(PermisoDtm.Tipo).ToLower())
                    registros = registros.Where(x => x.IdTipo == filtro.Valor.Entero());
            }

            return registros;

        }

        protected override IQueryable<PermisoDtm> AplicarOrden(IQueryable<PermisoDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            registros = base.AplicarOrden(registros, ordenacion);

            if (ordenacion.Count == 0)
                return registros.OrderBy(x => x.Nombre);

            foreach (var orden in ordenacion)
            {
                if (orden.Propiedad == nameof(PermisoDtm.Nombre).ToLower())
                    registros = orden.Modo == ModoDeOrdenancion.ascendente
                        ? registros.OrderBy(x => x.Nombre)
                        : registros.OrderByDescending(x => x.Nombre);

                if (orden.Propiedad == nameof(PermisoDtm.Clase).ToLower())
                    registros = orden.Modo == ModoDeOrdenancion.ascendente
                        ? registros.OrderBy(x => x.Clase)
                        : registros.OrderByDescending(x => x.Clase);

                if (orden.Propiedad == nameof(PermisoDtm.Tipo).ToLower())
                    registros = orden.Modo == ModoDeOrdenancion.ascendente
                        ? registros.OrderBy(x => x.Tipo)
                        : registros.OrderByDescending(x => x.Tipo);
            }

            return registros;
        }

        protected override void DefinirJoins(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            base.DefinirJoins(filtros, joins, parametros);

            joins.Add(new ClausulaDeJoin { Dtm = typeof(ClasePermisoDtm) });
            joins.Add(new ClausulaDeJoin { Dtm = typeof(TipoPermisoDtm) });
        }

        protected override IQueryable<PermisoDtm> AplicarJoins(IQueryable<PermisoDtm> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, joins, parametros);

            foreach (ClausulaDeJoin join in joins)
            {
                if (join.Dtm == typeof(ClasePermisoDtm))
                    registros = registros.Include(p => p.Clase);

                if (join.Dtm == typeof(TipoPermisoDtm))
                    registros = registros.Include(p => p.Tipo);
            }

            return registros;
        }

        public List<ClasePermisoDto> LeerClases()
        {
            return LeerClases(0, -1, "");
        }

        public List<ClasePermisoDto> LeerClases(int posicion, int cantidad, string valorDeFiltro)
        {
            var gestor = GestorDeClaseDePermisos.Gestor(Contexto, Mapeador);
            var filtros = new List<ClausulaDeFiltrado>();
            if (!valorDeFiltro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(ClasePermisoDtm.Nombre), Valor = valorDeFiltro });

            var clasesDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(clasesDtm).ToList();
        }

        public List<TipoPermisoDto> LeerTipos()
        {
            return LeerTipos(0, -1, "");
        }

        public List<TipoPermisoDto> LeerTipos(int posicion, int cantidad, string valorDeFiltro)
        {
            var gestor = GestorDeTipoPermiso.Gestor(Contexto, Mapeador);
            var filtros = new List<ClausulaDeFiltrado>();
            if (!valorDeFiltro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(TipoPermisoDtm.Nombre), Valor = valorDeFiltro });

            var tiposDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(tiposDtm).ToList();
        }

        protected override void AntesMapearRegistroParaEliminar(PermisoDto elemento, ParametrosDeNegocio opciones)
        {
            base.AntesMapearRegistroParaEliminar(elemento, opciones);

            var gestor = GestorDePermisosDeUnRol.Gestor(Contexto, Mapeador);
            var filtro = new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnRolDtm.IdPermiso), Criterio = CriteriosDeFiltrado.igual, Valor = elemento.Id.ToString() };
            var filtros = new List<ClausulaDeFiltrado> { filtro };
            var r = gestor.LeerRegistros(0, 1, filtros);
            if (r.Count > 0)
            {
                var roles = "";
                foreach (var r1 in r)
                    roles = $"{(roles == "" ? "" : $"{roles},")} {r1.Rol.Nombre}";

                GestorDeErrores.Emitir($"El permiso está incluido en {(r.Count == 1 ? "el rol" : "los roles") }: '{roles}'");
            }
        }

        private PermisoDtm Crear(string nombrePermiso, TipoPermisoDtm tipoDePermiso, ClasePermisoDtm claseDePermiso)
        {
            var registro = new PermisoDtm();
            registro.Nombre = nombrePermiso;
            registro.IdClase = claseDePermiso.Id;
            registro.IdTipo = tipoDePermiso.Id;
            PersistirRegistro(registro, new ParametrosDeNegocio(TipoOperacion.Insertar));
            return registro;
        }

        private PermisoDtm Modificar(PermisoDtm permiso)
        {
            PersistirRegistro(permiso, new ParametrosDeNegocio(TipoOperacion.Modificar));
            return permiso;
        }

        private PermisoDtm Eliminar(PermisoDtm permiso)
        {
            PersistirRegistro(permiso, new ParametrosDeNegocio(TipoOperacion.Eliminar));
            return permiso;
        }

        protected override void AntesDePersistir(PermisoDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);

            //validamos que el permiso no esté en un rol
            if (parametros.Tipo == TipoOperacion.Eliminar)
            {
                var gestor = new GestorDePermisosDeUnRol(Contexto, Mapeador);
                var filtro = new ClausulaDeFiltrado { Clausula = nameof(PermisosDeUnRolDtm.IdPermiso), Criterio = CriteriosDeFiltrado.igual, Valor = registro.Id.ToString() };
                var filtros = new List<ClausulaDeFiltrado> { filtro };
                if (gestor.Contar(filtros) > 0)
                {
                    throw new Exception($"El permiso {registro.Nombre} esta incluido en algún rol, desasígnelo primero");
                }
            }
        }

    }

}
