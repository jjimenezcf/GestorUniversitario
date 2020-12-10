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
using System;
using ServicioDeDatos.Elemento;
using ServicioDeDatos.Seguridad;
using GestoresDeNegocio.Negocio;

namespace GestoresDeNegocio.Entorno
{

    public class GestorDeUsuarios : GestorDeElementos<ContextoSe, UsuarioDtm, UsuarioDto>
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

        internal static List<UsuarioDto> Leer(GestorDeUsuarios gestor, int posicion, int cantidad, string filtro)
        {
            var filtros = new List<ClausulaDeFiltrado>();
            if (!filtro.IsNullOrEmpty())
                filtros.Add(new ClausulaDeFiltrado { Criterio = CriteriosDeFiltrado.contiene, Clausula = nameof(UsuarioDto.Nombre), Valor = filtro });

            var usuariosDtm = gestor.LeerRegistros(posicion, cantidad, filtros);
            return gestor.MapearElementos(usuariosDtm).ToList();
        }

        protected override IQueryable<UsuarioDtm> AplicarJoins(IQueryable<UsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Archivo);
            return registros;
        }

        protected override IQueryable<UsuarioDtm> AplicarOrden(IQueryable<UsuarioDtm> registros, List<ClausulaDeOrdenacion> ordenacion)
        {
            registros = base.AplicarOrden(registros, ordenacion);

            if (ordenacion.Count == 0)
                return registros.OrderBy(x => x.Apellido);

            foreach (var orden in ordenacion)
            {
                if (orden.Criterio == nameof(UsuarioDtm.Apellido).ToLower())
                    return orden.Modo == ModoDeOrdenancion.ascendente
                        ? registros.OrderBy(x => x.Apellido)
                        : registros.OrderByDescending(x => x.Apellido);

                if (orden.Criterio == nameof(UsuarioDtm.Login).ToLower())
                    return orden.Modo == ModoDeOrdenancion.ascendente
                        ? registros.OrderBy(x => x.Login)
                        : registros.OrderByDescending(x => x.Login);

                if (orden.Criterio == nameof(UsuarioDtm.Alta).ToLower())
                    return orden.Modo == ModoDeOrdenancion.ascendente
                        ? registros.OrderBy(x => x.Alta)
                        : registros.OrderByDescending(x => x.Alta);
            }

            return registros;

        }

        protected override IQueryable<UsuarioDtm> AplicarFiltros(IQueryable<UsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarFiltros(registros, filtros, parametros);

            if (HayFiltroPorId)
                return registros;

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
                        registros = registros.Where(u => u.Permisos.Any(up => up.IdPermiso == id && up.IdUsuario == u.Id));
                    }
                }
                if (filtro.Clausula.ToLower() == nameof(UsuarioDtm.Login).ToLower())
                {
                    registros = registros.Where(x => x.Login == filtro.Valor);
                }
            }

            return registros;

        }

        protected override void AntesMapearRegistroParaInsertar(UsuarioDto usuarioDto, ParametrosDeNegocio opciones)
        {
            base.AntesMapearRegistroParaInsertar(usuarioDto, opciones);
            usuarioDto.Alta = DateTime.Now;
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

            if (parametros.Operacion == TipoOperacion.Insertar)
                registro.password = new GenerarPassword(Contexto).Password;

            if (parametros.Operacion == TipoOperacion.Modificar)
                registro.password = RegistroEnBD.password;

        }

        protected override void DespuesDePersistir(UsuarioDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);
            if (parametros.Operacion != TipoOperacion.Insertar)
                ServicioDeCaches.EliminarElemento(cache: typeof(UsuarioDtm).FullName, clave: $"{nameof(UsuarioDtm.Login)}-{registro.Login}");
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

        public UsuarioDto ValidarUsuario(string login, string password)
        {
            UsuarioDtm usuariodtm = null;
            try
            {
                usuariodtm = LeerRegistroCacheado(nameof(UsuarioDtm.Login), login, true, true);
                
                if (new ObtenerPassword(Contexto, usuariodtm.Login).Password != password)
                    throw new Exception("Login/password incorrecto");
            }
            catch(Exception exc)
            {
                GestorDeErrores.Emitir($"Conexión no validada {login}", exc);    
            }

            return MapearElemento(usuariodtm);
        }

        public bool TienePermisos(UsuarioDtm usuarioConectado, enumClaseDePermiso claseDePermiso, enumTipoDePermiso permisosNecesarios, object elemento)
        {
            if (usuarioConectado.EsAdministrador)
                return true;

            if (claseDePermiso == enumClaseDePermiso.Negocio)
            {
                var gestorDeNegocio = GestorDeNegocio.Gestor(Contexto, Mapeador);
                return gestorDeNegocio.TienePermisos(usuarioConectado, permisosNecesarios, (enumNegocio)elemento);
            }
            
            if (claseDePermiso == enumClaseDePermiso.Vista)
            {
                var gestorDeVista = GestorDeVistaMvc.Gestor(Contexto, Mapeador);
                return gestorDeVista.TienePermisos(usuarioConectado, permisosNecesarios, (string)elemento);
            }

            switch (elemento)
            {
                case "UsuarioDto": return true;
            }
            return false;
        }
    }

    public class ObtenerPassword : ConsultaSql
    {
        public string Password => Leidos == 0 ? "" : (string)Registros[0][0];


        public ObtenerPassword(ContextoSe contexto, string login)
        : base(contexto, $"SELECT CONVERT(VARCHAR , DECRYPTBYPASSPHRASE('sistemaSe', password)) FROM entorno.usuario where login like '{login}'")
        {
            Ejecutar();
        }
    }


    public class GenerarPassword : ConsultaSql
    {
        public string Password => Leidos == 0 ? "" : (string)Registros[0][0];


        public GenerarPassword(ContextoSe contexto)
        : base(contexto, $"SELECT CONVERT(VARCHAR , ENCRYPTBYPASSPHRASE('sistemaSe', '12345678'))")
        {
            Ejecutar();
        }
    }
}
