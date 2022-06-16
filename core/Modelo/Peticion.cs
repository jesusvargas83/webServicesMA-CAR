using System.ComponentModel.DataAnnotations;

namespace core.modelo
{
    public class Peticion
    {
        [Required]
        [RegularExpression("^CC$")]
        public string TipoId { get; set; }
        [Required]
        [RegularExpression("^\\d{1,10}$")]
        public string IdUsuario { get; set; }
    }
}
