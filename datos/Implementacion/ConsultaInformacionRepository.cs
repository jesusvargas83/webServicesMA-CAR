using core.modelo;
using core.Modelo.ConsultaInformacion;
using datos.interfaz;
using datos.Interfaz;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace datos.Implementacion
{
    /// <summary>
    /// Clase que realiza la implementación de las consultas SQL necesarias a base de datos
    /// </summary>
    public class ConsultaInformacionRepository : IConsultaInformacionRepository
    {
        private const string ERRORMESSAGEBASICDATA = "Error durante ejecución de consulta de información básica.";
        private const string ERRORMESSAGEPROPERTYDATA = "Error durante ejecución de consulta de información de predios";
        private const string ERROMESSAGEASSOCIATIONDATA = "Error durante ejecución de consulta de información de asociaciones";

        private const int FILTROESTADOIDPRODUCTOR = 1;
        private const bool FILTROVALIDOPRODUCTOR = true;
        private const bool FILTROESTADOPRODUCTORASOCIADO = true;

        private readonly ILogger<ConsultaInformacionRepository> logger;
        private readonly IDapperConnector DapperConnector;

        /// <summary>
        /// Constructor de la clase que recibe la inyección de dependencias
        /// </summary>
        /// <param name="dapperConnector">Objeto para realizar el llamado a base de datos y ejecución de consultas SQL</param>
        /// <param name="logger">Objeto para realizar el logging</param>
        public ConsultaInformacionRepository(IDapperConnector dapperConnector, ILogger<ConsultaInformacionRepository> logger)
        {
            this.DapperConnector = dapperConnector;
            this.logger = logger;
        }

        public async Task<Persona> DatosBasicosPersona(Peticion peticion)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select distinct upper(td.\"Nombre\") as TipoIdentificacion, p2.\"NumeroDocumento\", p.\"FechaExpedicion\", upper(p2.\"PrimerNombre\") as PrimerNombre, upper(p2.\"SegundoNombre\") as SegundoNombre, ");
            sql.Append("upper(p2.\"PrimerApellido\") as PrimerApellido, upper(p2.\"SegundoApellido\") as SegundoApellido, upper(s.\"Nombre\") as Sexo, p.\"Telefono\", upper(p2.\"Direccion\") as Direccion, ");
            sql.Append("p.\"FechaNacimiento\", upper(p2.\"Email\") as CorreoElectronico, upper(ec.\"Nombre\") as EstadoCivil, case when p.\"CabezaFamilia\" is true then 'SÍ' when p.\"CabezaFamilia\" is false then 'NO' end as CabezaFamilia, ");
            sql.Append("upper(ge.\"Nombre\") as PertenenciaEtnica, p.\"CarnetProductor\" as Carnet ");
            sql.Append("from \"PRODUCTORES\".\"Productor\" p ");
            sql.Append("inner join \"PRODUCTORES\".\"TipoDocumentos\" td on td.\"Id\" = p.\"TipoDocumento\" ");
            sql.Append("inner join \"AUTORIZACION\".\"Personas\" p2 on p2.\"TipoDocumentoId\" = p.\"TipoDocumento\" and p2.\"NumeroDocumento\" = p.\"NumeroDocumento\" ");
            sql.Append("inner join \"PRODUCTORES\".\"Sexos\" s on s.\"Id\" = p.\"SexoId\" ");
            sql.Append("inner join \"PRODUCTORES\".\"EstadosCivil\" ec on ec.\"Id\" = p.\"EstadoCivilId\" ");
            sql.Append("inner join \"PRODUCTORES\".\"GruposEtnico\" ge on ge.\"Id\" = p.\"GrupoEtnicoId\" ");
            sql.AppendFormat("where p.\"NumeroDocumento\" = '{0}' ", peticion.IdUsuario);
            sql.AppendFormat("and td.\"Sigla\" = '{0}' ", peticion.TipoId);
            sql.AppendFormat("and p.\"EstadoId\" = {0} ", FILTROESTADOIDPRODUCTOR);
            sql.AppendFormat("and p.\"Valido\" = {0};", FILTROVALIDOPRODUCTOR);

            try
            {
                Persona persona = await DapperConnector.QuerySingleOrDefaultAsync<Persona>(sql.ToString(), commandType: System.Data.CommandType.Text);

                return persona;
            }
            catch (Exception exception)
            {
                logger.LogError(ERRORMESSAGEBASICDATA, exception);
                throw;
            }
        }

        public async Task<IReadOnlyList<Predio>> DatosPredios(Peticion peticion)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select distinct upper(d.\"Nombre\") as Departamento, upper(m.\"Nombre\") as Municipio, upper(v.\"Nombre\") as Vereda, upper(p.\"Nombre\") as Nombre, p.\"Coordenada\" as Ubicacion, ");
            sql.Append("p.\"CedulaCatastral\", p.\"MatriculaInmobiliaria\", upper(ft.\"Nombre\") as FormaTenencia, ");
            sql.Append("( ");
            sql.Append("select string_agg(distinct upper(p3.\"Propiedad\"), ', ') as Producto ");
            sql.Append("from \"PRODUCTORES\".\"LineasProductivas\" lp ");
            sql.Append("inner join \"PRODUCTORES\".\"Productos\" p3 on p3.\"Id\" = lp.\"ProductoId\" ");
            sql.Append("inner join \"PRODUCTORES\".\"Predios\" p4 on p4.\"Id\" = lp.\"PredioId\" ");
            sql.Append("inner join \"PRODUCTORES\".\"Productor\" p5 on p5.\"Id\" = p4.\"ProductoresId\" ");
            sql.Append("inner join \"PRODUCTORES\".\"TipoDocumentos\" td2 on td2.\"Id\" = p5.\"TipoDocumento\" ");
            sql.AppendFormat("where p5.\"NumeroDocumento\" = '{0}' ", peticion.IdUsuario);
            sql.AppendFormat("and td2.\"Sigla\" = '{0}' ", peticion.TipoId);
            sql.Append("and p4.\"Nombre\" = p.\"Nombre\" ");
            sql.Append(") as Producto ");
            sql.Append("from \"PRODUCTORES\".\"Predios\" p ");
            sql.Append("inner join \"PRODUCTORES\".\"Productor\" p2 on p2.\"Id\" = p.\"ProductoresId\" ");
            sql.Append("inner join \"PRODUCTORES\".\"TipoDocumentos\" td on td.\"Id\" = p2.\"TipoDocumento\" ");
            sql.Append("inner join \"PRODUCTORES\".\"Departamentos\" d on d.\"Id\" = p.\"DepartamentoId\" ");
            sql.Append("inner join \"PRODUCTORES\".\"Municipios\" m on m.\"Id\" = p.\"MunicipioId\" ");
            sql.Append("inner join \"PRODUCTORES\".\"Veredas\" v on v.\"Id\" = p.\"VeredaId\" ");
            sql.Append("inner join \"PRODUCTORES\".\"FormaTenencia\" ft on ft.id = p.\"FormaTenenciaId\" ");
            sql.AppendFormat("where p2.\"NumeroDocumento\" = '{0}' ", peticion.IdUsuario);
            sql.AppendFormat("and td.\"Sigla\" = '{0}' ", peticion.TipoId);
            sql.AppendFormat("and p2.\"EstadoId\" = {0} ", FILTROESTADOIDPRODUCTOR);
            sql.AppendFormat("and p2.\"Valido\" = {0};", FILTROVALIDOPRODUCTOR);

            try
            {
                List<Predio> predios = (List<Predio>)await DapperConnector.QueryAsync<Predio>(sql.ToString(), commandType: System.Data.CommandType.Text);

                return predios;
            }
            catch (Exception exception)
            {
                logger.LogError(ERRORMESSAGEPROPERTYDATA, exception);
                throw;
            }
        }

        public async Task<IReadOnlyList<Asociacion>> DatosAsociaciones(Peticion peticion)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append("select distinct upper(adb.\"Nombre\") as Nombre, adb.\"NitAgremiacion\" as Nit ");
            sql.Append("from \"AGREMIACION\".\"AgremiacionDatosBasicos\" adb ");
            sql.Append("inner join \"AGREMIACION\".\"ProductorAsociado\" pa on pa.\"AgremiacionId\" = adb.\"Id\" ");
            sql.Append("inner join \"PRODUCTORES\".\"Productor\" p on p.\"Id\" = pa.\"ProductorId\" ");
            sql.Append("inner join \"PRODUCTORES\".\"TipoDocumentos\" td on td.\"Id\" = p.\"TipoDocumento\" ");
            sql.AppendFormat("where p.\"NumeroDocumento\" = '{0}' ", peticion.IdUsuario);
            sql.AppendFormat("and td.\"Sigla\" = '{0}' ", peticion.TipoId);
            sql.AppendFormat("and p.\"EstadoId\" = {0} ", FILTROESTADOIDPRODUCTOR);
            sql.AppendFormat("and p.\"Valido\" = {0} ", FILTROVALIDOPRODUCTOR);
            sql.AppendFormat("and pa.\"Estado\" = {0};", FILTROESTADOPRODUCTORASOCIADO);

            try
            {
                List<Asociacion> asociaciones = (List<Asociacion>)await DapperConnector.QueryAsync<Asociacion>(sql.ToString(), commandType: System.Data.CommandType.Text);

                return asociaciones;
            }
            catch (Exception exception)
            {
                logger.LogError(ERROMESSAGEASSOCIATIONDATA, exception);
                throw;
            }
        }
    }
}
