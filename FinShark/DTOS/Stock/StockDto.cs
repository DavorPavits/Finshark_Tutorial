﻿namespace FinShark.DTOS.Stock;

public class StockDto
{
    public int Id { get; set; }
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;

    public decimal Purchase { get; set; }

    public decimal LastDiv { get; set; }
    public string Industry { get; set; } = string.Empty;
    public long MarketCap { get; set; }

    //Coments where here but we are not yet using them
}
