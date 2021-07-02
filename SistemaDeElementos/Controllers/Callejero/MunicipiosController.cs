using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Callejero;
using GestoresDeNegocio.Callejero;
using ModeloDeDto.Callejero;
using MVCSistemaDeElementos.Controllers;
using GestorDeElementos;
using System.Collections.Generic;

namespace MVCSistemaDeElementos.Controllers
{
    public class MunicipiosController : EntidadController<ContextoSe, MunicipioDtm, MunicipioDto>
    {

        public MunicipiosController(GestorDeMunicipios gestorDeMunicipios, GestorDeErrores gestorDeErrores)
         : base
         (
           gestorDeMunicipios,
           gestorDeErrores
         )
        {
        }

        public IActionResult CrudMunicipios()
        {
            return ViewCrud(new DescriptorDeMunicipios(Contexto, ModoDescriptor.Mantenimiento));
        }

        protected override dynamic CargaDinamica(string claseElemento, int posicion, int cantidad, List<ClausulaDeFiltrado> filtros)
        {
            if (claseElemento == nameof(PaisDto))
                return GestorDePaises.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerPaises(posicion, cantidad, filtros);

            if (claseElemento == nameof(ProvinciaDto))
                return GestorDeProvincias.Gestor(GestorDeElementos.Contexto, GestorDeElementos.Mapeador).LeerProvincias(posicion, cantidad, filtros);

            return base.CargaDinamica(claseElemento, posicion, cantidad, filtros);
        }

    }
}
