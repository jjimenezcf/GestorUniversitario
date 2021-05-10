using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ServicioDeDatos.Entorno;
using ServicioDeDatos;
using ModeloDeDto.Entorno;
using GestorDeElementos;
using ModeloDeDto;
using Utilidades;
using System.Linq.Dynamic.Core;
using System;
using System.IO;
using System.Reflection;

namespace GestoresDeNegocio.Entorno
{

    public class GestorDeVariables : GestorDeElementos<ContextoSe, VariableDtm, VariableDto>
    {
        public static readonly string RutaBase = @"..\SistemaDeElementos\wwwroot";
        public static readonly string RutaDeDescarga = $@"{RutaBase}\Archivos";
        public static readonly string RutaDeExportaciones = $@"{RutaBase}\Exportaciones";
        //public static readonly string RutaDeBinarios = $@"..\{RutaBase}\bin";

        public class MapearVariables : Profile
        {
            public MapearVariables()
            {
                CreateMap<VariableDtm, VariableDto>();
                CreateMap<VariableDto, VariableDtm>();
            }
        }

        public GestorDeVariables(ContextoSe contexto, IMapper mapeador)
            : base(contexto, mapeador)
        {
        }
        internal static GestorDeVariables Gestor(ContextoSe contexto, IMapper mapeador)
        {
            return new GestorDeVariables(contexto, mapeador);
        }

        protected override void DespuesDePersistir(VariableDtm registro, ParametrosDeNegocio parametros)
        {
            base.DespuesDePersistir(registro, parametros);
            if (parametros.Operacion == enumTipoOperacion.Modificar || parametros.Operacion == enumTipoOperacion.Eliminar)
                CacheDeVariable.BorrarCache(registro.Nombre);
        }

        internal static VariableDtm LeerVariable(ContextoSe contextoSe, string variable, bool emitirErrorSiNoExiste)
        {
            var indice = $"{nameof(VariableDtm.Nombre)}-{variable}";
            var cache = ServicioDeCaches.Obtener(typeof(VariableDtm).FullName);
            if (cache.ContainsKey(indice))
                return (VariableDtm)cache[indice];

            var gestor = Gestor(contextoSe, contextoSe.Mapeador);
            var registro = gestor.LeerRegistroCacheado(nameof(VariableDtm.Nombre), variable, emitirErrorSiNoExiste, true);
            return registro;
        }

        private static VariableDtm CrearVariable(ContextoSe contexto, string variable, string descripcion, string valor)
        {
            var v = new VariableDtm();
            v.Nombre = variable;
            v.Valor = valor;
            v.Descripcion = descripcion;
            var gestor = Gestor(contexto, contexto.Mapeador);
            v = gestor.PersistirRegistro(v, new ParametrosDeNegocio(enumTipoOperacion.Insertar));
            return v;
        }

        public static VariableDtm VariableDeRutaDeExportaciones(ContextoSe contexto)
        {
            var ruta = LeerVariable(contexto, Variable.CFG_Ruta_De_Exportaciones, false);
            
            if (ruta == null)
                ruta = CrearVariable(contexto, Variable.CFG_Ruta_De_Exportaciones, "Directorio donde se genera la documentación a exportar", RutaDeExportaciones);

            if (!Directory.Exists(ruta.Valor))
                Directory.CreateDirectory(ruta.Valor);

            return ruta;
        }

        public static VariableDtm VariableDeRutaDeBinarios(ContextoSe contexto)
        {
            var ruta = LeerVariable(contexto, Variable.CFG_Ruta_De_Binarios, false);
            if (ruta == null)
            {
                var rutaBinarios = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
                //if (!Directory.Exists(RutaDeBinarios))
                //    Directory.CreateDirectory(RutaDeBinarios);
                //var rutaAbsoluta = Path.GetFullPath(RutaDeBinarios);
                ruta = CrearVariable(contexto, Variable.CFG_Ruta_De_Binarios, "Directorio donde se genera los binarios del sistema", rutaBinarios);
            }

            return ruta;
        }

    }
}
