using core.modelo;
using core.Modelo.ConsultaInformacion;
using core.Util;
using datos.Interfaz;
using logica.Interfaz;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace logica.Implementacion
{
    /// <summary>
    /// Clase que realiza implementación de lógica de consulta de información
    /// </summary>
    public class ConsultaInformacion : IConsultaInformacion
    {
        private const string ERRORMESSAGE = "Error durante ejecución de consultas y creación de respuesta.";
        private const string TEXTOFECHA = "Fecha";

        private const string ARCHIVOBASE64 = "archivoBase64";
        private const string TIPOARCHIVO = "PDF";
        private const string NOMBREARCHIVO = "Mi registro rural";
        private const string DESCRIPCIONARCHIVO = "Mi registro rural";

        private readonly ILogger<ConsultaInformacion> logger;
        private readonly IConsultaInformacionRepository ConsultaInformacionRepository;

        /// <summary>
        /// Constructor de la clase que recibe la inyección de dependencias
        /// </summary>
        /// <param name="consultaInformacionRepository">Objeto para realizar consultas SQL necesarias a base de datos</param>
        /// <param name="logger">Objeto para realizar el logging</param>
        public ConsultaInformacion(IConsultaInformacionRepository consultaInformacionRepository, ILogger<ConsultaInformacion> logger)
        {
            this.ConsultaInformacionRepository = consultaInformacionRepository;
            this.logger = logger;
        }

        public async Task<Respuesta> ConsultaInformacionServicio(Peticion peticion)
        {
            Respuesta respuesta = new Respuesta();

            try
            {
                Persona persona = await ConsultaInformacionRepository.DatosBasicosPersona(peticion);
                if (persona != null)
                {
                    SetDatosBasicosPersona(respuesta, persona);
                    List<Predio> predios = (List<Predio>)await ConsultaInformacionRepository.DatosPredios(peticion);
                    SetDatosPredios(respuesta, predios);
                    List<Asociacion> asociaciones = (List<Asociacion>)await ConsultaInformacionRepository.DatosAsociaciones(peticion);
                    SetDatosAsociaciones(respuesta, asociaciones);

                    if (persona.Carnet != null)
                    {
                        SetDatoCarnet(respuesta, persona.Carnet);
                    }
                }
            }
            catch (Exception exception)
            {
                logger.LogError(ERRORMESSAGE, exception);
                throw;
            }

            return respuesta;
        }

        /// <summary>
        /// Método para organiar la información del carnet del productor
        /// </summary>
        /// <param name="respuesta">Objeto en el cual se almacena la información a retornar</param>
        /// <param name="carnet">String con la información del carnet</param>
        private void SetDatoCarnet(Respuesta respuesta, String carnet)
        {
            DatoConsultado datoConsultado = new DatoConsultado()
            {
                CampoDato = ARCHIVOBASE64,
                ValorDato = carnet,
                TipoArchivo = TIPOARCHIVO,
                NombreArchivo = NOMBREARCHIVO,
                DescripcionArchivo = DESCRIPCIONARCHIVO
            };

            respuesta.DatoConsultado.Add(datoConsultado);
        }

        /// <summary>
        /// Método para organizar la información de las asociaciones en la estructura definida
        /// </summary>
        /// <param name="respuesta">Objeto en el cual se almacena la información a retornar</param>
        /// <param name="asociaciones">Objeto con la información de las asociaciones</param>
        private static void SetDatosAsociaciones(Respuesta respuesta, List<Asociacion> asociaciones)
        {
            DatoConsultado datoConsultado;

            foreach (var asociacion in asociaciones)
            {
                datoConsultado = new DatoConsultado
                {
                    CampoDato = string.Format(CampoDato.GetNitAsociacion(), asociacion.Nombre),
                    ValorDato = asociacion.Nit ?? string.Empty
                };
                respuesta.DatoConsultado.Add(datoConsultado);
            }
        }

        /// <summary>
        /// Método para organizar la información de los predios en la estructura definida
        /// </summary>
        /// <param name="respuesta">Objeto en el cual se almacena la información a retornar</param>
        /// <param name="predios">Objeto con la información de los predios</param>
        private static void SetDatosPredios(Respuesta respuesta, List<Predio> predios)
        {
            DatoConsultado datoConsultado;
            var propiedadesPredio = typeof(Predio).GetProperties();

            foreach (var predio in predios)
            {
                foreach (var propiedad in propiedadesPredio.Where(x => x.Name != nameof(predio.Nombre)))
                {
                    datoConsultado = new DatoConsultado
                    {
                        CampoDato = string.Format(CampoDato.GetInformacionPredio()[propiedad.Name.ToUpperInvariant()], predio.Nombre),
                        ValorDato = (string)(propiedad.GetValue(predio) ?? string.Empty)
                    };
                    respuesta.DatoConsultado.Add(datoConsultado);
                }
            }
        }

        /// <summary>
        /// Método para organizar la información de la persona en la estructura definida
        /// </summary>
        /// <param name="respuesta">Objeto en el cual se almacena la información a retornar</param>
        /// <param name="persona">Objeto con la información de la persona</param>
        private static void SetDatosBasicosPersona(Respuesta respuesta, Persona persona)
        {
            DatoConsultado datoConsultado;
            var propiedadesPersona = persona.GetType().GetProperties();

            foreach (var propiedad in propiedadesPersona.Where(x => CampoDato.GetInformacionBasica().ContainsKey(x.Name.ToUpperInvariant()) && x.Name != nameof(persona.Carnet)))
            {
                // Se pregunta si la propiedad es alguna de las fechas para realizar conversión al formato requerido
                if (propiedad.Name.StartsWith(TEXTOFECHA))
                {
                    propiedad.SetValue(persona, propiedad.GetValue(persona) != null ? Fecha.FechaAnioMesDia((string)propiedad.GetValue(persona)) : string.Empty);
                }

                datoConsultado = new DatoConsultado
                {
                    CampoDato = CampoDato.GetInformacionBasica()[propiedad.Name.ToUpperInvariant()],
                    ValorDato = (string)(propiedad.GetValue(persona) ?? string.Empty)
                };
                respuesta.DatoConsultado.Add(datoConsultado);
            }
        }
    }
}
