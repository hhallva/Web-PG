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
