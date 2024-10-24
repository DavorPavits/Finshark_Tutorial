﻿using FinShark.Data;
using FinShark.DTOS.Stock;
using FinShark.Helpers;
using FinShark.Interfaces;
using FinShark.Models;
using Microsoft.EntityFrameworkCore;

namespace FinShark.Repository;

public class StockRepository : IStockRepository
{
    private readonly ApplicationDBContext _dbContext;

    public StockRepository(ApplicationDBContext dBContext)
    {
        _dbContext = dBContext;

    }

    public async Task<List<Stock>> GetAllAsync(QueryObject query)
    {
        var stocks =  _dbContext.Stocks.Include(c => c.Comments).AsQueryable();

        if(!string.IsNullOrWhiteSpace(query.CompanyName))
        {
            stocks = stocks.Where(s=>s.CompanyName.Contains(query.CompanyName));
        }

        if(!string.IsNullOrWhiteSpace(query.Symbol))
        {
            stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
        }

        if(!string.IsNullOrWhiteSpace(query.SortBy))
        {
            if(query.SortBy.Equals("Symbol", StringComparison.OrdinalIgnoreCase))
            {
                stocks = query.IsDecsending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
            }
        }

        var skipNumber = (query.PageNumber - 1) * query.PageSize;


        return await stocks.Skip(skipNumber).Take(query.PageSize).ToListAsync();
    }

    public async  Task<Stock?> CreateAsync(Stock stockModel)
    {
        stockModel.Id = await _dbContext.Stocks.MaxAsync(e => e.Id) + 1;
        await _dbContext.Stocks.AddAsync(stockModel);
        await _dbContext.SaveChangesAsync();
        return stockModel;
    }

    public async Task<Stock?> DeleteAsync(int id)
    {
        var stockModel = await _dbContext.Stocks.FirstOrDefaultAsync(e => e.Id == id);
        
        _dbContext.Stocks.Remove(stockModel);
        await _dbContext.SaveChangesAsync();
        return stockModel ?? null;
    }

    public async Task<Stock?> GetByIdAsync(int id)
    {
        return await _dbContext.Stocks.Include(c=>c.Comments).FirstOrDefaultAsync(i => i.Id==id);
        
    }

    public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto stockDto)
    {
        var existingStock = await _dbContext.Stocks.FirstOrDefaultAsync(e => e.Id == id);

        existingStock.Symbol = stockDto.Symbol;
        existingStock.CompanyName = stockDto.CompanyName;
        existingStock.Purchase = stockDto.Purchase;
        existingStock.LastDiv = stockDto.LastDiv;
        existingStock.Industry = stockDto.Industry;
        existingStock.MarketCap = stockDto.MarketCap;

        await _dbContext.SaveChangesAsync();
        return existingStock ?? null;
    }

    public Task<bool> StockExists(int id)
    {
       return _dbContext.Stocks.AnyAsync(c => c.Id == id);
    }

    public async Task<Stock?> GetBySymbolAsync(string symbol)
    {
        return await _dbContext.Stocks.FirstOrDefaultAsync(s => s.Symbol == symbol);
    }
}
