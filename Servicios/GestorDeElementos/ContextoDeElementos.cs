using Microsoft.EntityFrameworkCore;
using Gestor.Elementos.ModeloBd;
using System;
using GestorDeElementos.Utilidades;
using Utilidades.Traza;
using System.Data.Common;
using Z.EntityFramework.Extensions;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using Utilidades;
using System.IO;
using System.Collections.Generic;

namespace Gestor.Elementos
{

    public class Literal
    {
        internal static readonly string usuario = "jjimenezcf@gmail.com";
        internal static readonly string DebugarSqls = nameof(DebugarSqls);
        internal static readonly string esquemaBd = "ENTORNO";
        internal static readonly string version = "Versión";
        internal static readonly string Version_0 = "0.0.0";
        public static readonly string CadenaDeConexion = nameof(CadenaDeConexion);

        public class Vista
        {
            internal static string Catalogo = "CatalogoDelSe";
        }
        internal class Tabla
        {
            internal static string Variable = "Var_Elemento";
        }
    }
    public class DatosDeConexion
    {
        public string ServidorWeb { get; set; }
        public string ServidorBd { get; set; }
        public string Bd { get; set; }
        public string Usuario { get; set; }
        public string Version { get; set; }
        public string Menu { get; set; }
 
    }
    public class DebugarSql : ConsultaSql
    {
        public bool DebugarSqls => (Registros.Count == 1 ? Registros[0][3].ToString() == "S" : false);

        public DebugarSql(ContextoDeElementos contexto)
        : base(contexto, $"Select * from {Literal.esquemaBd}.{Literal.Tabla.Variable} where NOMBRE like '{Literal.DebugarSqls}'")
        {
            Ejecutar();
        }
    }
    public class VersionSql : ConsultaSql
    {
        public string Version => (Registros.Count == 1 ? (string)Registros[0][3] : Literal.Version_0);

        public VersionSql(ContextoDeElementos contexto)
            : base(contexto, $"Select * from {Literal.esquemaBd}.{Literal.Tabla.Variable} where NOMBRE like '{Literal.version}'")
        {
            Ejecutar();
        }
    }
    public class ContextoDeElementos : DbContext
    {
        public DatosDeConexion DatosDeConexion { get; private set; }
        public IConfiguration Configuracion { get; private set; }

        public bool Debuggar
        {
            get
            {
                var a = new DebugarSql(this);
                if (a != null)
                    return a.DebugarSqls;

                return false;
            }
        }


        public TrazaSql Traza { get; private set; }
        private InterceptadorDeConsultas _interceptadorDeConsultas;

        public ContextoDeElementos(DbContextOptions options, IConfiguration configuracion) :
        base(options)
        {
            Configuracion = configuracion;

            _interceptadorDeConsultas = new InterceptadorDeConsultas();
            DbInterception.Add(_interceptadorDeConsultas);

            InicializarDatosContexto();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            var conexion = Configuracion.GetConnectionString(Literal.CadenaDeConexion);
            options.UseSqlServer(conexion, x => x.MigrationsHistoryTable("__Migraciones", "ENTORNO"))
                   .UseSqlServer(conexion, x => x.MigrationsAssembly("Migraciones"));
        }

