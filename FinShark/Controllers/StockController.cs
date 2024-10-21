using FinShark.Data;
using FinShark.DTOS.Stock;
using FinShark.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace FinShark.Controllers;

[Route("api/stocks")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDBContext _dbContext;
    public StockController(ApplicationDBContext context)
    {
        _dbContext = context;
        
    }

    [HttpGet]
    public IActionResult GetAllStocks()
    {
        var stocks = _dbContext.Stocks.ToList()
                .Select(s=> s.ToStockDto());

        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public IActionResult GetStockById([FromRoute] int id) 
    {
        var stock = _dbContext.Stocks.Find(id);

        if(stock == null) { return NotFound(); }
        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public IActionResult CreateStock([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDTO();
        stockModel.Id = _dbContext.Stocks.Max(e => e.Id) +1;
        _dbContext.Stocks.Add(stockModel);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetStockById), new { id = stockModel.Id}, stockModel.ToStockDto());

    }


}
