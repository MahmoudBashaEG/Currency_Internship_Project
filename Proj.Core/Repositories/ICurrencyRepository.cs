using Proj.Core.Domains;
using Proj.Core.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Core.Repositories
{
    public interface ICurrencyRepository : IBaseRepository<Currency>
    {
        public IEnumerable<object> GetOrderedNCurrencies(int count, OrderType orderType);
        public IEnumerable<object> GetOrderedDifferenceRateNCurrenciesBetweenTwoDates(CurrenciesBetweenTwoDates count, OrderType orderType);
    }
}
