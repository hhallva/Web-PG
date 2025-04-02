using DataLayer.DataContexts;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VersionsController(AppDbContext context) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<GameVersion>> PostGameVersion(GameVersion gameVersion)
        {
            context.GameVersions.Add(gameVersion);
            await context.SaveChangesAsync();

            return Created("", gameVersion);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGameVersion(int id)
        {
            var gameVersion = await context.GameVersions.FindAsync(id);
            if (gameVersion == null)
            {
                return NotFound();
            }

            context.GameVersions.Remove(gameVersion);
            await context.SaveChangesAsync();

            return Ok();
        }

    }
}
