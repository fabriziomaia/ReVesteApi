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
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UsuariosController(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém todos os usuários registrados.
        /// </summary>
        /// <returns>Uma lista de usuários.</returns>
        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.Include(u => u.Apostas).ToListAsync();
        }

        /// <summary>
        /// Obtém um usuário específico pelo seu ID.
        /// </summary>
        /// <param name="id">ID do usuário.</param>
        /// <returns>O usuário correspondente ao ID.</returns>
        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.Include(u => u.Apostas).FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        /// <summary>
        /// Cria um novo usuário.
        /// </summary>
        /// <param name="usuario">Dados do usuário a ser criado.</param>
        /// <returns>O usuário recém-criado.</returns>
        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario([FromForm] Usuario usuario)
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuario);
        }

        /// <summary>
        /// Atualiza um usuário existente.
        /// </summary>
        /// <param name="id">ID do usuário a ser atualizado.</param>
        /// <param name="usuario">Novos dados do usuário.</param>
        /// <returns>NoContent se a atualização for bem-sucedida.</returns>
        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
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
        /// Exclui um usuário pelo seu ID.
        /// </summary>
        /// <param name="id">ID do usuário a ser excluído.</param>
        /// <returns>NoContent se a exclusão for bem-sucedida.</returns>
        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        /// <summary>
        /// Obtém usuários pelo nome.
        /// </summary>
        /// <param name="nome">Nome do usuário.</param>
        /// <returns>Uma lista de usuários com o nome especificado.</returns>
        // GET: api/Usuarios/ByName/NomeTeste
        [HttpGet("ByName/{nome}")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosByName(string nome)
        {
            return await _context.Usuarios
                               .Where(u => u.Nome.Contains(nome))
                               .Include(u => u.Apostas)
                               .ToListAsync();
        }
    }
}

