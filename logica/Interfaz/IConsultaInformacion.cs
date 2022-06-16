using core.modelo;
using core.Modelo.ConsultaInformacion;
using System.Threading.Tasks;

namespace logica.Interfaz
{
    /// <summary>
    /// Interfaz para la lógica de la consulta de información
    /// </summary>
    public interface IConsultaInformacion
    {
        /// <summary>
        /// Método en el cual se tiene el punto de entrada para realizar
        /// la lógica de la consulta de información
        /// </summary>
        /// <param name="peticion">
        /// Objeto que contiene los parámetros de la petición:
        /// - TipoId: Tipo de documento
        /// - IdUsuario: Número de documento
        /// </param>
        /// <returns>Retorna respuesta de la consulta de información</returns>
        Task<Respuesta> ConsultaInformacionServicio(Peticion peticion);
    }
}
