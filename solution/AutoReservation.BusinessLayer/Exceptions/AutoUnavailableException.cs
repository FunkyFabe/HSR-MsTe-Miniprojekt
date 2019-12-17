using System;
using System.Collections.Generic;
using System.Text;

namespace AutoReservation.BusinessLayer.Exceptions
{
    public class AutoUnavailableException : Exception
    {
        public AutoUnavailableException(string message) : base(message) { }
    }
}
