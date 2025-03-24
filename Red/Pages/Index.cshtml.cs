using DataLayer.Models;
using DataLayer.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Red.Pages
{
    public class IndexModel(UserService userService) : PageModel
    {
        public List<User?> Users { get; set; } = new();
            
        public async Task<IActionResult> OnGetAsync()
        {
            Users = await userService.GetAllAsync();
            return Page();
        }
    }
}
