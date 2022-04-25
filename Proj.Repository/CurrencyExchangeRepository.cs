using Proj.Core.Domains;
using Proj.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Repository
{
    public class CurrencyExchangeRepository : BaseRepository<ExchangeHistory>, ICurrencyExchangeRepository
    {
        public CurrencyExchangeRepository(ApplicationDbContext context) : base(context)
        {
        }

        public  ExchangeHistory GetLastExchangeHistoryForSpecificCurrency(int currencyId)
        {
           return  this._context.ExchangeHistories.Where(curExch => curExch.CurrencyId == currencyId)
                                .OrderByDescending(cur => cur.ExchangeDate)
                                .FirstOrDefault();

        }
    }
}
