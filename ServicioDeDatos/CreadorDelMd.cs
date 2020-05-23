using Microsoft.EntityFrameworkCore;
using System;
using Utilidades.Traza;
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Collections.Concurrent;
using ServicioDeDatos.Utilidades;
using Z.EntityFramework.Extensions;
using ServicioDeDatos.Elemento;
using Microsoft.EntityFrameworkCore.Design;
using ServicioDeDatos.Entorno;
using ServicioDeDatos.Seguridad;
using ServicioDeDatos.Archivos;

namespace ServicioDeDatos
{
    public partial class ContextoSe : DbContext
    {
        #region dbSets del esquema ENTORNO
        public DbSet<MenuDtm> Menus { get; set; }
        public DbSet<ArbolDeMenuDtm> MenuSe { get; set; }
        public DbSet<VistaMvcDtm> VistasMvc { get; set; }
        public DbSet<VariableDtm> Variables { get; set; }
        public DbSet<UsuarioDtm> Usuarios { get; set; }
        public DbSet<UsuariosDeUnPermisoDtm> UsuPermisos { get; set; }

        #endregion

        #region dbSets del esquema SEGURIDAD

        public DbSet<TipoPermisoDtm> TiposDePermisos { get; set; }
        public DbSet<ClasePermisoDtm> ClasesDePermisos { get; set; }
        public DbSet<PermisoDtm> Permisos { get; set; }
        public DbSet<RolDtm> Roles { get; set; }
        public DbSet<PuestoDtm> Puestos { get; set; }
        public DbSet<RolesDeUnPermiso> PermisosDeUnRol { get; set; }
        public DbSet<RolesDeUnPuestoDtm> PuestosDeUnRol { get; set; }
        public DbSet<PuestosDeUsuarioDtm> PuestosDeUnUsuario { get; set; }

        #endregion


        #region dbSets del esquema de SISDOC

        public DbSet<ArchivoDtm> Archivos { get; set; }

        #endregion

        public DbSet<CatalogoDelSe> CatalogoDelSe { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CatalogoDelSe>().ToView(Literal.Vista.Catalogo);

            DefinirTablasDelEsquemaDeEntorno(modelBuilder);

            DefinirEsquemaDeSeguridad(modelBuilder);

            DefinirTablasDelEsquemaSisDoc(modelBuilder);

        }

        private static void DefinirEsquemaDeSeguridad(ModelBuilder modelBuilder)
        {
            TablaClasePermiso.Definir(modelBuilder);

            TablaPermiso.Definir(modelBuilder);

            TablaPuesto.Definir(modelBuilder);

            TablaRol.Definir(modelBuilder);

            TablaRolPermiso.Definir(modelBuilder);

            TablaRolPuesto.Definir(modelBuilder);

            TablaPermisoTipo.Definir(modelBuilder);

            TablaUsuPuesto.Definir(modelBuilder);
        }

        private static void DefinirTablasDelEsquemaDeEntorno(ModelBuilder modelBuilder)
        {
            TablaVistaMvc.Definir(modelBuilder);

            TablaVariable.Definir(modelBuilder);

            VistaUsuarioPermiso.Definir(modelBuilder);

            TablaUsuario.Definir(modelBuilder);

            TablaMenu.Definir(modelBuilder);

            VistaMenuSe.Definir(modelBuilder);
        }

        private static void DefinirTablasDelEsquemaSisDoc(ModelBuilder modelBuilder)
        {
            TablaArchivo.Definir(modelBuilder);
        }

    }
}
