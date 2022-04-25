using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Core.DTOs
{
    public class AmountConvert
    {
        [Required]
        public string FromSign { get; set; } = String.Empty;

        [Required]
        public string ToSign { get; set; } = String.Empty;

        [Required]
        public float Amount { get; set; }
    }
}
