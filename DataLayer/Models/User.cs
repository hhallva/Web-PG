using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;

namespace DataLayer.Models;

public partial class User
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Логин обязателен.")]
    [StringLength(30, ErrorMessage = "Логин должен быть минимум 4 символа.", MinimumLength =4)]
    [RegularExpression(@"^\S*$", ErrorMessage = "Логин не должен содержать пробелы.")]
    public string Login { get; set; } = null!;

    [Required(ErrorMessage = "Пароль обязателен.")]
    [StringLength(30, ErrorMessage = "Пароль должен быть минимум 8 символов.", MinimumLength = 8)]
    [RegularExpression("^(?=.*[A-Z])(?=.*[a-z])(?=.*\\d)\\S{8,}$", ErrorMessage = "Пароль на надежный и/или содержит пробелы")]
    public string Password { get; set; } = null!;

    [Required(ErrorMessage = "Номер телефона обязателен.")]
    [RegularExpression(@"^((\+\d\(\d{3}\)\d{3}-\d{2}-\d{2})", ErrorMessage = "Номер телефона должен соответствовать формату +7(000)000-00-00")]
    public string Phone { get; set; } = null!;

    public virtual ICollection<Game> Games { get; set; } = new List<Game>();
}
