using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Core.DTOs
{
    public class NewCurrencyDTO
    {
        [Required]
        public string Name { get; set; } = String.Empty;

        [Required]
        [MaxLength(5)]
        public string Sign { get; set; } = string.Empty;
    }
}
