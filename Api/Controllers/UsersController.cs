using DataLayer.DataContexts;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(AppDbContext context) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
            => await context.Users.ToListAsync();

        [HttpGet("{userId}")]
        public async Task<ActionResult<User>> GetUser(int userId)
        {
            var user = await context.Users
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


            context.Entry(user).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (context.Users.Any(e => e.Id == userId))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        //Метод для получения списка всех игр из библиотеки пользователя
        [HttpGet("{userId}/Games")]
        public async Task<ActionResult<List<Game>>> GetGamesAsync(int userId)
        {
            var games = await context.Games
                .Include(g => g.Genre)
                .Where(g => g.Users.Any(u => u.Id == userId))
                .ToListAsync();

            if (games == null)
                return NotFound();

            return games;
        }

        //Методы для добалвения существующей игры в библиотеку
        [HttpPost("{userId}/Games/{gameId}")]
        public async Task<IActionResult> PostUserGameAsync(int userId, int gameId)
        {
            try
            {
                var user = await context.Users
                    .Include(u => u.Games)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user.Games.Any(g => g.Id == gameId))
                    return Conflict("Игра уже присутсвует");

                var game = await context.Games.FindAsync(gameId);

                if (game == null)
                    return NotFound();

                user.Games.Add(game);
                context.Users.Update(user);

                await context.SaveChangesAsync();
                return CreatedAtAction("GetUser", new { userId = user.Id }, user);
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{userId}/Games/{gameId}")]
        public async Task<IActionResult> DeleteUserGameAsync(int userId, int gameId)
        {
            try
            {
                var user = await context.Users
                    .Include(u => u.Games)
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (!user.Games.Any(g => g.Id == gameId))
                    return NotFound();

                var game = await context.Games.FindAsync(gameId);

                if (game == null)
                    return NotFound();

                user.Games.Remove(game);
                context.Users.Update(user);

                await context.SaveChangesAsync();
                return CreatedAtAction("GetUser", new { userId = user.Id }, user);
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
