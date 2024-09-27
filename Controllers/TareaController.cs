using DB.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SistemaTareas.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TareaController : ControllerBase
    {
        private readonly SistemaTareasContext _context;

        public TareaController(SistemaTareasContext context)
        {
            _context = context;
        }

        // GET: api/Tareas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tarea>>> GetTareas()
        {
            return await _context.Tareas
                .Include(t => t.IdUsuarioAsignaTareaNavigation)
                .Include(t => t.IdUsuarioTareaNavigation)
                .ToListAsync();
        }

        // GET: api/Tareas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Tarea>> GetTarea(int id)
        {
            var tarea = await _context.Tareas
                .Include(t => t.IdUsuarioAsignaTareaNavigation)
                .Include(t => t.IdUsuarioTareaNavigation)
                .FirstOrDefaultAsync(t => t.IdTarea == id);

            if (tarea == null)
            {
                return NotFound();
            }

            return tarea;
        }

        // PUT: api/Tareas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTarea(int id, Tarea tarea)
        {
            if (id != tarea.IdTarea)
            {
                return BadRequest();
            }

            _context.Entry(tarea).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TareaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Tareas
        [HttpPost]
        public async Task<ActionResult<Tarea>> PostTarea(Tarea tarea)
        {
            _context.Tareas.Add(tarea);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTarea", new { id = tarea.IdTarea }, tarea);
        }

        // DELETE: api/Tareas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTarea(int id)
        {
            var tarea = await _context.Tareas.FindAsync(id);
            if (tarea == null)
            {
                return NotFound();
            }

            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TareaExists(int id)
        {
            return _context.Tareas.Any(e => e.IdTarea == id);
        }
    }

}
