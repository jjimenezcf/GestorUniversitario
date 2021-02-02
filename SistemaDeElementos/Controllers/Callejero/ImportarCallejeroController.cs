using ServicioDeDatos;
using Gestor.Errores;
using Microsoft.AspNetCore.Mvc;
using MVCSistemaDeElementos.Descriptores;
using ServicioDeDatos.Callejero;
using GestoresDeNegocio.Callejero;
using ModeloDeDto.Callejero;
using AutoMapper;
using System.IO;
using Utilidades;
using System;
using Microsoft.AspNetCore.Http;

namespace MVCSistemaDeElementos.Controllers
{
    public class ImportarCallejeroController : FormularioController<ContextoSe>
    {

        public ImportarCallejeroController(ContextoSe contexto, IMapper mapeador, GestorDeErrores gestorDeErrores)
         : base(contexto
               , mapeador
               , new DescriptorImportarCallejero()
               , gestorDeErrores)
        {
        }


        public IActionResult ImportarCallejero()
        {
            return ViewFormulario();
        }

    }
}
