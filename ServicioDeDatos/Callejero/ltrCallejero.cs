namespace ServicioDeDatos.Callejero
{
    public static class ltrCallejero
    {
        public static string iso2Pais = nameof(iso2Pais);
        public static string CalleIso2 = $"{nameof(CalleDtm.Municipio)}.{nameof(MunicipioDtm.Provincia)}.{nameof(ProvinciaDtm.Pais)}.{nameof(PaisDtm.ISO2)}";

        public static string codigoProvincia = nameof(codigoProvincia);
        public static string CalleCodigoProvincia = $"{nameof(CalleDtm.Municipio)}.{nameof(MunicipioDtm.Provincia)}.{nameof(ProvinciaDtm.Codigo)}";

        public static string codigoMunicipio = nameof(codigoMunicipio);
        public static string CalleCodigoMunicipio = $"{nameof(CalleDtm.Municipio)}.{nameof(ProvinciaDtm.Codigo)}";

        public static string codigoCalle = nameof(codigoCalle);
        public static string CalleCodigo = $"{nameof(ProvinciaDtm.Codigo)}";

        public static string nombreProvincia = nameof(nombreProvincia);
        public static string CalleNombreProvincia = $"{nameof(CalleDtm.Municipio)}.{nameof(MunicipioDtm.Provincia)}.{nameof(ProvinciaDtm.Nombre)}";

        public static string nombreMunicipio = nameof(nombreMunicipio);
        public static string CalleNombreMunicipio = $"{nameof(CalleDtm.Municipio)}.{nameof(ProvinciaDtm.Nombre)}";

        public static string nombreCalle = nameof(nombreCalle);
        public static string CalleNombre = $"{nameof(ProvinciaDtm.Nombre)}";
    }
}
