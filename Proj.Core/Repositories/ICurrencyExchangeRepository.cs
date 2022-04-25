using Proj.Core.Domains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Core.Repositories
{
    public interface ICurrencyExchangeRepository : IBaseRepository<ExchangeHistory>
    {
        public ExchangeHistory GetLastExchangeHistoryForSpecificCurrency(int currencyId);

    }
}
