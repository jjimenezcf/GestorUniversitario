using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Gestor.Elementos.ModeloIu;
using Utilidades;

namespace MVCSistemaDeElementos.Descriptores
{
    internal class DescriptorControl
    {
        internal PropertyInfo Descriptor { get; set; }

        internal IUPropiedadAttribute atributos => Elemento.ObtenerAtributos(Descriptor);
    }

    internal class DescriptorDeColumna
    {
        private Dictionary<short, DescriptorControl> Controles = new Dictionary<short, DescriptorControl>();

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
                    if (control != null && control.atributos.Visible && !control.atributos.Etiqueta.IsNullOrEmpty())
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
                    if (control != null && control.atributos.Visible)
                        numero = (short)(numero + 1);
                }
                return numero;
            }
        }


        public void AnadirControl(short pos, PropertyInfo descriptor)
        {
            if (!Controles.ContainsKey(pos))
            {
                Controles[pos] = new DescriptorControl { Descriptor = descriptor };
                
                if (PosicionMaxima < pos)
                    PosicionMaxima = pos;

                NumeroDeControles = (short)(NumeroDeControles + 1);
            }
            else
                AnadirControl((short)(pos+1), descriptor);

        }

        public DescriptorControl ObtenerControlEnLaPosicion(short pos)
        {
            if (Controles.ContainsKey(pos))
                return Controles[pos];

            return null;
        }

    }

    internal class DescriptorDeFila
    {
        private Dictionary<short, DescriptorDeColumna> Columnas = new Dictionary<short, DescriptorDeColumna>();

        public string IdHtml => $"{_DescriptorDeTabla.IdHtml}_tr".ToLower();

        private DescriptorDeTabla _DescriptorDeTabla;

        public DescriptorDeFila(DescriptorDeTabla descriptorDeTabla)
        {
            this._DescriptorDeTabla = descriptorDeTabla;
        }

        public short NumeroDeColumnas { get; private set; } = 0;

        private bool ColumnaDefinida(short indice)
        {
            return Columnas.ContainsKey(indice);
        }

        private void DefinirColumna(short indice)
        {
            var celda = new DescriptorDeColumna();
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

    class DescriptorDeTabla
    {
        private Dictionary<short, DescriptorDeFila> Filas = new Dictionary<short, DescriptorDeFila>();
        private Type _Tipo;
        public short NumeroDeFilas { get; private set; } = 0;

        public short NumeroDeColumnas { get; private set; } = 0;

        public string IdHtml => $"id_table_{_Tipo.Name}".ToLower();

        public DescriptorDeTabla(Type tipo)
        {
            _Tipo = tipo;
            var propiedades = tipo.GetProperties();
            foreach (var p in propiedades)
                AnadirPropiedad(p);
        }
        private void DefinirFila(short indice)
        {
            var fila = new DescriptorDeFila(this);
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

        private DescriptorDeColumna ObtenerColumna(short fila, short columna)
        {
            var descriptorFila = ObtenerFila(fila);
            return descriptorFila.ObtenerColumna(columna);
        }
    }
}
