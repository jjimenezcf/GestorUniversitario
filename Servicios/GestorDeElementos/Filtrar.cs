using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Reflection;
using ModeloDeDto;
using ServicioDeDatos.Elemento;
using Utilidades;

namespace GestorDeElementos
{
    public static class Filtrar
    {

        public static IQueryable<TRegistro> AplicarFiltroPorExpresion<TRegistro>(this IQueryable<TRegistro> registros, string expresion)
        {
            return registros.Where(expresion);
        }

        public static IQueryable<TRegistro> AplicarFiltroPorEntero<TRegistro>(this IQueryable<TRegistro> registros, ClausulaDeFiltrado filtro, string propiedad)
        {
            string expresion;
            if (!(filtro.Criterio == CriteriosDeFiltrado.esNulo || filtro.Criterio == CriteriosDeFiltrado.noEsNulo || filtro.Criterio == CriteriosDeFiltrado.esAlgunoDe) && !filtro.Valor.EsEntero())
                throw new Exception($"Se ha solicitado filtrar por un número, y el valor pasado no lo es. Filtro: {filtro.Clausula}, Criterio {filtro.Criterio} Valor: '{filtro.Valor}' Dtm: {typeof(TRegistro).Name}. ");

            var valorEntero = filtro.Valor.Entero();

            switch (filtro.Criterio)
            {
                case CriteriosDeFiltrado.igual:
                    expresion = $"x => x.{propiedad}.Equals({valorEntero})";
                    return registros.AplicarFiltroPorExpresion(expresion);
                case CriteriosDeFiltrado.mayor:
                    expresion = $"x => x.{propiedad} > {valorEntero}";
                    return registros.AplicarFiltroPorExpresion(expresion);
                case CriteriosDeFiltrado.menor:
                    expresion = $"x => x.{propiedad} < {valorEntero}";
                    return registros.AplicarFiltroPorExpresion(expresion);
                case CriteriosDeFiltrado.mayorIgual:
                    expresion = $"x => x.{ propiedad} >= {valorEntero}";
                    return registros.AplicarFiltroPorExpresion(expresion);
                case CriteriosDeFiltrado.menorIgual:
                    expresion = $"x => x.{propiedad} <= {valorEntero}";
                    return registros.AplicarFiltroPorExpresion(expresion);
                case CriteriosDeFiltrado.esNulo:
                    expresion = $"x => x.{propiedad} == null";
                    return registros.AplicarFiltroPorExpresion(expresion);
                case CriteriosDeFiltrado.noEsNulo:
                    expresion = $"x => x.{propiedad} != null";
                    return registros.AplicarFiltroPorExpresion(expresion);
                case CriteriosDeFiltrado.esAlgunoDe:
                    var lista = filtro.Valor.Split(',').Select(s => s.Entero()).ToArray();
                    return registros = registros.Where($"@0.Contains({propiedad})", lista);
                default:
                    new Exception($"El filtro {filtro.Clausula} para la entidad {registros.GetType()} por el criterio {filtro.Criterio} no está definido");
                    break;
            }
            return registros;
        }

        public static IQueryable<TRegistro> AplicarFiltroPorIdentificador<TRegistro>(this IQueryable<TRegistro> registros, ClausulaDeFiltrado filtro, string propiedad)
        {
            var valorEntero = filtro.Valor.Entero();

            if (valorEntero == 0 && !(filtro.Criterio == CriteriosDeFiltrado.esNulo || filtro.Criterio == CriteriosDeFiltrado.noEsNulo || filtro.Criterio == CriteriosDeFiltrado.esAlgunoDe))
                throw new Exception($"Se ha solicitado filtrar por {filtro.Clausula}, con el criterio {filtro.Criterio} y el valor proporcionado es '{filtro.Valor}', y eso no se puede hacer sobre la tabla {typeof(TRegistro).Name}. ");

            return registros.AplicarFiltroPorEntero(filtro, propiedad);
        }

