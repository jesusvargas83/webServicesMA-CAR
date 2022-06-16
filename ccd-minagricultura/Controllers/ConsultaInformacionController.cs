using core.modelo;
using core.Modelo.ConsultaInformacion;
using logica.Interfaz;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace ccd_minagricultura.Controllers
{
    /// <summary>
    /// Clase para endpoint de consulta de información
    /// </summary>
    [ApiController]
    [Produces("application/json")]
    public class ConsultaInformacionController : ControllerBase
    {
        private const string ERRORMESSAGE = "Error durante ejecución de consulta de información.";

        private readonly ILogger<ConsultaInformacionController> logger;
        private readonly IConsultaInformacion ConsultaInformacionLogica;

        /// <summary>
        /// Constructor de la clase que recibe la inyección de dependencias
        /// </summary>
        /// <param name="consultaInformacion">Objeto para realizar la lógica de consulta de información</param>
        /// <param name="logger">Objeto para realizar el logging</param>
        public ConsultaInformacionController(IConsultaInformacion consultaInformacion, ILogger<ConsultaInformacionController> logger)
        {
            this.ConsultaInformacionLogica = consultaInformacion;
            this.logger = logger;
        }

        /// <summary>
        /// Consulta de información del ciudadano beneficiario
        /// </summary>
        /// <remarks>
        /// Obtiene información básica de la persona y los beneficios obtenidos
        /// </remarks>
        /// <param name="tipoId">Tipo de documento</param>
        /// <param name="idUsuario">Número de documento</param>
        /// <returns>Información básica y beneficio(s) obtenido(s)</returns>
        /// <response code="200">Información de beneficiario</response>
        /// <response code="400">Mal request</response>
        /// <response code="500">Error interno</response>
        [HttpGet]
        [Route("/servicio/{tipoId}/{idUsuario}")]
        [ProducesResponseType(typeof(Respuesta), 200)]
        [ProducesResponseType(typeof(Respuesta), 400)]
        [ProducesResponseType(typeof(Respuesta), 500)]
        public async Task<IActionResult> ConsultaInformacion([FromRoute] string tipoId, [FromRoute] string idUsuario)
        {
            Peticion peticion = new Peticion
            {
                TipoId = tipoId,
                IdUsuario = idUsuario
            };

            Respuesta respuesta = new Respuesta();

            if (TryValidateModel(peticion))
            {
                try
                {
                    respuesta = await ConsultaInformacionLogica.ConsultaInformacionServicio(peticion);
                }
                catch (Exception exception)
                {
                    logger.LogError(ERRORMESSAGE + Environment.NewLine + exception);
                    return StatusCode(StatusCodes.Status500InternalServerError, respuesta);
                }

                HttpContext.Response.ContentType = "application/json";
                return new JsonResult(respuesta);
            }
            return BadRequest(respuesta);
        }
    }
}
