using System;
using System.Collections.Generic;
using System.Text;

namespace Application.Booking.Exceptions
{
    public class MissingBodyException : Exception
    {
        public MissingBodyException(string message) : base(message) { }
    }
}
