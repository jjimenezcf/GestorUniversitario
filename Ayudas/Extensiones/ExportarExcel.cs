using System.Collections.Generic;
using System.IO;
using System.Linq;
using OfficeOpenXml;
using OfficeOpenXml.Table;

namespace Utilidades
{
    public class HojaExcel<TRegistro>
    {
        public string Nombre { get; }
        public List<TRegistro> Filas { get; }

        public int Columnas => typeof(TRegistro).GetProperties().Count();

        public List<string> Cabecera { get; } = new List<string>();

        public bool HayCabecera => Cabecera.Count > 0;

        public HojaExcel(string nombre)
        {
            Nombre = nombre;
        }

        public HojaExcel(List<string> cabecera, List<TRegistro> registros)
        : this(registros)
        {
            Cabecera = cabecera;
        }

        public HojaExcel(List<TRegistro> registros)
        : this(typeof(TRegistro).Name)
        {
            Filas = registros;
        }
    }

    public class LibroExcel<TRegistro>
    {
        public string Nombre { get; }

        public List<HojaExcel<TRegistro>> hojas = new List<HojaExcel<TRegistro>>();
        public LibroExcel(string nombre)
        {
            Nombre = nombre;
        }
    }


    public class ExportarExcel<T> 
    {
        public string Fichero { get; }
        LibroExcel<T> Libro { get;} 

        public ExportarExcel(string fichero, List<T> registros)
        {
            Fichero = fichero;
            var hoja = new HojaExcel<T>(registros);
            Libro = new LibroExcel<T>(Path.GetFileNameWithoutExtension(fichero));
            Libro.hojas.Add(hoja);
        }

        public void Exportar()
        {
            using (var libroExcel = new ExcelPackage())
            {
                foreach (var hoja in Libro.hojas)
                {
                    var hojaExcel = libroExcel.Workbook.Worksheets.Add(hoja.Nombre);
                    var fila = 1;
                    if (hoja.HayCabecera)
                    {
                        hojaExcel.Cells[$"A1"].LoadFromCollection(hoja.Cabecera, PrintHeaders: false);
                        fila = 2;
                    }
                    hojaExcel.Cells[$"A{fila}"].LoadFromCollection(hoja.Filas, PrintHeaders: true);
                    for (var col = 1; col < hoja.Filas.Count + 1; col++)
                    {
                        hojaExcel.Column(col).AutoFit();
                    }

                    var tabla = hojaExcel.Tables.Add(new ExcelAddressBase(fromRow: 1, fromCol: 1, toRow: hoja.Filas.Count + 1, toColumn: hoja.Columnas), hoja.Nombre);
                    tabla.ShowHeader = true;
                    tabla.TableStyle = TableStyles.Light6;
                    tabla.ShowTotal = true;

                }

                libroExcel.SaveAs(new FileInfo(Fichero));
            }
        }
    }

}
