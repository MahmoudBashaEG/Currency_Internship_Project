using Proj.Core;
using Proj.Core.Domains;
using Proj.Core.DTOs;
using Proj.Core.Exceptions;
using Proj.Core.Repositories;
using Proj.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class CurrencyServices : ICurrncyServices
    {
        private readonly ICurrencyRepository _currencyRepository;
        private readonly ICurrencyExchangeRepository _currencyExchangeRepository;

        public CurrencyServices(ICurrencyRepository currencyRepository, ICurrencyExchangeRepository currencyExchangeRepository)
        {
            this._currencyRepository= currencyRepository;
            this._currencyExchangeRepository = currencyExchangeRepository;
        }
        public async Task AddCurrency(NewCurrencyDTO currencyDto)
        {
            var currency = new Currency() { Name = currencyDto.Name.ToUpper(), Sign = currencyDto.Sign.ToUpper() };

            var deletedBefore = await this._currencyRepository.SingleOrDefault(cur => cur.Name == currency.Name && cur.Sign == currency.Sign && !cur.IsActive) ;

            //For Not Adding The Same Data Again If isActice was false
            if (deletedBefore != null)
            {
                currency.Id = deletedBefore.Id;
                currency.IsActive = true;
                this._currencyRepository.Update(currency);
                return;
            }
            var isFoundedBefore = this.CheckIfFoundedBefore(currency);

            if (isFoundedBefore)
                throw new RepeatedException("This Currency is Actually Founded. You Need To Change The Currency Name Or The Sign.");

            await this._currencyRepository.AddAsync(currency);
        }

        public async Task DeleteCurrency(FoundedCurrencyDTO currency)
        {
            var currencyTemp = new Currency() { Id = currency.Id, Name = currency.Name.ToUpper(), Sign = currency.Sign.ToUpper(), IsActive = currency.IsActive };

            var founded = await this._currencyRepository.FindByIdAsync(currencyTemp.Id);

            
            if (founded != null)
            {
                if (currencyTemp.Sign != founded.Sign || currencyTemp.Name != founded.Name || !founded.IsActive)
                    throw new NotFoundException($"There is No Currency With That Name:{currency.Name} And Sign:{currency.Sign}");

                founded.IsActive = false;
                this._currencyRepository.SaveChanges();
            }
            else
            {
                throw new NotFoundException($"There Is No Currency With That Id:{currency.Id}");
            }
            
        }

        public  IEnumerable<Currency> GetAllCurrencies()
        {
            return this._currencyRepository.GetWithFilter(cur => cur.IsActive);
        }

        public async Task<Currency> GetCurrencyByName(string name)
        {
            string tempName = new string(name);
            name = name.ToUpper();

            var cur = await this._currencyRepository.FirstOrDefaultAsync(cur => cur.Name == name && cur.IsActive);

            if (cur == null)
                throw new NotFoundException($"There Is No Currency With That Name:{tempName}");

            return cur;
        }

        public async Task UpdateCurrency(FoundedCurrencyDTO currency)
        {
            var currencyTemp = new Currency() { Id = currency.Id, Name = currency.Name.ToUpper(), Sign = currency.Sign.ToUpper(),IsActive = currency.IsActive };

            var FoundedCurrency = await this._currencyRepository.SingleOrDefault(cur => cur.Id == currencyTemp.Id);

            if (FoundedCurrency == null)
                throw new NotFoundException($"You Can't Update To This Currency Because There isn't No Currency With That Id:{currencyTemp.Id}");

            var isFoundedBefore = this.CheckIfFoundedBefore(currencyTemp);
            if (isFoundedBefore)
                throw new RepeatedException($"This Currency With This Sign:{currencyTemp.Sign} Or Name:{currencyTemp.Name} is Actually Founded You Can't Update To Founded Currency. You Need To Change The Currency.");

            this._currencyRepository.Update(currencyTemp);
        }

        public async Task AddCurrencyExchange(ExchangeHistoryDTO currencyExchangeDto)
        {
            var exchange = new ExchangeHistory() { CurrencyId = currencyExchangeDto.CurrencyId,ExchangeDate = currencyExchangeDto.ExchangeDate,Rate = currencyExchangeDto.Rate };

            if (exchange.Rate <= 0)
                throw new Exception("Rate Must Be Bigger Than 0");
            var cur = await this._currencyRepository.SingleOrDefault(cur => cur.Id == exchange.CurrencyId);

            if (cur == null)
                throw new NotFoundException($"There Isn't Curreny With That Id:{exchange.CurrencyId}");

            await this._currencyExchangeRepository.AddAsync(exchange);

        }

        private bool CheckIfFoundedBefore(Currency currency)
        {
            return this._currencyRepository.Any(cur => ( (cur.Sign == currency.Sign || cur.Name == currency.Name) && cur.IsActive == true));
        }

        public IEnumerable<object> GetHighestOrLowestNCurrencies(int count,OrderType orderType)
        {
            int nFounded = this._currencyRepository.Count(cur => cur.IsActive);
            
            if (nFounded < count || nFounded == 0)
                throw new NotFoundException($"There Are Only {nFounded} Currencies And You Ordered {count} Currencies");

            return this._currencyRepository.GetOrderedNCurrencies(count, orderType);
        }

        public IEnumerable<object> GetHighestOrLowestImprovedNCurrenciesBetweentwoDates(CurrenciesBetweenTwoDates query, OrderType orderType)
        {

            var res = this._currencyRepository.GetOrderedDifferenceRateNCurrenciesBetweenTwoDates(query, orderType);

            int count = res.Count();
            if (count == 0)
                throw new NotFoundException("There Is No Currency Changes Between These Two Dates");

            if (count < query.Count)
                throw new NotFoundException($"Thers Are Only {count} Currency Exchanged Between These Two Dates But You Ordered {query.Count}");

            return res;
        }


        public async Task<float> ConvertCurrency(AmountConvert amount)
        {
            var fromCur = await this._currencyRepository.SingleOrDefault(cur => cur.Sign == amount.FromSign && cur.IsActive);
            var toCur = await this._currencyRepository.SingleOrDefault(cur => cur.Sign == amount.ToSign && cur.IsActive);

            if (fromCur == null)
                throw new NotFoundException($"There Isn't Currency With That Sign:{amount.FromSign}");
            if (toCur == null)
                throw new NotFoundException($"There Isn't Currency With That Sign:{amount.ToSign}");

            // here I am sure that there will be at least one exchangeHistory to added currencies
            var fromExchange = this._currencyExchangeRepository.GetLastExchangeHistoryForSpecificCurrency(fromCur.Id);
            var toExchange = this._currencyExchangeRepository.GetLastExchangeHistoryForSpecificCurrency(toCur.Id);

            float res = ((float)fromExchange.Rate / (float)toExchange.Rate) * amount.Amount;

            return res;
        }
    }
}
