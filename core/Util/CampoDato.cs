using System.Collections.Generic;

namespace core.Util
{
    public static class CampoDato
    {
        private const string SUFIJOPREDIO = " Predio {0}";
        private const string SUFIJOASOCIACION = " Asociación {0}";

        private const string TIPOIDENTIFICACION = "Tipo Identificación";
        private const string NUMERODOCUMENTO = "Número de Identificación";
        private const string FECHAEXPEDICION = "Fecha de expedición de documento identidad";
        private const string PRIMERNOMBRE = "Primer Nombre";
        private const string SEGUNDONOMBRE = "Segundo Nombre";
        private const string PRIMERAPELLIDO = "Primer Apellido";
        private const string SEGUNDOAPELLIDO = "Segundo Apellido";
        private const string SEXO = "Sexo";
        private const string TELEFONO = "Teléfono del contacto";
        private const string DIRECCION = "Dirección de contacto";
        private const string FECHANACIMIENTO = "Fecha de nacimiento";
        private const string CORREOELECTRONICO = "Correo electrónico";
        private const string ESTADOCIVIL = "Estado civil";
        private const string CABEZAFAMILIA = "Cabeza de familia";
        private const string PERTENENCIAETNICA = "Pertenencia étnica";
        private const string CARNET = "Carnet";

        private const string DEPARTAMENTO = "Departamento" + SUFIJOPREDIO;
        private const string MUNICIPIO = "Municipio" + SUFIJOPREDIO;
        private const string VEREDA = "Vereda" + SUFIJOPREDIO;
        private const string UBICACION = "Ubicación" + SUFIJOPREDIO;
        private const string CEDULACATASTRAL = "Cédula catastral" + SUFIJOPREDIO;
        private const string MATRICULAINMOBILIARIA = "Matrícula inmobiliaria" + SUFIJOPREDIO;
        private const string FORMATENENCIA = "Forma de tenencia" + SUFIJOPREDIO;
        private const string PRODUCTO = "Producto" + SUFIJOPREDIO;

        private const string NITASOCIACION = "NIT" + SUFIJOASOCIACION;

        private static readonly Dictionary<string, string> InformacionBasica = new Dictionary<string, string>()
        {
            { nameof(TIPOIDENTIFICACION), TIPOIDENTIFICACION },
            { nameof(NUMERODOCUMENTO), NUMERODOCUMENTO },
            { nameof(FECHAEXPEDICION), FECHAEXPEDICION },
            { nameof(PRIMERNOMBRE), PRIMERNOMBRE },
            { nameof(SEGUNDONOMBRE), SEGUNDONOMBRE },
            { nameof(PRIMERAPELLIDO), PRIMERAPELLIDO },
            { nameof(SEGUNDOAPELLIDO), SEGUNDOAPELLIDO },
            { nameof(SEXO), SEXO },
            { nameof(TELEFONO), TELEFONO },
            { nameof(DIRECCION), DIRECCION },
            { nameof(FECHANACIMIENTO), FECHANACIMIENTO },
            { nameof(CORREOELECTRONICO), CORREOELECTRONICO },
            { nameof(ESTADOCIVIL), ESTADOCIVIL },
            { nameof(CABEZAFAMILIA), CABEZAFAMILIA },
            { nameof(PERTENENCIAETNICA), PERTENENCIAETNICA },
            { nameof(CARNET), CARNET }
        };

        private static readonly Dictionary<string, string> InformacionPredio = new Dictionary<string, string>()
        {
            { nameof(DEPARTAMENTO), DEPARTAMENTO },
            { nameof(MUNICIPIO), MUNICIPIO },
            { nameof(VEREDA), VEREDA },
            { nameof(UBICACION), UBICACION },
            { nameof(CEDULACATASTRAL), CEDULACATASTRAL },
            { nameof(MATRICULAINMOBILIARIA), MATRICULAINMOBILIARIA },
            { nameof(FORMATENENCIA), FORMATENENCIA },
            { nameof(PRODUCTO), PRODUCTO }
        };

        public static Dictionary<string, string> GetInformacionBasica()
        {
            return InformacionBasica;
        }

        public static Dictionary<string, string> GetInformacionPredio()
        {
            return InformacionPredio;
        }

        public static string GetNitAsociacion()
        {
            return NITASOCIACION;
        }
    }
}
