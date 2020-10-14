using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Utilidades;
using ServicioDeDatos.Entorno;
using ServicioDeDatos;
using ServicioDeDatos.Archivos;
using ModeloDeDto.Entorno;
using GestorDeElementos;
using Gestor.Errores;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using System.Threading;

namespace GestoresDeNegocio.Entorno
{

    public static partial class Joins
    {
        public static IQueryable<T> AplicarJoinDeArchivo<T>(this IQueryable<T> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros) where T : UsuarioDtm
        {
            foreach (ClausulaDeJoin join in joins)
            {
                if (join.Dtm == typeof(ArchivoDtm))
                    registros = registros.Include(p => p.Archivo);
            }

            return registros;
        }
    }

    static class OrdenacioDeUsuarios
    {
        public static IQueryable<UsuarioDtm> Orden(this IQueryable<UsuarioDtm> set, List<ClausulaDeOrdenacion> ordenacion)
        {
            if (ordenacion.Count == 0)
                return set.OrderBy(x => x.Apellido);

            foreach (var orden in ordenacion)
            {
                if (orden.Propiedad == nameof(UsuarioDtm.Apellido).ToLower())
                    return orden.Modo == ModoDeOrdenancion.ascendente
                        ? set.OrderBy(x => x.Apellido)
                        : set.OrderByDescending(x => x.Apellido);

                if (orden.Propiedad == nameof(UsuarioDtm.Login).ToLower())
                    return orden.Modo == ModoDeOrdenancion.ascendente
                        ? set.OrderBy(x => x.Login)
                        : set.OrderByDescending(x => x.Login);

                if (orden.Propiedad == nameof(UsuarioDtm.Alta).ToLower())
                    return orden.Modo == ModoDeOrdenancion.ascendente
                        ? set.OrderBy(x => x.Alta)
                        : set.OrderByDescending(x => x.Alta);
            }

            return set;
        }
    }

    public class GestorDeUsuarios : GestorDeElementos<ContextoSe, UsuarioDtm, UsuarioDto>, IUserStore<UsuarioDtm>
    {

        public class MapearUsuario : Profile
        {
            public MapearUsuario()
            {
                CreateMap<UsuarioDtm, UsuarioDto>();
                CreateMap<UsuarioDto, UsuarioDtm>();
            }
        }

        public GestorDeUsuarios(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {

        }

        public static GestorDeUsuarios Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeUsuarios(contexto, mapeador);
        }

        protected override void DefinirJoins(List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            base.DefinirJoins(filtros, joins, parametros);
            joins.Add(new ClausulaDeJoin { Dtm = typeof(ArchivoDtm) });
        }

        protected override IQueryable<UsuarioDtm> AplicarJoins(IQueryable<UsuarioDtm> registros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, joins, parametros);
            return Joins.AplicarJoinDeArchivo(registros, joins, parametros);
        }

        protected override IQueryable<UsuarioDtm> AplicarOrden(IQueryable<UsuarioDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            registros = base.AplicarOrden(registros, ordenacion);
            return registros.Orden(ordenacion);
        }

        protected override IQueryable<UsuarioDtm> AplicarFiltros(IQueryable<UsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (!hayFiltroPorId)
                registros = FiltrarUsuarios(registros, filtros);

            return registros;
        }

        private IQueryable<UsuarioDtm> FiltrarUsuarios(IQueryable<UsuarioDtm> registros, List<ClausulaDeFiltrado> filtros)
        {
            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                if (filtro.Clausula.ToLower() == UsuariosPor.NombreCompleto)
                {
                    var partesDelNombre = filtro.Valor.Split('(', ')', ',');
                    if (partesDelNombre.Length == 4)
                        registros = registros.Where(x => x.Login == partesDelNombre[1].Trim()
                                                      && x.Apellido == partesDelNombre[2].Trim()
                                                      && x.Nombre == partesDelNombre[3].Trim());
                    else
                        registros = registros.Where(x => x.Apellido.Contains(filtro.Valor)
                                                      || x.Nombre.Contains(filtro.Valor)
                                                      || x.Login.Contains(filtro.Valor));
                }
                if (filtro.Clausula.ToLower() == UsuariosPor.Permisos)
                {
                    var listaIds = filtro.Valor.ListaEnteros();
                    foreach (int id in listaIds)
                    {
                        registros = registros.Where(u => u.Permisos.Any(up => up.IdPermiso == id && up.IdUsua == u.Id));
                    }
                }
            }

            return registros;
        }

        protected override void AntesMapearRegistroParaInsertar(UsuarioDto usuarioDto, ParametrosDeNegocio opciones)
        {
            base.AntesMapearRegistroParaInsertar(usuarioDto, opciones);
            usuarioDto.Alta = System.DateTime.Now;
            ValidarDatos(usuarioDto);
        }

        protected override void AntesMapearRegistroParaModificar(UsuarioDto usuarioDto, ParametrosDeNegocio opciones)
        {
            base.AntesMapearRegistroParaModificar(usuarioDto, opciones);
            ValidarDatos(usuarioDto);
        }

        protected override void AntesDePersistir(UsuarioDtm registro, ParametrosDeNegocio parametros)
        {
            base.AntesDePersistir(registro, parametros);

            if (registro.IdArchivo == 0 || registro.IdArchivo == null)
            {
                registro.IdArchivo = null;
                registro.Archivo = null;
            }
        }


        private void ValidarDatos(UsuarioDto usuarioDto)
        {
            if (usuarioDto.Login.IsNullOrEmpty())
                GestorDeErrores.Emitir("Es necesario indicar el login del usuario");
            if (usuarioDto.Apellido.IsNullOrEmpty())
                GestorDeErrores.Emitir("Es necesario indicar el apellido del usuario");
            if (usuarioDto.Nombre.IsNullOrEmpty())
                GestorDeErrores.Emitir("Es necesario indicar el nombre del usuario");
        }

        protected override void DespuesDeMapearElemento(UsuarioDtm registro, UsuarioDto elemento, ParametrosDeMapeo parametros)
        {
            base.DespuesDeMapearElemento(registro, elemento, parametros);
            if (registro.Archivo != null)
            {
                elemento.Foto = GestorDeElementos.Utilidades.DescargarArchivo(registro.Archivo.Id, registro.Archivo.Nombre, registro.Archivo.AlmacenadoEn);
            }
        }

        public Task<string> GetUserIdAsync(UsuarioDtm user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetUserNameAsync(UsuarioDtm user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetUserNameAsync(UsuarioDtm user, string userName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetNormalizedUserNameAsync(UsuarioDtm user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task SetNormalizedUserNameAsync(UsuarioDtm user, string normalizedName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> CreateAsync(UsuarioDtm user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> UpdateAsync(UsuarioDtm user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<IdentityResult> DeleteAsync(UsuarioDtm user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<UsuarioDtm> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<UsuarioDtm> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }


}
