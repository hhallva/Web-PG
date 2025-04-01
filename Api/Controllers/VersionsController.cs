using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataLayer.DataContexts;
using DataLayer.Models;

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
