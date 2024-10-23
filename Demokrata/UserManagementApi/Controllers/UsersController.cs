using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UserManagementApi.Data;
using UserManagementApi.Models;
using UserManagementApi.Models.DTO;

namespace UserManagementApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : Controller
    {
        private readonly UserContext _context;

        public UsersController(UserContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var users = await _context.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            return Ok(user);
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(
            [FromQuery] string? primerNombre = null,
            [FromQuery] string? primerApellido = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10)
        {
            if (page <= 0 || pageSize <= 0)
            {
                return BadRequest("Page and pageSize must be greater than 0.");
            }

            // Construcción dinámica de la consulta con filtros opcionales
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrWhiteSpace(primerNombre))
            {
                query = query.Where(u => EF.Functions.Like(u.PrimerNombre, $"%{primerNombre}%"));
            }

            if (!string.IsNullOrWhiteSpace(primerApellido))
            {
                query = query.Where(u => EF.Functions.Like(u.PrimerApellido, $"%{primerApellido}%"));
            }

            // Obtener el total de registros para la paginación
            var totalRecords = await query.CountAsync();

            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // Crear un objeto de respuesta con la lista y la paginación
            var response = new
            {
                Data = users,
                Page = page,
                PageSize = pageSize,
                TotalRecords = totalRecords,
                TotalPages = (int)Math.Ceiling(totalRecords / (double)pageSize)
            };

            return Ok(response);
        }


        [HttpPost]
        public async Task<IActionResult> Create(UserDto userDto)
        {
            var user = new User
            {
                PrimerNombre = userDto.PrimerNombre,
                SegundoNombre = userDto.SegundoNombre,
                PrimerApellido = userDto.PrimerApellido,
                SegundoApellido = userDto.SegundoApellido,
                FechaNacimiento = userDto.FechaNacimiento,
                Salario = userDto.Salario
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UserDto userDto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            user.PrimerNombre = userDto.PrimerNombre;
            user.SegundoNombre = userDto.SegundoNombre;
            user.PrimerApellido = userDto.PrimerApellido;
            user.SegundoApellido = userDto.SegundoApellido;
            user.FechaNacimiento = userDto.FechaNacimiento;
            user.Salario = userDto.Salario;
            user.FechaModificacion = DateTime.Now;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return NotFound();

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
