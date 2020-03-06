using System.Collections.Generic;
using Gestor.Elementos.Entorno;
using Gestor.Elementos.Permiso;
using UtilidadesParaIu;

namespace MVCSistemaDeElementos.Descriptores
{
    public class CrudCurso : DescriptorDeCrud<GrupoDto>
    {
        public CrudCurso(ModoDescriptor modo)
        : base(controlador: "Cursos", vista: "MantenimientoCurso", titulo: "Mantenimiento de cursos", modo: modo)
        {            
            if (modo == ModoDescriptor.Mantenimiento)
            {
                var descEstu = new CrudUsuario(ModoDescriptor.Seleccion);
                new Selector<UsuarioDto>(padre: new Bloque(Filtro, "Específico", new Dimension(1, 2)),
                                              etiqueta: "Usuario",
                                              propiedad: GrupoPor.EstudianteInscrito,
                                              ayuda: "Seleccionar usuario",
                                              posicion: new Posicion() { fila = 0, columna = 0 },
                                              paraFiltrar: nameof(UsuarioDto.Id),
                                              paraMostrar: nameof(UsuarioDto.Apellido),
                                              descriptor: descEstu,
                                              propiedadDondeMapear: UsuariosPor.NombreCompleto.ToString());
            }

            DefinirVistaDeCreacion(accion: "IraCrearCurso", textoMenu: "Crear curso");

            DefinirColumnasDelGrid();

        }

        protected override void DefinirColumnasDelGrid()
        {
            base.DefinirColumnasDelGrid();
            Grid.Columnas.Add(new ColumnaDelGrid() { Nombre = nameof(GrupoDto.Id), Visible = false, Tipo = typeof(int) });
            Grid.Columnas.Add(new ColumnaDelGrid() { Nombre = nameof(GrupoDto.Titulo), Titulo = "Título", Ordenar = false });
            Grid.Columnas.Add(new ColumnaDelGrid() { Nombre = nameof(GrupoDto.Creditos), Titulo = "Créditos", Tipo = typeof(int) });
        }

        public override void MapearElementosAlGrid(IEnumerable<GrupoDto> elementos)
        {
            base.MapearElementosAlGrid(elementos);
            foreach (var curso in elementos)
            {
                var fila = new FilaDelGrid();
                foreach (ColumnaDelGrid columna in Grid.Columnas)
                {
                    CeldaDelGrid celda = new CeldaDelGrid(columna);
                    if (columna.Nombre == nameof(GrupoDto.Id))
                        celda.Valor = curso.Id.ToString();
                    else
                    if (columna.Nombre == nameof(GrupoDto.Titulo))
                        celda.Valor = curso.Titulo;
                    else
                    if (columna.Nombre == nameof(GrupoDto.Creditos))
                        celda.Valor = curso.Creditos;
                    fila.Celdas.Add(celda);
                }
                Grid.Filas.Add(fila);
            }
        }

    }
}
