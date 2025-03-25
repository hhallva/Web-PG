using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Red.Pages
{
    public class GameModel(GameService gameService) : PageModel
    {
        public Game Game { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int GameId { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Game = await gameService.GetAsync(GameId);

            return Page();
        }
    }
}
