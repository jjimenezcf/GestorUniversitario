using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Gestor.Elementos.ModeloIu
{
    public enum LadoDeRenderizacion { izquierdo, derecho }
    public enum ModoDeTrabajo { Nuevo, Consulta, Edicion }

    public enum Aliniacion { no_definida, izquierda, centrada, derecha, justificada };

    public class IUPropiedadAttribute : Attribute
    {
        public string Etiqueta { get; set; } = "";
        public string Ayuda { get; set; } = "";
        public bool Visible { get; set; } = true;
        public bool VisibleAlCrear { get; set; } = true;
        public bool VisibleAlEditar { get; set; } = true;

        public bool VisibleAlConsultar { get; set; } = true;
        public bool EditableAlCrear { get; set; } = true;
        public bool EditableAlEditar { get; set; } = true;
        public bool Obligatorio { get; set; } = true;
        public Type Tipo { get; set; } = typeof(string);
        public short Fila { get; set; }
        public short Columna { get; set; }
        public short Posicion { get; set; } = 0;
        public string ValorPorDefecto { get; set; }
        public bool Ordenar { get; set; } = false;
        public int PosicionEnGrid { get; set; } = -1;
        public Aliniacion Alineada
        {
            get
            {
                if (Tipo == typeof(string)) return Aliniacion.izquierda;
                if (Tipo == typeof(int)) return Aliniacion.derecha;
                if (Tipo == typeof(DateTime)) return Aliniacion.centrada;
                return Aliniacion.izquierda;
            }
        }
        public int PorAnchoMnt { get; set; } = 0;
        public int PorAnchoSel { get; set; } = 0;

        public bool EsVisible(ModoDeTrabajo modo)
        {
            if (Visible)
            {
                if (modo == ModoDeTrabajo.Edicion)
                    return VisibleAlEditar;
                else
                if (modo == ModoDeTrabajo.Nuevo)
                    return VisibleAlCrear;
                if (modo == ModoDeTrabajo.Consulta)
                    return VisibleAlConsultar;
            }

            return false;
        }
        public bool EsEditable(ModoDeTrabajo modo)
        {
            if (EsVisible(modo))
            {
                if (modo == ModoDeTrabajo.Edicion)
                    return EditableAlEditar;
                else
                if (modo == ModoDeTrabajo.Nuevo)
                    return EditableAlCrear;
            }

            return false;
        }



    }

    public class IUDtoAttribute : Attribute
    {
        public short AnchoEtiqueta { get; set; } = 15;
        public short AnchoSeparador { get; set; } = 2;
        public string ClaseTypeScriptDeCreacion { get; set; } = "CrudCreacion";
        public string ClaseTypeScriptDeEdicion { get; set; } = "CrudEdicion";

        public string AlCerrar { get; set; } = "Crud.Crear.AlCerrar()";

        public string AlAceptar { get; set; } = "Crud.Crear.AlAceptar()";

    }


    public static class FiltroPor
    {
        public static string Nombre = nameof(Nombre).ToLower();
        public static string Id = nameof(Id).ToLower();
    }


    public class Elemento
    {
        [IUPropiedad(
            Etiqueta = "Id",
            Ayuda = "id del elemento",
            Tipo = typeof(int),
            Visible = false
            )
        ]
        public int Id { get; set; }


        public static IUPropiedadAttribute ObtenerAtributos(PropertyInfo propiedad)
        {
            var iEnumerableAtrb = propiedad.GetCustomAttributes(typeof(IUPropiedadAttribute));
            if (iEnumerableAtrb == null)
                Gestor.Errores.GestorDeErrores.Emitir($"No se puede definir el descriptor de creación para el tipo {propiedad.DeclaringType} por no tener definidas las etiquetas de {typeof(IUPropiedadAttribute)}");

            var listaAtrb = iEnumerableAtrb.ToList();
            if (listaAtrb.Count == 0)
                return null;
            if (listaAtrb.Count != 1)
                Gestor.Errores.GestorDeErrores.Emitir($"No se puede definir el descriptor de creación para el tipo {propiedad.DeclaringType} por tener mal definidas las etiquetas de {typeof(IUPropiedadAttribute)}");

            var atributos = (IUPropiedadAttribute)propiedad.GetCustomAttributes(typeof(IUPropiedadAttribute)).ToList()[0];
            return atributos;
        }

        public static object ValorDelAtributo(Type clase, string nombreAtributo, bool obligatorio = true)
        {
            Attribute[] atributosDeDto = System.Attribute.GetCustomAttributes(clase);

            if (atributosDeDto == null || atributosDeDto.Length == 0)
                Errores.GestorDeErrores.Emitir($"No hay definido descriptores para el dto {clase.Name}");

            foreach (Attribute propiedad in atributosDeDto)
            {
                if (propiedad is IUDtoAttribute)
                {
                    IUDtoAttribute a = (IUDtoAttribute)propiedad;
                    switch (nombreAtributo)
                    {
                        case nameof(IUDtoAttribute.ClaseTypeScriptDeCreacion):
                            return a.ClaseTypeScriptDeCreacion;

                        case nameof(IUDtoAttribute.ClaseTypeScriptDeEdicion):
                            return a.ClaseTypeScriptDeEdicion;

                        case nameof(IUDtoAttribute.AlAceptar):
                            return a.AlAceptar;

                        case nameof(IUDtoAttribute.AlCerrar):
                            return a.AlCerrar;

                        case nameof(IUDtoAttribute.AnchoEtiqueta):
                            return a.AnchoEtiqueta;

                        case nameof(IUDtoAttribute.AnchoSeparador):
                            return a.AnchoSeparador;
                    }
                    if (obligatorio)
                        throw new Exception($"Se ha solicitado el atributo {nameof(IUDtoAttribute)}.{nombreAtributo} de la clase {clase} y no está definido");
                }
                //if (propiedad is IUPropiedadAttribute)
                //{
                //    IUPropiedadAttribute p = (IUPropiedadAttribute)propiedad;
                //    switch (nombreAtributo)
                //    {
                //        case nameof(IUPropiedadAttribute.Visible):
                //            return p.Visible;
                //    }
                //    if (obligatorio)
                //        throw new Exception($"Se ha solicitado el atributo {nameof(IUPropiedadAttribute)}.{nombreAtributo} de la clase {clase} y no está definido");
                //}

            }

            return null;

        }
    }
}