using System;
using System.Collections.Generic;
using System.Text;

namespace AutoReservation.BusinessLayer.Exceptions
{
    public class InvaildDateRangException : Exception
    {
        public InvaildDateRangException(string message) : base(message) { }
    }
}
