using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarDealerAPI.Exceptions
{
    public class ForbiddenExc : Exception
    {
        public ForbiddenExc(string message) : base (message)
        {

        }
    }
}
