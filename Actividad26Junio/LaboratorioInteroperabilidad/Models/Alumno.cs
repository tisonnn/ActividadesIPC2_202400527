using System.ComponentModel.DataAnnotations;

namespace LaboratorioInteroperabilidad.Models
{
    public class Alumno
    {
        [Key]
        public string Carnet { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
    }
}