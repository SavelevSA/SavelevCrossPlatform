using System.ComponentModel.DataAnnotations;

namespace SavelevCrossPlatform.Dtos;

public class LoginDto
{
    [Required(AllowEmptyStrings = false)]
    public string Username { get; set; } = string.Empty;

    [Required(AllowEmptyStrings = false)]
    public string Password { get; set; } = string.Empty;
}