        public static IQueryable<TRegistro> AplicarFiltroPorFecha<TRegistro>(this IQueryable<TRegistro> registros, ClausulaDeFiltrado filtro, string propiedad)
        {
            switch (filtro.Criterio)
            {
                case CriteriosDeFiltrado.entreFechas:
                    return AplicarFiltroEntreFechas(registros, filtro, propiedad);
                case CriteriosDeFiltrado.igual:
                    return AplicarFiltroPorFechaIgual(registros, filtro, propiedad);
                default:
                    new Exception($"El filtro {filtro.Clausula} para la entidad {registros.GetType()} por el criterio {filtro.Criterio} no está definido");
                    break;
            }
            return registros;
        }

        public static IQueryable<TRegistro> AplicarFiltroEntreFechas<TRegistro>(this IQueryable<TRegistro> registros, ClausulaDeFiltrado filtro, string propiedad)
        {
            var fecha = ParsearFechas(filtro.Valor);
            var expresionFechaDesde = fecha.desde != null ? $"x.{propiedad} >= DateTime({((DateTime)fecha.desde).Year},{((DateTime)fecha.desde).Month},{((DateTime)fecha.desde).Day},{((DateTime)fecha.desde).Hour},{((DateTime)fecha.desde).Minute},{((DateTime)fecha.desde).Second})" : "";
            var expresionFechaHasta = fecha.hasta != null ? $"x.{propiedad} <= DateTime({((DateTime)fecha.hasta).Year},{((DateTime)fecha.hasta).Month},{((DateTime)fecha.hasta).Day},{((DateTime)fecha.hasta).Hour},{((DateTime)fecha.hasta).Minute},{((DateTime)fecha.hasta).Second})" : "";
            string expresion = $"x => {expresionFechaDesde} {(fecha.desde != null && fecha.hasta != null ? "&&" : "")} {expresionFechaHasta}";
            return registros.AplicarFiltroPorExpresion(expresion);
        }

        public static IQueryable<TRegistro> AplicarFiltroPorFechaIgual<TRegistro>(this IQueryable<TRegistro> registros, ClausulaDeFiltrado filtro, string propiedad)
        {
            var expresionFecha = $"x.{propiedad} = null || x.{propiedad} = DateTime({"0001"},{"01"},{"01"},{"00"},{"00"},{"00"})";
            if (!filtro.Valor.IsNullOrEmpty())
            {
                var fecha = filtro.Valor.Fecha();
                expresionFecha = $"x.{propiedad} = DateTime({((DateTime)fecha).Year},{((DateTime)fecha).Month},{((DateTime)fecha).Day},{((DateTime)fecha).Hour},{((DateTime)fecha).Minute},{((DateTime)fecha).Second})";
            }
            return registros.AplicarFiltroPorExpresion($"x => {expresionFecha}");
        }

        private static (DateTime? desde, DateTime? hasta) ParsearFechas(string valor)
        {
            var fechas = valor.Split("-");
            return (fechas[0].Fecha(), fechas[1].Fecha(true));
        }

        public static IQueryable<TRegistro> AplicarFiltroPorBooleano<TRegistro>(this IQueryable<TRegistro> registros, ClausulaDeFiltrado filtro, string propiedad)
        {
            string expresion;
            if (filtro.Valor == null)
                return registros;

            var valorBooleano = bool.Parse(filtro.Valor);
            switch (filtro.Criterio)
            {
                case CriteriosDeFiltrado.igual:
                    expresion = $"x => x.{propiedad} == {valorBooleano}";
                    return registros.AplicarFiltroPorExpresion(expresion);
                case CriteriosDeFiltrado.diferente:
                    expresion = $"x => x.{propiedad} != {valorBooleano}";
                    return registros.AplicarFiltroPorExpresion(expresion);
                default:
                    new Exception($"El filtro {filtro.Clausula} para la entidad {registros.GetType()} por el criterio {filtro.Criterio} no está definido");
                    break;
            }
            return registros;
        }

