using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Core.DTOs
{
    public class ExchangeHistoryDTO
    {
        [Required]
        public int CurrencyId { get; set; }

        [Required]
        public float Rate { get; set; }

        public DateTime ExchangeDate { get; set; } = DateTime.Now;
    }
}
