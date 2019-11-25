﻿using GestorDeElementos;
using GestorUniversitario.BdModelo;
using GestorUniversitario.ContextosDeBd;
using GestorUniversitario.IuModelo;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace GestorUniversitario
{
    public class GestorDeEstudiantes : GestorDeElementos<ContextoUniversitario, RegistroDeEstudiante, ElementoEstudiante>
    {

        public GestorDeEstudiantes(ContextoUniversitario contexto)
            : base(contexto)
        {

        }

        protected override RegistroDeEstudiante LeerConDetalle(int Id)
        {
            return _Contexto.Set<RegistroDeEstudiante>()
                            .Include(i => i.Inscripciones)
                            .ThenInclude(e => e.Curso)
                            .AsNoTracking()
                            .FirstOrDefault(m => m.Id == Id);
        }

        protected override void MapearDetalleParaLaIu(ElementoEstudiante elemento, RegistroDeEstudiante registro, PropertyInfo propiedadOrigen)
        {
            var gestorDeInscripciones = new GestorDeInscripciones(_Contexto);

            if (registro.Inscripciones == null)
                return;

            foreach (var inscripcion in registro.Inscripciones)
            {
                elemento.Inscripciones.Add(gestorDeInscripciones.MaperaElementoParaLaIu(inscripcion));
            }
        }
    }


}
