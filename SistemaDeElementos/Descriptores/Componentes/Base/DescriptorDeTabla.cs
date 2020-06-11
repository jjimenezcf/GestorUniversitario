using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Gestor.Elementos.ModeloIu;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    public class DescriptorControl
    {
        internal PropertyInfo Descriptor { get; set; }

        internal string propiedad => Descriptor.Name.ToLower();

        public DescriptorDeFila Fila { get; set; }
        public short NumeroColumna { get; set; }

        internal string IdHtmlContenedor => $"{Fila.Tabla.IdHtml}-{Fila.NumeroFila}-{NumeroColumna}-crtl";
        internal string IdHtml => $"{Fila.Tabla.IdHtml}-{propiedad}";

        internal IUPropiedadAttribute atributos => Elemento.ObtenerAtributos(Descriptor);
    }

    public class DescriptorDeColumna
    {
        private Dictionary<short, DescriptorControl> Controles = new Dictionary<short, DescriptorControl>();

        public DescriptorDeFila Fila { get; private set; }
        public DescriptorDeTabla Tabla => Fila.Tabla;

        public short NumeroColumna { get; private set; }

        public short NumeroDeControles { get; private set; } = 0;
        public short PosicionMaxima { get; private set; } = 0;

        public short NumeroDeEtiquetasVisibles
        {
            get
            {
                short numero = 0;
                for (short i = 0; i <= PosicionMaxima; i++)
                {
                    var control = ObtenerControlEnLaPosicion(i);
                    if (control != null && control.atributos.EsVisible(Tabla.ModoDeTrabajo) && !control.atributos.Etiqueta.IsNullOrEmpty())
                        numero = (short)(numero + 1);
                }
                return numero;
            }
        }
        public short NumeroControlesVisibles
        {
            get
            {
                short numero = 0;
                for (short i = 0; i <= PosicionMaxima; i++)
                {
                    var control = ObtenerControlEnLaPosicion(i);

                    if (control != null && control.atributos.EsVisible(Tabla.ModoDeTrabajo))
                        numero = (short)(numero + 1);
                }
                return numero;
            }
        }
        public int ColSpan => Tabla.NumeroDeColumnas - Fila.NumeroDeColumnas + 1;

        public DescriptorDeColumna(DescriptorDeFila fila, short indice)
        {
            Fila = fila;
            NumeroColumna = indice;
        }

        public void AnadirControl(short pos, PropertyInfo descriptor)
        {
            if (!Controles.ContainsKey(pos))
            {
                Controles[pos] = new DescriptorControl { Descriptor = descriptor, Fila = Fila, NumeroColumna  = NumeroColumna };

                if (PosicionMaxima < pos)
                    PosicionMaxima = pos;

                NumeroDeControles = (short)(NumeroDeControles + 1);
            }
            else
                AnadirControl((short)(pos + 1), descriptor);

        }

        public DescriptorControl ObtenerControlEnLaPosicion(short pos)
        {
            if (Controles.ContainsKey(pos))
                return Controles[pos];

            return null;
        }

    }

    public class DescriptorDeFila
    {
        private Dictionary<short, DescriptorDeColumna> Columnas = new Dictionary<short, DescriptorDeColumna>();

        public string IdHtml => $"{Tabla.IdHtml}-{NumeroFila}".ToLower();

        public short NumeroFila { get; set; }

        public DescriptorDeTabla Tabla { get; private set; }

        public DescriptorDeFila(DescriptorDeTabla descriptorDeTabla, short indice)
        {
            Tabla = descriptorDeTabla;
            NumeroFila = indice;
        }

        public short NumeroDeColumnas { get; private set; } = 0;

        private bool ColumnaDefinida(short indice)
        {
            return Columnas.ContainsKey(indice);
        }
        
        private void DefinirColumna(short indice)
        {
            var celda = new DescriptorDeColumna(this, indice);
            Columnas[indice] = celda;
            if (NumeroDeColumnas <= indice)
            {
                NumeroDeColumnas = (short)(indice + 1);
            }
        }

        internal DescriptorDeColumna ObtenerColumna(short columna)
        {
            if (!ColumnaDefinida(columna))
                DefinirColumna(columna);

            return Columnas[columna];
        }
    }

    public class DescriptorDeTabla
    {
        private Dictionary<short, DescriptorDeFila> Filas = new Dictionary<short, DescriptorDeFila>();
        private Type _Tipo;
        public ModoDeTrabajo ModoDeTrabajo { get; private set; }
        public short NumeroDeFilas { get; private set; } = 0;

        public short NumeroDeColumnas { get; private set; } = 0;

        public string Controlador { get; private set; }

        public string IdHtml => $"table-{_Tipo.Name}-{ModoDeTrabajo}".ToLower();

        public DescriptorDeTabla(Type tipo, ModoDeTrabajo modoDeTrabajo, string controlador)
        {
            _Tipo = tipo;
            Controlador = controlador;
            ModoDeTrabajo = modoDeTrabajo;
            var propiedades = tipo.GetProperties();
            foreach (var p in propiedades)
                AnadirPropiedad(p);
        }
        private void DefinirFila(short indice)
        {
            var fila = new DescriptorDeFila(this,indice);
            Filas[indice] = fila;

            if (NumeroDeFilas <= indice)
            {
                NumeroDeFilas = (short)(indice + 1);
            }

        }


        private bool FilaDefinida(short indice)
        {
            return Filas.ContainsKey(indice);
        }
        public DescriptorDeFila ObtenerFila(short fila)
        {
            if (!FilaDefinida(fila))
                DefinirFila(fila);

            return Filas[fila];
        }

        private void AnadirPropiedad(PropertyInfo propiedad)
        {
            IUPropiedadAttribute atributos = Elemento.ObtenerAtributos(propiedad);
            if (atributos != null)
            {
                var descriptorColumna = ObtenerColumna(atributos.Fila, atributos.Columna);

                if (NumeroDeColumnas <= atributos.Columna)
                    NumeroDeColumnas = (short)(atributos.Columna + 1);

                descriptorColumna.AnadirControl(atributos.Posicion, propiedad);
            }
        }

        public DescriptorDeColumna ObtenerColumna(short fila, short columna)
        {
            var descriptorFila = ObtenerFila(fila);
            return descriptorFila.ObtenerColumna(columna);
        }
    }

}
