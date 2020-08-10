namespace Gestor.Elementos.Seguridad
{
    public enum Grado
    {
        A, B, C, D, F
    }
    enum ClaseDePermiso
    {
        Tipo = 1,
        Estado = 2,
        Transicion = 3,
        CentroGestor = 4,
        Negocio = 5,
        Elemento = 6,
        Funcion = 7,
        Vista = 8,
        Menu = 9
    }
    enum TipoDePermiso
    {
        Gestor = 1,
        Consultor = 2,
        Creador = 3,
        Administrador = 4,
        Acceso = 5
    }
}
