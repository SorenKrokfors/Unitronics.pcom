using System;
using System.Collections.Generic;
using System.Text;

namespace Unitronics.Exceptions
{
    public class MessageException:Exception
    {
        public  MessageException(string message):base(message)
        { }
    }
}
