using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Core.Domains
{
    public class ExchangeHistory : Base
    {
        public int CurrencyId { get; set; }

        [Required]
        public float Rate { get; set; }

        public DateTime ExchangeDate { get; set; } = DateTime.Now;
        public Currency Currency { get; set; }
    }
}
