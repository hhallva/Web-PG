using DataLayer.DataContexts;
using DataLayer.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MaterialsController(AppDbContext context) : ControllerBase
    {
        //Метод для получения материала по Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Material>> GetMaterial(int id)
        {
            var material = await context.Materials.FindAsync(id);

            if (material == null)
            {
                return NotFound();
            }

            return material;
        }

        //Метод добавления материала к игре
        [HttpPost]
        public async Task<ActionResult<Material>> PostMaterial(Material material)
        {
            var game = await context.Games.FindAsync(material.GameId);

            if (game == null)
                NotFound();

            game.Materials.Add(material);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetMaterial), new { id = material.Id }, material);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMaterial(int id)
        {
            var material = await context.Materials.FindAsync(id);
            if (material == null)
                return NotFound();

            string filePath = material.Path;
            if (!string.IsNullOrEmpty(filePath) && System.IO.File.Exists(filePath))
                System.IO.File.Delete(filePath);

            context.Materials.Remove(material);
            await context.SaveChangesAsync();

            return NoContent();
        }
    }
}
