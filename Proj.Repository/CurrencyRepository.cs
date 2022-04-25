using Microsoft.EntityFrameworkCore;
using Proj.Core;
using Proj.Core.Domains;
using Proj.Core.DTOs;
using Proj.Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Repository
{
    public class CurrencyRepository : BaseRepository<Currency>, ICurrencyRepository
    {
        public CurrencyRepository(ApplicationDbContext context) : base(context)
        {
        }

        public IEnumerable<object> GetOrderedNCurrencies(int count,OrderType orderType)
        {
            var query = this._context.Currencies.Include(curExch => curExch.CurrenyHistory).Where(p => p.IsActive)
                    .GroupBy(cur => cur.Id, (key,g) => new {
                        Currency = g.First(),
                        Rate = g.First().CurrenyHistory.OrderByDescending(p => p.ExchangeDate).First().Rate,
                    });

            if (orderType == OrderType.Descending)
                return  query.OrderByDescending(cur => cur.Rate)
                    .Take(count);


            return query.OrderBy(cur => cur.Rate)
                    .Take(count);




        }

        public IEnumerable<object> GetOrderedDifferenceRateNCurrenciesBetweenTwoDates(CurrenciesBetweenTwoDates query, OrderType orderType)
        {

            var Query = this._context.ExchangeHistories
                    .Include(curExch => curExch.Currency)
                    .Where(p => p.ExchangeDate >= query.From && p.ExchangeDate <= query.To)
                    .GroupBy(cur => cur.CurrencyId)
                    .Select((g) => new
                    {
                        Currency = g.First().Currency,
                        RateDifference = g.OrderByDescending(p => p.ExchangeDate).First().Rate - g.OrderBy(p => p.ExchangeDate).First().Rate,
                    }).Where(p => p.RateDifference != 0);



            if (orderType == OrderType.Descending)
                return Query.OrderByDescending(cur => cur.RateDifference)
                    .Take(query.Count);


            return Query.OrderBy(cur => cur.RateDifference)
                    .Take(query.Count);
        }
    }
}
