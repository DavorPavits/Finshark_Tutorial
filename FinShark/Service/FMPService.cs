using FinShark.DTOS.Stock;
using FinShark.Interfaces;
using FinShark.Mappers;
using FinShark.Models;
using Newtonsoft.Json;

namespace FinShark.Service;

public class FMPService : IFMPService
{
    private HttpClient _httpClient;
    private IConfiguration _config;

    public FMPService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;
    }

    public async Task<Stock> FindStockBySymbolAsync(string symbol)
    {
        try
        {
            var result = await _httpClient.GetAsync($"https://financialmodelingprep.com/api/v3/profile/{symbol}?apikey={_config["FMPKey"]}");

            if (result.IsSuccessStatusCode)
            {
                var content = await result.Content.ReadAsStringAsync();
                var tasks = JsonConvert.DeserializeObject<FMPStock[]>(content);
                var stock = tasks[0];
                if(stock!=null)
                {
                    return stock.ToStockfromFMP();
                }
                return null;
            }
            return null; 
        
        }catch (Exception ex)
        {
            Console.WriteLine(ex);
            return null;
        }
    }
}
