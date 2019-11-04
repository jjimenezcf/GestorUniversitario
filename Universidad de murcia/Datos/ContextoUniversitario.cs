﻿using Microsoft.EntityFrameworkCore;
using System.Linq;
using UniversidadDeMurcia.Models;

namespace UniversidadDeMurcia.Datos
{


    public class ContextoUniversitario : DbContext
    {
        public ContextoUniversitario(DbContextOptions<ContextoUniversitario> options) : base(options)
        {
        }

        public DbSet<Curso> Cursos { get; set; }
        public DbSet<Inscripcion> Inscripciones { get; set; }
        public DbSet<Estudiante> Estudiantes { get; set; }

        public IQueryable<Objeto> Objetos(string tipo)
        {
            IQueryable<Objeto> objetos = tipo switch
            {
                nameof(Estudiante) => Estudiantes,
                //nameof(Inscripcion) => Inscripciones,
                //nameof(Curso) => Cursos,
                _ => null,
            };
            return objetos;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Curso>().ToTable("Curso");
            modelBuilder.Entity<Inscripcion>().ToTable("Inscripcion");
            modelBuilder.Entity<Estudiante>().ToTable("Estudiante");
        }
    }
}
