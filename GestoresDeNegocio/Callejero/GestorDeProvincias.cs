using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServicioDeDatos.Entorno;
using ServicioDeDatos;
using ModeloDeDto.Entorno;
using GestorDeElementos;
using ServicioDeDatos.Callejero;
using ModeloDeDto.Callejero;
using Gestor.Errores;
using Utilidades;
using GestoresDeNegocio.TrabajosSometidos;
using System.Reflection;
using System;
using Newtonsoft.Json;
using GestoresDeNegocio.Archivos;
using ServicioDeDatos.TrabajosSometidos;

namespace GestoresDeNegocio.Callejero
{
    public class GestorDeProvincias : GestorDeElementos<ContextoSe, ProvinciaDtm, ProvinciaDto>
    {
        class archivoParaImportar
        {
            public string parametro { get; set; }
            public int valor { get; set; }
        }

        public const string ParametroPais = "csvProvincia";

        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<PaisDtm, PaisDto>();
                CreateMap<PaisDto, PaisDtm>();
            }
        }

        public GestorDeProvincias(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }

        public static GestorDeProvincias Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeProvincias(contexto, mapeador); ;
        }


        private static void ImportarFicheroDeProvincias(EntornoDeTrabajo entorno, int idArchivo)
        {
            var gestor = GestorDeProvincias.Gestor(entorno.contextoPr, entorno.contextoPr.Mapeador);
            var rutaFichero = GestorDocumental.DescargarArchivo(entorno.contextoPr, idArchivo);
            var fichero = new FicheroCsv(rutaFichero);
            var linea = 0;
            entorno.AnotarTraza($"Inicio del proceso");
            var idTraza = entorno.AnotarTraza($"Procesando la fila {linea}");
            foreach (var fila in fichero)
            {
                var tran = gestor.IniciarTransaccion();
                try
                {
                    linea++;
                    if (fila.EnBlanco)
                        continue;

                    if (fila.Columnas != 5)
                        throw new Exception($"la fila {linea} solo debe tener 5 columnas");

                    if (fila["A"].IsNullOrEmpty() || fila["B"].IsNullOrEmpty() ||
                        fila["C"].IsNullOrEmpty() || fila["D"].IsNullOrEmpty() ||
                        fila["E"].IsNullOrEmpty())
                        throw new Exception($"El contenido de la fila {linea} debe ser: nombre de la provincia, nombre en ingles, iso de 2 iso de 3 y prefijo telefónico");

                    ProcesarProvinciaLeida(gestor, fila["A"], fila["B"], fila["C"], fila["D"], fila["E"], fila["F"]);
                    gestor.Commit(tran);
                }
                catch (Exception e)
                {
                    gestor.Rollback(tran);
                    entorno.AnotarError(e);
                }
                finally
                {
                    entorno.AnotarTraza(idTraza, $"Procesando la fila {linea}");
                }
            }

            entorno.AnotarTraza($"Procesadas un total de {linea} filas");
        }

        private static ProvinciaDtm ProcesarProvinciaLeida(GestorDeProvincias gestor, string codigoPais, string nombreProvincia, string sigla, string nombre, string codigo, string prefijoTelefono)
        {
            ParametrosDeNegocio operacion;
            var p = gestor.LeerRegistro(nameof(ProvinciaDtm.Codigo), codigo, false, true, true, true);
            var pais = GestorDePaises.LeerPais(gestor.Contexto,new ClausulaDeFiltrado() { Clausula = nameof(PaisDtm.Codigo), Criterio= ModeloDeDto.CriteriosDeFiltrado.igual, Valor=codigoPais});
            if (p == null)
            {
                p = new ProvinciaDtm();
                p.Codigo = codigo;
                p.Nombre = nombreProvincia;
                p.Sigla = sigla;
                p.IdPais = pais.Id;
                p.Prefijo = prefijoTelefono;
                operacion = new ParametrosDeNegocio(enumTipoOperacion.Insertar);
            }
            else
            {
                if (p.Nombre != nombreProvincia || p.Codigo != codigo || p.Sigla != sigla || p.Prefijo != prefijoTelefono)
                {
                    p.Nombre = nombreProvincia;
                    p.Sigla = sigla;
                    p.Codigo = codigo;
                    p.Prefijo = prefijoTelefono;
                    operacion = new ParametrosDeNegocio(enumTipoOperacion.Modificar);
                }
                else
                    return p;
            }

            return gestor.PersistirRegistro(p, operacion);
        }

    }
}
