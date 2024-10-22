﻿using FinShark.Data;
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
        //The id of the stock entity is not auto-generated, so in order to deal with the error of violating 
        //the pk constraint in the database, the id is set manually, by finding the last id in the database 
        //and increasing by 1
        stockModel.Id = _dbContext.Stocks.Max(e => e.Id) +1;
        
        _dbContext.Stocks.Add(stockModel);
        _dbContext.SaveChanges();

        return CreatedAtAction(nameof(GetStockById), new { id = stockModel.Id}, stockModel.ToStockDto());

    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
    {
        var stockModel = _dbContext.Stocks.FirstOrDefault(x => x.Id == id);
        if (stockModel == null) { return NotFound(); }

        stockModel.Symbol = updateDto.Symbol;
        stockModel.CompanyName = updateDto.CompanyName;
        stockModel.Purchase = updateDto.Purchase;
        stockModel.LastDiv = updateDto.LastDiv;
        stockModel.Industry = updateDto.Industry; 
        stockModel.MarketCap = updateDto.MarketCap;

        _dbContext.SaveChanges();
        return Ok(stockModel.ToStockDto());
    }


    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete([FromRoute] int id)
    {
        var stockModel = _dbContext.Stocks.FirstOrDefault(x => x.Id == id);

        if (stockModel == null) { return NotFound(); }

        _dbContext.Stocks.Remove(stockModel);
        _dbContext.SaveChanges(true);
        return NoContent();
    }

}