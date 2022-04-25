using Proj.Core.Domains;
using Proj.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Core.Services
{
    public interface ICurrncyServices : IBaseServices
    {
        public Task AddCurrency(NewCurrencyDTO currencyDto);
        public Task AddCurrencyExchange(ExchangeHistoryDTO currencyExchangeDto);
        public IEnumerable<Currency> GetAllCurrencies();
        public Task<Currency> GetCurrencyByName(string name);
        public Task UpdateCurrency(FoundedCurrencyDTO currency);
        public Task DeleteCurrency(FoundedCurrencyDTO currency);
        public IEnumerable<object> GetHighestOrLowestNCurrencies(int count, OrderType orderType);
        public Task<float> ConvertCurrency(AmountConvert amount);
        public IEnumerable<object> GetHighestOrLowestImprovedNCurrenciesBetweentwoDates(CurrenciesBetweenTwoDates query, OrderType orderType);
    }
}
