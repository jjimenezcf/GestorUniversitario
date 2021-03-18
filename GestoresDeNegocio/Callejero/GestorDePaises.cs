using System.Collections.Generic;
using AutoMapper;
using ServicioDeDatos;
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

namespace GestoresDeNegocio.Callejero
{

    public class GestorDePaises : GestorDeElementos<ContextoSe, PaisDtm, PaisDto>
    {

        class archivoParaImportar
        {
            public string parametro { get; set; }
            public int valor { get; set; }
        }

        public const string ParametroPais = "csvPais";

        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<PaisDtm, PaisDto>();
                CreateMap<PaisDto, PaisDtm>();
            }
        }

        public GestorDePaises(ContextoSe contexto, IMapper mapeador)
        : base(contexto, mapeador)
        {

        }

        public static GestorDePaises Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDePaises(contexto, mapeador); ;
        }

        public static void SometerImportarCallejero(ContextoSe contexto, string parametros)
        {
            if (parametros.IsNullOrEmpty())
                GestorDeErrores.Emitir("No se han proporcionado los parámetros para someter el trabajo de importación");

            var dll = Assembly.GetExecutingAssembly().GetName().Name;
            var clase = typeof(GestorDePaises).FullName;
            var ts = GestorDeTrabajosSometido.Obtener(contexto, "Importar callejero", dll, clase, nameof(SometerImportarCallejero).Replace("Someter", ""));
            // crear trabajo de usuario

            var tu = GestorDeTrabajosDeUsuario.Crear(contexto, ts, parametros);
            //liberarlo
        }

        public static void ImportarCallejero(EntornoDeTrabajo entorno)
        {
            var archivos = JsonConvert.DeserializeObject<List<archivoParaImportar>>(entorno.Trabajo.Parametros);

            if (archivos.Count == 0)
                GestorDeErrores.Emitir("No se ha sometido ningún fichero a cargar");

            foreach (archivoParaImportar archivo in archivos)
            {
                switch (archivo.parametro)
                {
                    case ParametroPais:
                        ImportarFicheroDePaises(entorno, archivo.valor);
                        break;
                    default:
                        GestorDeErrores.Emitir($"No es valido el parámetro {archivo.parametro} en el proceso {nameof(ImportarCallejero)}");
                        break;
                }
            }
        }


        private static void ImportarFicheroDePaises(EntornoDeTrabajo entorno, int idArchivo)
        {
            var gestor = GestorDePaises.Gestor(entorno.contextoPr, entorno.contextoPr.Mapeador);
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
                        throw new Exception($"El contenido de la fila {linea} debe ser: nombre del país, nombre en ingles, iso de 2 iso de 3 y prefijo telefónico");

                    ProcesarPaisLeido(gestor, fila["A"], fila["B"], fila["C"],fila["D"], fila["E"]);
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

        internal static PaisDtm LeerPais(ContextoSe contexto, ClausulaDeFiltrado clausulaDeFiltrado)
        {
            throw new NotImplementedException();
        }

        private static PaisDtm ProcesarPaisLeido(GestorDePaises gestor, string nombrePais, string nombreEnIngles, string Iso2, string codigoPais, string prefijoTelefono )
        {
            ParametrosDeNegocio operacion;
            var p = gestor.LeerRegistro(nameof(PaisDtm.Codigo), codigoPais, false, true, true, true);
            if (p == null)
            {
                p = new PaisDtm();
                p.Codigo = codigoPais;
                p.Nombre = nombrePais;
                p.NombreIngles = nombreEnIngles;
                p.ISO2 = Iso2;
                p.Prefijo = prefijoTelefono;
                operacion = new ParametrosDeNegocio(enumTipoOperacion.Insertar);
            }
            else
            {
                if (p.Nombre != nombrePais || p.ISO2 != Iso2 || p.NombreIngles != nombreEnIngles || p.Prefijo != prefijoTelefono)
                {
                    p.Nombre = nombrePais;
                    p.NombreIngles = nombreEnIngles;
                    p.ISO2 = Iso2;
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
