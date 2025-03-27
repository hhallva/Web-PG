using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Red.Pages
{
    public class GameModel(
        GameService gameService,
        MaterialService materialService,
        IWebHostEnvironment webHostEnvironment) : PageModel
    {
        private const long MaxFileSize = 20 * 1024 * 1024;
        private static readonly string[] AllowedExtensions = { ".pdf", ".pptx", ".xlsx", ".docx", ".jpg", ".mkv", ".avi", ".mp4" };

        [BindProperty(SupportsGet = true)]
        public int GameId { get; set; }

        public Game? Game { get; set; } = new();
        public List<Material?> Materials { get; set; } = new();

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();


        public async Task<IActionResult> OnGetAsync()
        {
            Game = await gameService.GetAsync(GameId);
            Materials = await gameService.GetMaterialsAsync(GameId);
            return Page();
        }

        public async Task<IActionResult> OnPostUploadAsync()
        {
            if (!ModelState.IsValid)
            {
                await OnGetAsync();
                return Page();
            }

            if (Input.UploadedFile != null && Input.UploadedFile.Length > 0)
            {
                if (Input.UploadedFile.Length > MaxFileSize)
                {
                    ModelState.AddModelError("Input.UploadedFile", "Размер файла превышает 20 Мбайт.");
                    await OnGetAsync();
                    return Page();
                }

                string fileExtension = Path.GetExtension(Input.UploadedFile.FileName).ToLower();
                if (!AllowedExtensions.Contains(fileExtension))
                {
                    ModelState.AddModelError("Input.UploadedFile", "Неподдерживаемый тип файла.");
                    await OnGetAsync();
                    return Page();
                }

                var game = await gameService.GetAsync(GameId);
                string gameName = game.Name.Replace(" ", "_");
                string uploadFolder = Path.Combine(webHostEnvironment.WebRootPath, "materials", gameName);

                if (!Directory.Exists(uploadFolder))
                    Directory.CreateDirectory(uploadFolder);

                string uniqueFileName = Guid.NewGuid().ToString() + fileExtension;
                string filePath = Path.Combine(uploadFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                    await Input.UploadedFile.CopyToAsync(fileStream);


                var material = new Material
                {
                    GameId = GameId,
                    Name = Input.MaterialName,
                    Path = filePath,
                    Type = fileExtension,
                    Size = Input.UploadedFile.Length
                };
                materialService.AddMaterialAsync(material);

                return RedirectToPage(new { GameId });
            }

            ModelState.AddModelError("", "Необходимо выбрать файл для загрузки.");
            await OnGetAsync();
            return Page();
        }

        public async Task<IActionResult> OnPostDeleteMaterialAsync(int materialId)
        {
            try
            {
                await materialService.DeleteMaterialAsync(materialId);
                return RedirectToPage(new { gameId = GameId });
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "Произошла ошибка при удалении материала. Пожалуйста, попробуйте позже.");
                await OnGetAsync();
                return Page();
            }
        }
    }
}



public class InputModel
{
    [Required(ErrorMessage = "Необходимо указать название материала.")]
    [Display(Name = "Название материала")]
    public string MaterialName { get; set; } = "";

    [Display(Name = "Файл")]
    public IFormFile? UploadedFile { get; set; } = null!;
}