using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proj.Core.Exceptions
{
    public class RepeatedException : Exception
    {
        public RepeatedException(string message) : base(message)
        {

        }
    }
}
