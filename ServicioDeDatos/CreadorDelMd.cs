﻿using Microsoft.EntityFrameworkCore;
using ServicioDeDatos.Elemento;
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
        public DbSet<VistaMvcDtm> VistaMvc { get; set; }
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
        public DbSet<PermisosDeUnRolDtm> PermisosDeUnRol { get; set; }

        //public DbSet<RolesDeUnPermisoDtm> RolesDeUnPermiso { get; set; }

        public DbSet<RolesDeUnPuestoDtm> PuestosDeUnRol { get; set; }
        public DbSet<PuestosDeUnUsuarioDtm> PuestosDeUnUsuario { get; set; }

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

            //TablaPermisoRol.Definir(modelBuilder);

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
