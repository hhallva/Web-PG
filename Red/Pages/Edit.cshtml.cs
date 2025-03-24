using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Red.Pages
{
    public class EditModel(UserService userService) : PageModel
    {

        [BindProperty]
        public User CurrentUser { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public int UserId { get; set; }

        public string Message { get; set; } = "";

        public async Task<IActionResult> OnGetAsync()
        {
            CurrentUser = await userService.GetAsync(UserId);

            return Page();
        }

        public async Task OnPostAsync(int id)
        {
            try
            {
                await userService.UpdateAsync(CurrentUser);
                Message = "Данные сохранены успешно.";
            }
            catch
            {
                Message = "Ошибка сохранения данных.";
            }
        }
    }
}
