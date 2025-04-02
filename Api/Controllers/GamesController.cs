using DataLayer.DataContexts;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController(AppDbContext context) : ControllerBase
    {
        //Метод для получения списка всех существующих игр
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGamesAsync()
            => await context.Games
                .Include(g => g.Genre)
                .ToListAsync();


        //Метод для получения информации об одной конкретной игре
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGameAsync(int id)
        {
            var game = await context.Games
                .Include(g => g.Genre)
                .Include(g => g.GameVersions)
                .SingleOrDefaultAsync(g => g.Id == id);

            if (game == null)
                return NotFound();

            return game;
        }

        [HttpPost]
        public async Task<int> PostGameAsync(Game game)
        {
            context.Games.Add(game);
            await context.SaveChangesAsync();

            return game.Id;
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameAsync(int id)
        {
            var game = await context.Games.SingleOrDefaultAsync(g => g.Id == id);

            if (game == null)
                return NotFound();

            var versions = await context.GameVersions
                .Where(v => v.GameId == id)
                .ToListAsync();

            context.GameVersions.RemoveRange(versions);
            context.Games.Remove(game);

            await context.SaveChangesAsync();

            return Ok();
        }

        //Метод для обновления информации об игре
        [HttpPut("{id}")]
        public async Task<ActionResult<Game>> PutGameAsunc(int id, Game game)
        {
            if (id != game.Id)
                return BadRequest();

            context.Entry(game).State = EntityState.Modified;

            try
            {
                await context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (context.Games.Any(e => e.Id == id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        //Метод для получения списка материалов прикрепленных к игре
        [HttpGet("{gameId}/Materials")]
        public async Task<ActionResult<List<Material>>> GetMaterials(int gameId)
        {
            var materials = await context.Materials
                .Where(m => m.GameId == gameId)
                .ToListAsync();

            if (materials == null)
                return NotFound();

            return materials;
        }




    }
}
