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
using ModeloDeDto;
using ModeloDeDto.Seguridad;

namespace GestoresDeNegocio.Entorno
{

    public class GestorDeUsuarios : GestorDeElementos<ContextoSe, UsuarioDtm, UsuarioDto>
    {

        public class MapearUsuario : Profile
        {
            public MapearUsuario()
            {
                CreateMap<UsuarioDtm, UsuarioDto>()
                .ForMember(dto => dto.NombreCompleto, dtm => dtm.MapFrom( x => UsuarioDtm.NombreCompleto(x)));
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


        protected override IQueryable<UsuarioDtm> AplicarJoins(IQueryable<UsuarioDtm> registros, List<ClausulaDeFiltrado> filtros, List<ClausulaDeJoin> joins, ParametrosDeNegocio parametros)
        {
            registros = base.AplicarJoins(registros, filtros, joins, parametros);
            registros = registros.Include(p => p.Archivo);
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

                if (filtro.Clausula.ToLower() == nameof(PermisosDeUnUsuarioDto.IdPermiso).ToLower())
                {
                        registros = registros.Where(u => u.Permisos.Any(x=>x.IdPermiso == filtro.Valor.Entero()));
                }

                if (filtro.Clausula.ToLower() == nameof(RolesDeUnPuestoDto.IdRol).ToLower())
                {
                    registros = registros.Where(u => u.Puestos.Any(x => x.Puesto.Roles.Any(y=>y.IdRol == filtro.Valor.Entero())));
                }

                if (filtro.Clausula.ToLower() == nameof(PuestosDeUnUsuarioDtm.IdPuesto).ToLower()){

                   if (filtro.Criterio == CriteriosDeFiltrado.diferente)
                    registros = registros.Where(i => !i.Puestos.Any(r => r.IdPuesto.Equals(filtro.Valor.Entero())));

                    if (filtro.Criterio == CriteriosDeFiltrado.igual)
                        registros = registros.Where(i => i.Puestos.Any(r => r.IdPuesto.Equals(filtro.Valor.Entero())));
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

            if (parametros.Operacion == enumTipoOperacion.Insertar)
                registro.password = GestorDePassword.Generar(registro.Login);

            if (parametros.Operacion == enumTipoOperacion.Modificar)
                registro.password = ((UsuarioDtm)parametros.registroEnBd).password;

        }

        protected override void DespuesDePersistir(UsuarioDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);
            if (parametros.Operacion != enumTipoOperacion.Insertar)
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
            if (registro.Archivo != null && parametros.Opciones.ContainsKey(ElementoDto.DescargarGestionDocumental) && Equals(parametros.Opciones[ElementoDto.DescargarGestionDocumental], true) )
            {
                elemento.Foto = GestorDeElementos.Utilidades.DescargarUrlDeArchivo(registro.Archivo.Id, registro.Archivo.Nombre, registro.Archivo.AlmacenadoEn);
            }
        }

        public List<UsuarioDto> LeerUsuarios(int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            var registros = LeerRegistrosPorNombre(posicion, cantidad, filtros);
            return MapearElementos(registros).ToList();
        }

        public UsuarioDto ValidarUsuario(string login, string password)
        {
            UsuarioDtm usuariodtm = null;
            try
            {
                usuariodtm = LeerRegistroCacheado(nameof(UsuarioDtm.Login), login, true, true);
                if (GestorDePassword.Leer(login) != password)
                    throw new Exception("Login/password incorrecto");
            }
            catch (Exception exc)
            {
                GestorDeErrores.Emitir($"Conexión no validada {login}", exc);
            }

            return MapearElemento(usuariodtm);
        }

        public bool TienePermisoDeDatos(UsuarioDtm usuarioConectado, enumModoDeAccesoDeDatos permisosNecesarios, object elemento)
        {
            var gestorDeNegocio = GestorDeNegocios.Gestor(Contexto, Mapeador);
            return gestorDeNegocio.TienePermisos(usuarioConectado, permisosNecesarios, (enumNegocio)elemento);
        }

        public bool TienePermisoFuncional(UsuarioDtm usuarioConectado, object elemento)
        {
            var gestorDeVista = GestorDeVistaMvc.Gestor(Contexto, Mapeador);
            return gestorDeVista.TienePermisos(usuarioConectado, (string)elemento);
        }

    }

    class GestorDePassword
    {
        class Credenciales: Registro
        {
            public string Password { get; set; }
            public string Login { get; set; }
        }

        public static string Leer(string login)
        {
            var consulta = new ConsultaSql<Credenciales>($@"SELECT LOGIN as Login, CONVERT(VARCHAR , DECRYPTBYPASSPHRASE('sistemaSe', password)) as Password
                                                         FROM entorno.usuario
                                                         where login like '{login}'");

            var credenciales = consulta.LanzarConsulta();
            if (credenciales.Count == 0)
                throw new Exception($"Credenciales del usuario {login} no localizadas");

            return credenciales[0].Password;
        }

        public static string Generar(string login)
        {
            var consulta = new ConsultaSql<Credenciales>($@"SELECT '{login}' as Login,  CONVERT(VARCHAR , ENCRYPTBYPASSPHRASE('sistemaSe', '12345678')) as Password");
            var credenciales = consulta.LanzarConsulta();
            return credenciales[0].Password;
        }

        //public string Password => Leidos == 0 ? "" : (string)Registros[0][0];


        //public Password(ContextoSe contexto, string login)
        //: base(contexto, $"SELECT CONVERT(VARCHAR , DECRYPTBYPASSPHRASE('sistemaSe', password)) FROM entorno.usuario where login like '{login}'")
        //{
        //    Ejecutar();
        //}
    }


    //class GenerarPassword : ConsultaSql
    //{
    //    public string Password => Leidos == 0 ? "" : (string)Registros[0][0];


    //    public GenerarPassword(ContextoSe contexto)
    //    : base(contexto, $"SELECT CONVERT(VARCHAR , ENCRYPTBYPASSPHRASE('sistemaSe', '12345678'))")
    //    {
    //        Ejecutar();
    //    }
    //}
}
