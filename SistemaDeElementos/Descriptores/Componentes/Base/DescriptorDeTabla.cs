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

        internal IUCreacionAttribute atributos => ObtenerAtributos(Descriptor);

        public static IUCreacionAttribute ObtenerAtributos(PropertyInfo propiedad)
        {
            var iEnumerableAtrb = propiedad.GetCustomAttributes(typeof(IUCreacionAttribute));
            if (iEnumerableAtrb == null)
                Gestor.Errores.GestorDeErrores.Emitir($"No se puede definir el descriptor de creación para el tipo {propiedad.DeclaringType} por no tener definidas las etiquetas de {typeof(IUCreacionAttribute)}");

            var listaAtrb = iEnumerableAtrb.ToList();
            if (listaAtrb.Count == 0)
                return null;
            if (listaAtrb.Count != 1)
                Gestor.Errores.GestorDeErrores.Emitir($"No se puede definir el descriptor de creación para el tipo {propiedad.DeclaringType} por tener mal definidas las etiquetas de {typeof(IUCreacionAttribute)}");

            var atributos = (IUCreacionAttribute)propiedad.GetCustomAttributes(typeof(IUCreacionAttribute)).ToList()[0];
            return atributos;
        }
    }

    internal class DescriptorDeColumna
    {
        private Dictionary<short, DescriptorControl> Controles = new Dictionary<short, DescriptorControl>();

        public short Count { get; private set; } = 0;

        public string Etiqueta
        {
            get
            {
                for (short i = 0; i < Count; i++)
                {
                    var control = ObtenerControl(i);
                    if (control != null && control.atributos.Visible)
                    {
                        var atributos = DescriptorControl.ObtenerAtributos(control.Descriptor);
                        return atributos.Etiqueta;
                    }
                }

                return "";
            }
        }

        public void AnadirControl(short pos, PropertyInfo descriptor)
        {
            if (!Controles.ContainsKey(pos))
                Controles[pos] = new DescriptorControl { Descriptor = descriptor };
            else
                AnadirControl((short)(pos + 1), descriptor);

            if (Count <= pos)
                Count = (short)(Count + 1);

        }

        public DescriptorControl ObtenerControl(short pos)
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

        private short _numeroDeColumnas = 0;
        public short NumeroDeColumnas
        {
            get
            {
                if (_numeroDeColumnas == 0)
                    for (short i = 0; i < NumeroDeFilas; i++)
                    {
                        if (_numeroDeColumnas < ObtenerFila(i).NumeroDeColumnas)
                            _numeroDeColumnas = ObtenerFila(i).NumeroDeColumnas;
                    }
                return _numeroDeColumnas;
            }
        }

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
            _numeroDeColumnas = 0;

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
            IUCreacionAttribute atributos = DescriptorControl.ObtenerAtributos(propiedad);
            if (atributos != null)
            {
                var descriptorColumna = ObtenerColumna(atributos.Fila, atributos.Columna);
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