        public void InicializarDatosContexto()
        {
            DatosDeConexion = new DatosDeConexion();
            DatosDeConexion.ServidorWeb = Environment.MachineName;
            DatosDeConexion.ServidorBd = Database.GetDbConnection().DataSource;
            DatosDeConexion.Bd = Database.GetDbConnection().Database;
            DatosDeConexion.Usuario = Literal.usuario;

            try
            {
                DatosDeConexion.Version = new ExisteTabla(this, Literal.Tabla.Variable).Existe ?
                                          ObtenerVersion() :
                                          Literal.Version_0;
            }
            catch
            {
                DatosDeConexion.Version = Literal.Version_0;
            }

            DatosDeConexion.Menu = $@"
                     <ul id='id_menuraiz' class=¨menu-contenido¨>
                        <li>
                            <a>{ComponerMenu(literalOpcion: "Configuracion", icono: "cog-solid.svg", idMenu: "id_menu_configuracion")}</a>
                            <ul id =¨id_menu_configuracion¨ name =¨menu¨ menu-plegado=¨true¨ >
                                <li>
                                  <a>{ComponerMenu(literalOpcion: "Funcionalidad", icono: "bars-solid.svg", "")}</a>
                                </li>
                                <li>
                                    <a>{ComponerMenu(literalOpcion: "Accesos", icono: "cog-solid.svg", idMenu: "id_menu_accesos")}</a>
                                    <ul id =¨id_menu_accesos¨ name =¨menu¨ menu-plegado=¨true¨>
                                       {componerOpcion(idOpcion: "id_menu_usuarios", literal: "Usuarios")}
                                       {componerOpcion(idOpcion: "id_menu_permisos", literal: "Permisos")}
                                    </ul>
                                </li>
                            </ul>
                        </li>
                        <li>
                            <a>{ComponerMenu(literalOpcion: "Maestros", icono: "cog-solid.svg", idMenu: "id_menu_maestros")}</a>
                            <ul id=¨id_menu_maestros¨ name=¨menu¨ menu-plegado=¨true¨>
                            </ul>
                        </li>
                        <li>
                            <a>{ComponerMenu(literalOpcion: "Gestión documental", icono: "cog-solid.svg", idMenu: "id_menu_gestion_documental")}</a>
                            <ul id=¨id_menu_gestion_documental¨ name=¨menu¨ menu-plegado=¨true¨>
                            </ul>
                        </li>
                        <li>
                            <a>{ComponerMenu(literalOpcion: "Gestión administrativa", icono: "cog-solid.svg", idMenu: "id_menu_gestion_administrativa")}</a>
                            <ul id=¨id_menu_gestion_administrativa¨ name=¨menu¨ menu-plegado=¨true¨>
                            </ul>
                        </li>
                        <li>
                            <a>{ComponerMenu(literalOpcion: "Gestión jurídica", icono: "cog-solid.svg", idMenu: "id_menu_gestion_jurídica")}</a>
                            <ul id=¨id_menu_gestion_jurídica¨ name=¨menu¨ menu-plegado=¨true¨>
                            </ul>
                        </li>
                        <li>
                            <a>{ComponerMenu(literalOpcion: "Gestión logística", icono: "cog-solid.svg", idMenu: "id_menu_gestion_logística")}</a>
                            <ul id=¨id_menu_gestion_logística¨ name=¨menu¨ menu-plegado=¨true¨>
                            </ul>
                        </li>
                        <li>
                            <a>{ComponerMenu(literalOpcion: "Gestión técnica", icono: "cog-solid.svg", idMenu: "id_menu_gestion_técnica")}</a>
                            <ul id=¨id_menu_gestion_técnica¨ name=¨menu¨ menu-plegado=¨true¨>
                            </ul>
                        </li>
                        <li>
                            <a>{ComponerMenu(literalOpcion: "Gestión financiera", icono: "cog-solid.svg", idMenu: "id_menu_gestion_financiera")}</a>
                            <ul id=¨id_menu_gestion_financiera¨ name=¨menu¨ menu-plegado=¨true¨>
                            </ul>
                        </li>
                    </ul>            ";
        }

        private string componerOpcion(string idOpcion, string literal)
        {
            var opcionHtml = $@"
                <li>
                  <input id='{idOpcion}' type='button' class='menu-opcion' value='{literal}' onclick=¨Menu.OpcionSeleccionada('{idOpcion}')¨ />
                </li>
                ";

            return opcionHtml;
        }

        private object ComponerMenu(string literalOpcion, string icono, string idMenu)
        {
            var opcionHtml = "";

            if (!icono.IsNullOrEmpty() && File.Exists($@"wwwroot\images\menu\{icono}"))
                opcionHtml = @$"<img src=¨/images/menu/{icono}¨ class=¨icono izquierdo¨ />" + "\n";

            opcionHtml = $@"{opcionHtml}{literalOpcion}";

            if (!idMenu.IsNullOrEmpty())
                opcionHtml = $"{opcionHtml}\n<img src=¨/images/menu/angle-down-solid.svg¨ class=¨icono derecho¨ onclick=¨Menu.MenuPulsado('{idMenu}')¨/>";

