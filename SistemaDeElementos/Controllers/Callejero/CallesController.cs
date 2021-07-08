using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Callejero;
using GestoresDeNegocio.Callejero;
using ModeloDeDto.Callejero;
using GestorDeElementos;
using System.Collections.Generic;

namespace MVCSistemaDeElementos.Controllers
{
    public class CallesController : EntidadController<ContextoSe, CalleDtm, CalleDto>
    {

        public CallesController(GestorDeCalles gestorDeCalles, GestorDeErrores gestorDeErrores)
         : base
         (
           gestorDeCalles,
           gestorDeErrores
         )
        {
        }

        public IActionResult CrudCalles()
        {
            return ViewCrud(new DescriptorDeCalles(Contexto, ModoDescriptor.Mantenimiento));
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            if (claseElemento == nameof(PaisDto))
                return GestorDePaises.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPaises(posicion, cantidad, filtros);

            if (claseElemento == nameof(ProvinciaDto))
                return GestorDeProvincias.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerProvincias(posicion, cantidad, filtros);

            if (claseElemento == nameof(MunicipioDto))
                return GestorDeMunicipios.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerMunicipios(posicion, cantidad, filtros);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtros);
        }

    }
}
