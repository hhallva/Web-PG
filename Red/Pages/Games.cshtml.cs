using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Red.Pages
{
    public class GamesModel(GameService gameService, UserService userService) : PageModel
    {
        [BindProperty]
        public int? SelectedGameId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }

        public List<Game?> UserGames { get; set; } = new();

        public async Task OnGetAsync()
        {
            UserGames = await userService.GetGamesAsync(UserId);
            ViewData["games"] = await gameService.GetAllAsync();
        }

        public async Task<IActionResult> OnPostAddGameAsync()
        {
            if (!SelectedGameId.HasValue)
                ModelState.AddModelError("SelectedGameId", "Пожалуйста, выберите игру.");

            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            try
            {
                await userService.AddGameAsync(UserId, SelectedGameId.Value);
                return RedirectToPage(new { userId = UserId });
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка при добавлении игры. Пожалуйста, попробуйте позже.");
                await OnGetAsync();
                return Page();
            }
        }
        public async Task<IActionResult> OnPostDeleteGameAsync(int gameId)
        {
            try
            {
                await userService.DeleteGameAsync(UserId, gameId);
                return RedirectToPage(new { userId = UserId });
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка при удалении игры. Пожалуйста, попробуйте позже.");
                await OnGetAsync();
                return Page();
            }
        }
    }
}