using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace UserManagementApi.Models.DTO
{
    public class UserDto
    {
        public string PrimerNombre { get; set; } = string.Empty;
        public string? SegundoNombre { get; set; }
        public string PrimerApellido { get; set; } = string.Empty;
        public string? SegundoApellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public decimal Salario { get; set; }
    }
}
