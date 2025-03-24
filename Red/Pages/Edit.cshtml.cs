using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Red.Pages
{
    public class EditModel(UserService userService) : PageModel
    {

        [BindProperty]
        public User CurrentUser { get; set; } = new();

        public string Message { get; set; } = "";

        public async Task<IActionResult> OnGetAsync(int id)
        {
            CurrentUser = await userService.GetAsync(id);
            
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
