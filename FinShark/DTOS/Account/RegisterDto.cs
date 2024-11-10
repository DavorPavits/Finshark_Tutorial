using System.ComponentModel.DataAnnotations;

namespace FinShark.DTOS.Account;

public class RegisterDto
{
    [Required]
    public string? UserName { get; set; }

    [Required]
    [EmailAddress]
    public string? EmailAddress { get; set; }

    [Required]
    public string? Password { get; set; }   
}
