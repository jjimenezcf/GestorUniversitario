﻿using ModeloDeDto.Callejero;
using ServicioDeDatos;
using MVCSistemaDeElementos.Controllers;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorDeProvincias : DescriptorDeCrud<ProvinciaDto>
    {
        public DescriptorDeProvincias(ContextoSe contexto, ModoDescriptor modo)
        : base(contexto
               , nameof(ProvinciasController)
                 , nameof(ProvinciasController.CrudProvincias)
                 , modo
                 , rutaBase: "Callejero")
        {            
            new ListasDinamicas<ProvinciaDto>(Mnt.BloqueGeneral,
                etiqueta: "País",
                filtrarPor: nameof(ProvinciaDto.IdPais),
                ayuda: "seleccione al país",
                seleccionarDe: nameof(PaisDto),
                buscarPor: nameof(PaisDto.Nombre),
                mostrarExpresion: $"([{nameof(PaisDto.Codigo)}]) [{nameof(PaisDto.Nombre)}]",
                criterioDeBusqueda: ModeloDeDto.CriteriosDeFiltrado.contiene,
                posicion: new Posicion(0, 0),
                controlador: nameof(PaisesController),
                restringirPor: "",
                alSeleccionarBlanquearControl: "");

            new EditorFiltro<ProvinciaDto>(bloque: Mnt.BloqueGeneral
                , etiqueta: "Codigo"
                , propiedad: nameof(ProvinciaDto.Codigo)
                , ayuda: "buscar por codigo"
                , new Posicion { fila = 0, columna = 1 });

            AnadirOpcionDeDependencias(Mnt
                , controlador: nameof(MunicipiosController)
                , vista: nameof(MunicipiosController.CrudMunicipios)
                , datosDependientes: nameof(MunicipioDto)
                , navegarAlCrud: DescriptorDeMantenimiento<MunicipioDto>.NombreMnt
                , nombreOpcion: "Municipios"
                , propiedadQueRestringe: nameof(ProvinciaDto.Id)
                , propiedadRestrictora: nameof(MunicipioDto.IdProvincia)
                , "Municipios de una provincia");

            new EditorFiltro<ProvinciaDto>(bloque: Mnt.BloqueGeneral
                , etiqueta: "CP"
                , propiedad: nameof(CpsDeUnMunicipioDto.CodigoPostal)
                , ayuda: "buscar por codigo postal"
                , new Posicion { fila = 1, columna = 1 });


            AnadirOpciondeRelacion(Mnt
                , controlador: nameof(CpsDeUnaProvinciaController)
                , vista: nameof(CpsDeUnaProvinciaController.CrudCpsDeUnaProvincia)
                , relacionarCon: nameof(CodigoPostalDto)
                , navegarAlCrud: DescriptorDeMantenimiento<CpsDeUnaProvinciaDto>.NombreMnt
                , nombreOpcion: "C.P."
                , propiedadQueRestringe: nameof(ProvinciaDto.Id)
                , propiedadRestrictora: nameof(CpsDeUnaProvinciaDto.IdProvincia)
                , "Añadir puestos al usuario seleccionado");

            RecolocarControl(Mnt.Filtro.FiltroDeNombre, new Posicion(1, 0), "Provincia", "Buscar Buscar por nombre de provincia");
            Mnt.OrdenacionInicial = @$"{nameof(ProvinciaDto.Pais)}:pais.nombre:{enumModoOrdenacion.ascendente.Render()};
                                       {nameof(ProvinciaDto.Nombre)}:nombre:{enumModoOrdenacion.ascendente.Render()}";
        }


        public override string RenderControl()
        {
            var render = base.RenderControl();

            render = render +
                   $@"<script src=¨../../js/Callejero/literales.js¨></script>
                      <script src=¨../../js/Callejero/Provincias.js¨></script>
                      <script>
                         try {{      
                           Callejero.CrearCrudDeProvincias('{Mnt.IdHtml}','{Creador.IdHtml}','{Editor.IdHtml}', '{Borrado.IdHtml}') 
                         }}
                         catch(error) {{                           
                            MensajesSe.Error('Creando el crud', error.message);
                         }}
                      </script>
                    ";
            return render.Render();
        }
    }
}

