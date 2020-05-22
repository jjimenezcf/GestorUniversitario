namespace ServicioDeDatos.Elemento
{

    public class CatalogoDelSe : Registro
    {
        public string Catalogo { get; set; }
        public string Esquema { get; set; }
        public string Tabla { get; set; }
    }



    public class ExisteTabla : ConsultaSql
    {
        public bool Existe => Leidos == 0 ? false : (int)Registros[0][0] == 1;


        public ExisteTabla(ContextoSe contexto, string tabla)
        : base(contexto, $"SELECT 1 FROM sysobjects WHERE type = 'U' AND name = '{tabla}'")
        {
            Ejecutar();
        }
    }



}
