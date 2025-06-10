using Microsoft.AspNetCore.Mvc;
using ConsultaMaisAPI.Data;
using ConsultaMaisAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ConsultaMaisAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PacientesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PacientesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Pacientes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Paciente>>> GetPacientes()
        {
            var pacientes = await _context.Pacientes.ToListAsync();

            if (pacientes == null || !pacientes.Any())
            {
                return NotFound(new { Message = "Nenhum paciente encontrado." });
            }

            return Ok(pacientes);
        }

        // GET: api/Pacientes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Paciente>> GetPaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);

            if (paciente == null)
            {
                return NotFound(new { Message = "Paciente não encontrado." });
            }

            return Ok(paciente);
        }

        // POST: api/Pacientes
        [HttpPost]
        public async Task<ActionResult<Paciente>> PostPaciente(Paciente paciente)
        {
            if (paciente == null)
            {
                return BadRequest(new { Message = "Dados do paciente inválidos." });
            }

            _context.Pacientes.Add(paciente);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPaciente), new { id = paciente.Id }, paciente);
        }

        // PUT: api/Pacientes/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPaciente(int id, Paciente paciente)
        {
            if (id != paciente.Id)
            {
                return BadRequest(new { Message = "ID do paciente não corresponde." });
            }

            if (!await _context.Pacientes.AnyAsync(p => p.Id == id))
            {
                return NotFound(new { Message = "Paciente não encontrado para atualizar." });
            }

            _context.Entry(paciente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Pacientes.AnyAsync(p => p.Id == id))
                {
                    return NotFound(new { Message = "Paciente não encontrado durante a atualização." });
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Pacientes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePaciente(int id)
        {
            var paciente = await _context.Pacientes.FindAsync(id);
            if (paciente == null)
            {
                return NotFound(new { Message = "Paciente não encontrado para exclusão." });
            }

            _context.Pacientes.Remove(paciente);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
