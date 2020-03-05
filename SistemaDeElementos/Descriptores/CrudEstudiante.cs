using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gestor.Elementos.ModeloIu;
using Gestor.Elementos.Usuario.ModeloIu;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudUsuario : DescriptorDeCrud<UsuarioDto>
    {
        public CrudUsuario(ModoDescriptor modo)
        : base(controlador: "Usuarios", vista: "MantenimientoUsuario", titulo: "Mantenimiento de usuarios", modo: modo)
        {
            if (modo == ModoDescriptor.Mantenimiento)
                new Selector<ElementoCurso>(padre: new Bloque(Filtro, titulo: "Específico", dimension: new Dimension(1, 2)),
                                        etiqueta: "Curso",
                                        propiedad: UsuariosPor.CursosInscrito,
                                        ayuda: "Seleccionar curso",
                                        posicion: new Posicion() { fila = 0, columna = 0 },
                                        paraFiltrar: nameof(ElementoCurso.Id),
                                        paraMostrar: nameof(ElementoCurso.Titulo),
                                        descriptor: new CrudCurso(ModoDescriptor.Seleccion),
                                        propiedadDondeMapear: FiltroPor.Nombre.ToString());

            DefinirVistaDeCreacion(accion: "IraCrearUsuario", textoMenu: "Crear usuario");

            BuscarControlEnFiltro(FiltroPor.Nombre).CambiarAtributos(UsuariosPor.NombreCompleto, "Buscar por 'apellido, nombre'");            

            DefinirColumnasDelGrid();
        }


        protected override void DefinirColumnasDelGrid()
        {
            base.DefinirColumnasDelGrid();

            var columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(UsuarioDto.Id), Tipo = typeof(int), Visible = false };
            Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(UsuarioDto.Login) };
            Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(UsuarioDto.Apellido), Ordenar = true, Ruta = "Usuarios", Accion = "IraMantenimientoUsuario" };
            Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid { Nombre = nameof(UsuarioDto.Nombre) };
            Grid.Columnas.Add(columnaDelGrid);

            columnaDelGrid = new ColumnaDelGrid
            {
                Nombre = nameof(UsuarioDto.Alta),
                Tipo = typeof(DateTime),
                Alineada = Aliniacion.centrada,
                Ordenar = true,
                Ruta = "Usuarios",
                Accion = "IraMantenimientoUsuario"
            };
            Grid.Columnas.Add(columnaDelGrid);
        }

        public override void MapearElementosAlGrid(IEnumerable<UsuarioDto> elementos)
        {
            base.MapearElementosAlGrid(elementos);
            foreach (var usuario in elementos)
            {
                var fila = new FilaDelGrid();
                foreach (ColumnaDelGrid columna in Grid.Columnas)
                {
                    CeldaDelGrid celda = new CeldaDelGrid(columna);
                    if (columna.Nombre == nameof(UsuarioDto.Id))
                        celda.Valor = usuario.Id.ToString();
                    else
                    if (columna.Nombre == nameof(UsuarioDto.Login))
                        celda.Valor = usuario.Login.ToString();
                    else
                    if (columna.Nombre == nameof(UsuarioDto.Apellido))
                        celda.Valor = usuario.Apellido;
                    else
                    if (columna.Nombre == nameof(UsuarioDto.Nombre))
                        celda.Valor = usuario.Nombre.ToString();
                    else
                    if (columna.Nombre == nameof(UsuarioDto.Alta))
                        celda.Valor = usuario.Alta.ToString();

                    fila.Celdas.Add(celda);
                }
                Grid.Filas.Add(fila);
            }
        }

    }
}
