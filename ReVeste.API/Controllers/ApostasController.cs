using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReVeste.API.Data;
using ReVeste.API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReVeste.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApostasController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ApostasController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todas as apostas registradas.
        /// </summary>
        /// <returns>Uma lista de apostas.</returns>
        // GET: api/Apostas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Aposta>>> GetApostas()
        {
            return await _context.Apostas.Include(a => a.Usuario).ToListAsync();
        }

        /// <summary>
        /// Obtém uma aposta específica pelo seu ID.
        /// </summary>
        /// <param name="id">ID da aposta.</param>
        /// <returns>A aposta correspondente ao ID.</returns>
        // GET: api/Apostas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Aposta>> GetAposta(int id)
        {
            var aposta = await _context.Apostas.Include(a => a.Usuario).FirstOrDefaultAsync(a => a.Id == id);

            if (aposta == null)
            {
                return NotFound();
            }

            return aposta;
        }

        /// <summary>
        /// Cria uma nova aposta.
        /// </summary>
        /// <param name="aposta">Dados da aposta a ser criada.</param>
        /// <returns>A aposta recém-criada.</returns>
        // POST: api/Apostas
        [HttpPost]
        public async Task<ActionResult<Aposta>> PostAposta(Aposta aposta)
        {
            _context.Apostas.Add(aposta);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAposta), new { id = aposta.Id }, aposta);
        }

        /// <summary>
        /// Atualiza uma aposta existente.
        /// </summary>
        /// <param name="id">ID da aposta a ser atualizada.</param>
        /// <param name="aposta">Novos dados da aposta.</param>
        /// <returns>NoContent se a atualização for bem-sucedida.</returns>
        // PUT: api/Apostas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAposta(int id, Aposta aposta)
        {
            if (id != aposta.Id)
            {
                return BadRequest();
            }

            _context.Entry(aposta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApostaExists(id))
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

        /// <summary>
        /// Exclui uma aposta pelo seu ID.
        /// </summary>
        /// <param name="id">ID da aposta a ser excluída.</param>
        /// <returns>NoContent se a exclusão for bem-sucedida.</returns>
        // DELETE: api/Apostas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAposta(int id)
        {
            var aposta = await _context.Apostas.FindAsync(id);
            if (aposta == null)
            {
                return NotFound();
            }

            _context.Apostas.Remove(aposta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApostaExists(int id)
        {
            return _context.Apostas.Any(e => e.Id == id);
        }

        /// <summary>
        /// Obtém todas as apostas de um usuário específico.
        /// </summary>
        /// <param name="usuarioId">ID do usuário.</param>
        /// <returns>Uma lista de apostas do usuário.</returns>
        // GET: api/Apostas/ByUsuario/1
        [HttpGet("ByUsuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<Aposta>>> GetApostasByUsuario(int usuarioId)
        {
            return await _context.Apostas
                               .Where(a => a.UsuarioId == usuarioId)
                               .Include(a => a.Usuario)
                               .ToListAsync();
        }

        /// <summary>
        /// Obtém apostas com valor maior que o especificado.
        /// </summary>
        /// <param name="valor">Valor mínimo da aposta.</param>
        /// <returns>Uma lista de apostas com valor maior que o especificado.</returns>
        // GET: api/Apostas/ValorMaiorQue/100
        [HttpGet("ValorMaiorQue/{valor}")]
        public async Task<ActionResult<IEnumerable<Aposta>>> GetApostasValorMaiorQue(decimal valor)
        {
            return await _context.Apostas
                               .Where(a => a.Valor > valor)
                               .Include(a => a.Usuario)
                               .ToListAsync();
        }

        /// <summary>
        /// Obtém apostas feitas em uma data específica.
        /// </summary>
        /// <param name="data">Data da aposta.</param>
        /// <returns>Uma lista de apostas feitas na data especificada.</returns>
        // GET: api/Apostas/ByData/2023-01-01
        [HttpGet("ByData/{data}")]
        public async Task<ActionResult<IEnumerable<Aposta>>> GetApostasByData(DateTime data)
        {
            return await _context.Apostas
                               .Where(a => a.DataAposta.Date == data.Date)
                               .Include(a => a.Usuario)
                               .ToListAsync();
        }
    }
}

