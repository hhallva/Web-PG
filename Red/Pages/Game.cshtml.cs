using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Red.Pages
{
    public class GameModel(
        GameService gameService,
        IWebHostEnvironment webHostEnvironment
        ) : PageModel
    {

        public Game Game { get; set; } = new();

        

        [BindProperty(SupportsGet = true)]
        public int GameId { get; set; }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        public async Task<IActionResult> OnGet()
        {
            Game = await gameService.GetAsync(GameId);

            return Page();
        }


        public async Task<IActionResult> OnPostUploadAsync() 
        {
            if (UploadedFile != null && UploadedFile.Length > 0)
            {
                var game = await gameService.GetAsync(GameId);

                string gameName = game.Name.Replace(" ", "_");
                string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "materials", gameName);

                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);
 
                string filePath = Path.Combine(uploadFolder, UploadedFile.FileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                    await UploadedFile.CopyToAsync(fileStream);

                return RedirectToPage(new { GameId }); 
            }

            return Page();
        }
    }
}