        public static IQueryable<TRegistro> AplicarFiltroDeCadena<TRegistro>(this IQueryable<TRegistro> registros, ClausulaDeFiltrado filtro, string propiedad)
        {
            string expresion;

            if (filtro.Valor.IsNullOrEmpty() && !(filtro.Criterio == CriteriosDeFiltrado.esNulo || filtro.Criterio == CriteriosDeFiltrado.noEsNulo))
                return registros;

            switch (filtro.Criterio)
            {
                case CriteriosDeFiltrado.igual:
                    expresion = $"x => x.{propiedad}.Equals(\"{filtro.Valor}\")";
                    return registros.AplicarFiltroPorExpresion(expresion);
                case CriteriosDeFiltrado.contiene:
                    expresion = $"x => x.{propiedad}.Contains(\"{filtro.Valor}\")";
                    return registros.AplicarFiltroPorExpresion(expresion);
                case CriteriosDeFiltrado.diferente:
                    expresion = $"x => !x.{propiedad}.Contains(\"{filtro.Valor}\")";
                    return registros.AplicarFiltroPorExpresion(expresion);
                case CriteriosDeFiltrado.comienza:
                    expresion = $"x => x.{propiedad}.StartsWith(\"{filtro.Valor}\")";
                    return registros.AplicarFiltroPorExpresion(expresion);
                case CriteriosDeFiltrado.termina:
                    expresion = $"x => x.{propiedad}.EndsWith(\"{filtro.Valor}\")";
                    return registros.AplicarFiltroPorExpresion(expresion);
                case CriteriosDeFiltrado.esNulo:
                    expresion = $"x => x.{propiedad}.IsNullOrEmpty()";
                    return registros.AplicarFiltroPorExpresion(expresion);
                case CriteriosDeFiltrado.noEsNulo:
                    expresion = $"!x => x.{propiedad}.IsNullOrEmpty()";
                    return registros.AplicarFiltroPorExpresion(expresion);
                default:
                    new Exception($"El filtro {filtro.Clausula} para la entidad {registros.GetType()} por el criterio {filtro.Criterio} no está definido");
                    break;
            }
            return registros;
        }

        public static IQueryable<TRegistro> AplicarFiltroPorPropiedad<TRegistro>(this IQueryable<TRegistro> registros, ClausulaDeFiltrado filtro, PropertyInfo propiedad)
        {

            if (propiedad.Name.ToLower().Equals("id") && filtro.Criterio != CriteriosDeFiltrado.igual)
            {
                registros = registros.AplicarFiltroPorIdentificador(filtro, propiedad.Name);
            }

            if (propiedad.PropertyType == typeof(string))
            {
                registros = registros.AplicarFiltroDeCadena(filtro, propiedad.Name);
            }

            if ((propiedad.PropertyType == typeof(int) || propiedad.PropertyType == typeof(int?)) && propiedad.Name.ToLower().StartsWith("id") && propiedad.Name.Length > 2)
            {
                registros = registros.AplicarFiltroPorIdentificador(filtro, propiedad.Name);
            }

            if ((propiedad.PropertyType == typeof(int) || propiedad.PropertyType == typeof(int?)) && !propiedad.Name.ToLower().StartsWith("id"))
            {
                registros = registros.AplicarFiltroPorEntero(filtro, propiedad.Name);
            }

            if (propiedad.PropertyType == typeof(bool))
            {
                registros = registros.AplicarFiltroPorBooleano(filtro, propiedad.Name);
            }

            if (propiedad.PropertyType == typeof(DateTime) || propiedad.PropertyType == typeof(DateTime?))
            {
                registros = registros.AplicarFiltroPorFecha(filtro, propiedad.Name);
            }

            return registros;
        }

        public static IQueryable<TRegistro> AplicarFiltroPorPropiedades<TRegistro>(this IQueryable<TRegistro> registros, List<ClausulaDeFiltrado> filtros) where TRegistro : IRegistro
        {
            var propiedades = typeof(TRegistro).GetProperties();

            foreach (ClausulaDeFiltrado filtro in filtros)
            {
                foreach (var propiedad in propiedades)
                {

                    if (!propiedad.Name.ToLower().Equals(filtro.Clausula.ToLower()))
                        continue;

                    if (propiedad.Name.ToLower().Equals(nameof(IRegistro.Id).ToLower()) && filtro.Criterio == CriteriosDeFiltrado.igual)
                        continue;

                    registros = registros.AplicarFiltroPorPropiedad(filtro, propiedad);
                }

            }
            return registros;
        }
    }

    public class ClausulaDeFiltrado
    {
        public string Clausula { get; set; }

        public CriteriosDeFiltrado Criterio { get; set; }

        private string _valor = "";
        public string Valor { get { return _valor.Trim(); } set { _valor = value == null ? "" : value; } }

    }
}
