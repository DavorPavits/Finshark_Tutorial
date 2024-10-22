using FinShark.Data;
using FinShark.DTOS.Stock;
using FinShark.Interfaces;
using FinShark.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Controllers;

[Route("api/stocks")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDBContext _dbContext;
    private readonly IStockRepository _stockRepo;
        

    public StockController(ApplicationDBContext context, IStockRepository stockRepository)
    {
        _dbContext = context;
        _stockRepo = stockRepository;
        
    }

    [HttpGet]
    public async Task<IActionResult> GetAllStocks()
    {
        var stocks = await _stockRepo.GetAllAsync();
        var stockDto = stocks.Select(s=> s.ToStockDto());

        return Ok(stocks);
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> GetStockById([FromRoute] int id) 
    {
        var stock = await _stockRepo.GetByIdAsync(id);

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
        await _stockRepo.CreateAsync(stockModel);

        return CreatedAtAction(nameof(GetStockById), new { id = stockModel.Id}, stockModel.ToStockDto());

    }

    [HttpPut]
    [Route("{id}")]
    public async  Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        var stockModel = await _stockRepo.UpdateAsync(id, updateDto);
        if (stockModel == null) { return NotFound(); }

        return Ok(stockModel.ToStockDto());
    }


    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stockModel = await _stockRepo.DeleteAsync(id);

        if (stockModel == null) { return NotFound(); }

      
        return NoContent();
    }

}
