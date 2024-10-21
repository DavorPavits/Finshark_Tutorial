using System.ComponentModel.DataAnnotations.Schema;

namespace FinShark.Models;

[Table("Stocks")]
public class Stock
{
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;
    
    public decimal Purchase { get; set; }
    
    public decimal LastDiv { get; set; }
    public string Industry { get; set; } = string.Empty;
    public long MarketCap { get; set; }

    //Mapping One stock to multiple comments (one-to-many relationship)
    public List<Comment> Comments { get; set; } = new List<Comment>();
}
