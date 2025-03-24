using DataLayer.DataContexts;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(int userId)
        {
            var user = await _context.Users
                .Include(u => u.Games)
                .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
                return NotFound();
            return user;
        }

        [HttpPut("{userId}")]
        public async Task<IActionResult> PutUser(int userId, User user)
        {
            if (userId != user.Id)
                return BadRequest();


            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (_context.Users.Any(e => e.Id == userId))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        [HttpGet("{userId}/Games")]
        public async Task<ActionResult<List<Game>>> GetGamesAsync(int userId)
        {
            var games = await _context.Games
                .Where(g => g.Users.Any(u => u.Id == userId))
                .ToListAsync();

            if (games == null)
                return NotFound();

            return games;
        }

        [HttpPost("{userId}/Games/{gameId}")]
        public async Task<IActionResult> PostGameAsync(int userId, int gameId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Games)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user.Games.Any(g => g.Id == gameId))
                    return Conflict("Игра уже присутсвует");

                var game = await _context.Games.FindAsync(gameId);

                if (game == null)
                    return NotFound();

                user.Games.Add(game);
                _context.Users.Update(user);

                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUser", new { userId = user.Id }, user);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{userId}/Games/{gameId}")]
        public async Task<IActionResult> DeleteGameAsync(int userId, int gameId)
        {
            try
            {
                var user = await _context.Users
                    .Include(u => u.Games)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (!user.Games.Any(g => g.Id == gameId))
                    return NotFound();

                var game = await _context.Games.FindAsync(gameId);

                if (game == null)
                    return NotFound();

                user.Games.Remove(game);
                _context.Users.Update(user);

                await _context.SaveChangesAsync();
                return CreatedAtAction("GetUser", new { userId = user.Id }, user);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
