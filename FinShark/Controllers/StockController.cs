using FinShark.Data;
using FinShark.DTOS.Stock;
using FinShark.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> GetAllStocks()
    {
        var stocks = await _dbContext.Stocks.ToListAsync();
        var stockDto = stocks.Select(s=> s.ToStockDto());

        return Ok(stocks);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetStockById([FromRoute] int id) 
    {
        var stock = await _dbContext.Stocks.FindAsync(id);

        if(stock == null) { return NotFound(); }
        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> CreateStock([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDTO();
        //The id of the stock entity is not auto-generated, so in order to deal with the error of violating 
        //the pk constraint in the database, the id is set manually, by finding the last id in the database 
        //and increasing by 1
        stockModel.Id = await _dbContext.Stocks.MaxAsync(e => e.Id) +1;
        
        await _dbContext.Stocks.AddAsync(stockModel);
        await _dbContext.SaveChangesAsync();

        return CreatedAtAction(nameof(GetStockById), new { id = stockModel.Id}, stockModel.ToStockDto());

    }

    [HttpPut]
    [Route("{id}")]
    public async  Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        var stockModel = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);
        if (stockModel == null) { return NotFound(); }

        stockModel.Symbol = updateDto.Symbol;
        stockModel.CompanyName = updateDto.CompanyName;
        stockModel.Purchase = updateDto.Purchase;
        stockModel.LastDiv = updateDto.LastDiv;
        stockModel.Industry = updateDto.Industry; 
        stockModel.MarketCap = updateDto.MarketCap;

        await _dbContext.SaveChangesAsync();
        return Ok(stockModel.ToStockDto());
    }


    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stockModel = await _dbContext.Stocks.FirstOrDefaultAsync(x => x.Id == id);

        if (stockModel == null) { return NotFound(); }

        _dbContext.Stocks.Remove(stockModel);
        await _dbContext.SaveChangesAsync(true);
        return NoContent();
    }

}
