namespace ServicioDeDatos.Seguridad
{
    public enum enumGrado
    {
        A, B, C, D, F
    }
    public enum enumClaseDePermiso
    {
        Tipo,
        Estado,
        Transicion,
        CentroGestor,
        Negocio,
        Elemento,
        Funcion,
        Vista,
        Menu
    }
    enum enumTipoDePermiso
    {
        Gestor = 1,
        Consultor = 2,
        Creador = 3,
        Administrador = 4,
        Acceso = 5
    }
}