            return opcionHtml;
        }

        private string ObtenerVersion()
        {
            var a = new VersionSql(this);
            if (a != null)
                return a.Version;

            return Literal.Version_0;
        }

        public DbSet<CatalogoDelSe> CatalogoDelSe { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CatalogoDelSe>().ToView(Literal.Vista.Catalogo);
        }

        public void IniciarTraza()
        {
            if (!Debuggar)
                return;

            if (Traza == null)
                CrearTraza(NivelDeTraza.Siempre, @"c:\Temp\Trazas", $"traza_{DateTime.Now}.txt");
            else
            if (!Traza.Abierta)
                Traza.Abrir(true);
        }

        public void CerrarTraza()
        {
            if (Traza != null)
            {
                if (!Traza.Abierta)
                    Traza.Abrir(true);

                Traza.CerrarTraza("Conexión cerrada");
            }
        }

        private void CrearTraza(NivelDeTraza nivel, string ruta, string fichero)
        {
            Traza = new TrazaSql(nivel, ruta, fichero, $"Traza iniciada por {DatosDeConexion.Usuario}");
            _interceptadorDeConsultas.Traza = Traza;
        }


    }
    public class InterceptadorDeConsultas : DbCommandInterceptor
    {
        public TrazaSql Traza { get; set; }
        private DbCommand _sentenciaSql { get; set; }
        private Stopwatch _cronoSql;


        public override void NonQueryExecuting(DbCommand sentenciaSql, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuting(sentenciaSql, interceptionContext);
            _sentenciaSql = sentenciaSql;
            _cronoSql = new Stopwatch();
            _cronoSql.Start();
        }
        public override void NonQueryExecuted(DbCommand command, DbCommandInterceptionContext<int> interceptionContext)
        {
            base.NonQueryExecuted(command, interceptionContext);
            RegistrarTraza();
        }
        public override void NonQueryError(DbCommand sentenciaSql, DbCommandInterceptionContext<int> interceptionContext, Exception exception)
        {
            base.NonQueryError(sentenciaSql, interceptionContext, exception);
            RegistrarError(exception);
        }


        public override void ReaderExecuting(DbCommand sentenciaSql, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuting(sentenciaSql, interceptionContext);
            _sentenciaSql = sentenciaSql;
            _cronoSql = new Stopwatch();
            _cronoSql.Start();
        }
        public override void ReaderExecuted(DbCommand sentenciaSql, DbCommandInterceptionContext<DbDataReader> interceptionContext)
        {
            base.ReaderExecuted(sentenciaSql, interceptionContext);
            RegistrarTraza();
        }
        public override void ReaderError(DbCommand sentenciaSql, DbCommandInterceptionContext<DbDataReader> interceptionContext, Exception exception)
        {
            base.ReaderError(sentenciaSql, interceptionContext, exception);
            RegistrarError(exception);
        }



        public override void ScalarExecuting(DbCommand sentenciaSql, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuting(sentenciaSql, interceptionContext);
            RegistrarTraza();
        }
        public override void ScalarExecuted(DbCommand sentenciaSql, DbCommandInterceptionContext<object> interceptionContext)
        {
            base.ScalarExecuted(sentenciaSql, interceptionContext);
            RegistrarTraza();
        }
        public override void ScalarError(DbCommand sentenciaSql, DbCommandInterceptionContext<object> interceptionContext, Exception exception)
        {
            base.ScalarError(sentenciaSql, interceptionContext, exception);
            RegistrarError(exception);
        }

        private void RegistrarTraza()
        {
            if (_cronoSql != null)
            {
                _cronoSql.Stop();

                if (Traza != null)
                    Traza.AnotarTrazaSql(_sentenciaSql.CommandText, _sentenciaSql.Parameters, _cronoSql.ElapsedMilliseconds);
            }
        }

        private void RegistrarError(Exception excepcion)
        {
            if (_cronoSql != null)
            {
                _cronoSql.Stop();

                if (Traza != null)
                    Traza.AnotarExcepcion(excepcion);
            }
        }
    }
}


