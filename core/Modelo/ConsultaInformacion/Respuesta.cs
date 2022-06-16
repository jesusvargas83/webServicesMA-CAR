using System.Collections.Generic;

namespace core.Modelo.ConsultaInformacion
{
    public class Respuesta
    {
        public Respuesta()
        {
            DatoConsultado = new List<DatoConsultado>();
            UrlDescarga = string.Empty;
        }

        public IList<DatoConsultado> DatoConsultado { get; set; }
        public string UrlDescarga { get; set; }
    }
}
