using core.modelo;
using core.Modelo.ConsultaInformacion;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace datos.Interfaz
{
    /// <summary>
    /// Interfaz para realizar consultas SQL necesarias a base de datos
    /// </summary>
    public interface IConsultaInformacionRepository
    {
        /// <summary>
        /// Método para realizar la consulta de la información básica de la persona
        /// </summary>
        /// <param name="peticion">
        /// Objeto que contiene los parámetros de la petición:
        /// - TipoId: Tipo de documento
        /// - IdUsuario: Número de documento
        /// </param>
        /// <returns>Objeto con información de la persona</returns>
        Task<Persona> DatosBasicosPersona(Peticion peticion);

        /// <summary>
        /// Método para realizar consulta de la información de los predios
        /// que posee el productor
        /// </summary>
        /// <param name="peticion">
        /// Objeto que contiene los parámetros de la petición:
        /// - TipoId: Tipo de documento
        /// - IdUsuario: Número de documento
        /// </param>
        /// <returns>Objeto con información de los predios</returns>
        Task<IReadOnlyList<Predio>> DatosPredios(Peticion peticion);

        /// <summary>
        /// Método para realizar consulta de la información de las asociaciones
        /// a las cuales se encuentra vinculado el productor
        /// </summary>
        /// <param name="peticion">
        /// Objeto que contiene los parámetros de la petición:
        /// - TipoId: Tipo de documento
        /// - IdUsuario: Número de documento
        /// </param>
        /// <returns>Objeto con información de las asociaciones</returns>
        Task<IReadOnlyList<Asociacion>> DatosAsociaciones(Peticion peticion);
    }
}
