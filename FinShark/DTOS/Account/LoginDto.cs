using System.ComponentModel.DataAnnotations;

namespace FinShark.DTOS.Account;

public class LoginDto
{
    [Required]
    public string UserName  { get; set; }
    [Required]
    public string Password { get; set; }
}
