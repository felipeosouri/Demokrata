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
        public async Task<IActionResult> Search([FromQuery] string? firstName, [FromQuery] string? lastName, [FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var query = _context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(firstName))
                query = query.Where(u => u.PrimerNombre.Contains(firstName));

            if (!string.IsNullOrEmpty(lastName))
                query = query.Where(u => u.PrimerApellido.Contains(lastName));

            var users = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return Ok(users);
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
