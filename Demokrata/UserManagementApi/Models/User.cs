using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserManagementApi.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, MaxLength(50), RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Se deben ingresar solo letras")]
        public string PrimerNombre { get; set; } = string.Empty;

        [MaxLength(50), RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Se deben ingresar solo letras")]
        public string? SegundoNombre { get; set; }

        [Required, MaxLength(50), RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Se deben ingresar solo letras")]
        public string PrimerApellido { get; set; } = string.Empty;

        [MaxLength(50), RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Se deben ingresar solo letras")]
        public string? SegundoApellido { get; set; }

        [Required]
        public DateTime FechaNacimiento { get; set; }

        [Required, Range(1, double.MaxValue, ErrorMessage = "El sueldo debe ser mayor a 0")]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Salario { get; set; }

        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public DateTime? FechaModificacion { get; set; }
    }
}
