using System;
using System.Globalization;

namespace core.Util
{
    public static class Fecha
    {
        private const string FORMATOFECHA = "yyyy-MM-dd";

        public static string FechaAnioMesDia(string fecha)
        {
            try
            {
                DateTime fechaToDateTime = Convert.ToDateTime(fecha, CultureInfo.InvariantCulture);

                return fechaToDateTime.ToString(FORMATOFECHA);
            }
            catch (Exception)
            {
                // No tendría porqué ocurrir una excepción ya que se observa en base de datos que las fechas
                // se han definido como tipo fecha y vendría un valor válido pero se coloca try por si llega
                // un valor nulo o hay alguna sorpresa en el camino

                // Se controla retornando un string vacío
                return string.Empty;
            }
        }
    }
}
