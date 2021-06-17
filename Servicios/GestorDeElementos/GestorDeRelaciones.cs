using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Gestor.Errores;
using ModeloDeDto;
using ServicioDeDatos;
using ServicioDeDatos.Elemento;
using Utilidades;

namespace GestorDeElementos
{
    public interface IGestorDeRelaciones
    {
    }

    public class GestorDeRelaciones<TContexto, TRelacion, TElemento> : GestorDeElementos<TContexto, TRelacion, TElemento>, IGestorDeRelaciones
    where TRelacion : RegistroDeRelacion
    where TElemento : ElementoDto
    where TContexto : ContextoSe
    {

        private bool _invertir;

        bool InvertirMapeoDeRelacion => _invertir;

        public GestorDeRelaciones(TContexto contexto, IMapper mapper, bool invertirMapeoDeRelacion) : base(contexto, mapper)
        {
            _invertir = invertirMapeoDeRelacion;
        }

        public (TRelacion relacio, bool existe) CrearRelacion(int idElemento1, int idElemento2, bool errorSiYaExiste)
        {
            var registro = ApiDeRegistro.RegistroVacio<TRelacion>();
            if (!registro.ImplementaUnaRelacion())
                throw new Exception($"El registro {typeof(TRelacion)} no es de relación.");

            var filtros = new List<ClausulaDeFiltrado>();
            DefinirFiltroDeRelacion(registro, filtros, idElemento1, idElemento2);
            var registros = ValidarAntesDeRelacionar(filtros).ToList();

            if (registros.Count != 0 && errorSiYaExiste)
                GestorDeErrores.Emitir($"El registro {registro} ya existe");

            if (registros.Count == 0)
            {
                MapearDatosDeRelacion(registro, idElemento1, idElemento2);
                return (PersistirRegistro(registro, new ParametrosDeNegocio(enumTipoOperacion.Insertar)), false);
            }

            return (registros[0], true);
        }

        private void DefinirFiltroDeRelacion(TRelacion registro, List<ClausulaDeFiltrado> filtros, int idElemento1, int idElemento2)
        {
            var propiedades = registro.PropiedadesDelObjeto();
            foreach (var propiedad in propiedades)
            {
                var c = new ClausulaDeFiltrado
                {
                    Clausula = propiedad.Name,
                    Criterio = CriteriosDeFiltrado.igual
                };

                if (propiedad.Name == registro.ValorPropiedad(nameof(IRelacion.NombreDeLaPropiedadDelIdElemento1)).ToString())
                    c.Valor = InvertirMapeoDeRelacion ? idElemento2.ToString() : idElemento1.ToString();

                if (propiedad.Name == registro.ValorPropiedad(nameof(IRelacion.NombreDeLaPropiedadDelIdElemento2)).ToString())
                    c.Valor = InvertirMapeoDeRelacion ? idElemento1.ToString() : idElemento2.ToString();

                if (c.Valor.Entero() > 0)
                    filtros.Add(c);

                if (filtros.Count == 2)
                    break;
            }
        }

        private void MapearDatosDeRelacion(TRelacion registro, int idElemento1, int idElemento2)
        {
            var propiedades = registro.PropiedadesDelObjeto();
            foreach (var propiedad in propiedades)
            {
                if (propiedad.Name == registro.ValorPropiedad(nameof(IRelacion.NombreDeLaPropiedadDelIdElemento1)).ToString())
                    propiedad.SetValue(registro, InvertirMapeoDeRelacion ? idElemento2 : idElemento1);

                if (propiedad.Name == registro.ValorPropiedad(nameof(IRelacion.NombreDeLaPropiedadDelIdElemento2)).ToString())
                    propiedad.SetValue(registro, InvertirMapeoDeRelacion ? idElemento1 : idElemento2);
            }

            //throw new Exception($"El gestor: {this} no tiene definida la función de {nameof(MapearDatosDeRelacion)}.");
        }

        public List<TRelacion> ValidarAntesDeRelacionar(List<ClausulaDeFiltrado> filtros)
        {
            return LeerRegistros(0, 1, filtros, null, null, null);
        }

    }
}
