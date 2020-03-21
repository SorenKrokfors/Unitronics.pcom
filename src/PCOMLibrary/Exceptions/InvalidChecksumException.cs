using System;

namespace Unitronics.Exceptions
{
    public class InvalidChecksumException:Exception
    {
        public InvalidChecksumException(string message):base(message)
        {
        }
    }
    
}
