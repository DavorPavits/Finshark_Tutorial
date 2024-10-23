using System.ComponentModel.DataAnnotations;

namespace FinShark.DTOS.Stock;

public class CreateStockRequestDto
{
    [Required]
    [MaxLength(10, ErrorMessage ="Symbol cannot be over 10 characters")]
    public string Symbol { get; set; } = string.Empty;
    [Required]
    [MaxLength(10, ErrorMessage = "Company Name cannot be over 10 characters")]
    public string CompanyName { get; set; } = string.Empty;

    [Required]
    [Range(1, 10000000)]
    public decimal Purchase { get; set; }

    [Required]
    [Range(1, 10000000)]
    public decimal LastDiv { get; set; }

    [Required]
    [MaxLength(10, ErrorMessage = "Industry cannot be over 10 characters")]
    public string Industry { get; set; } = string.Empty;

    [Required]
    [Range(1, 100000000000)]
    public long MarketCap { get; set; }
}
