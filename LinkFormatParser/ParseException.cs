using System;

namespace LinkFormatParser
{
    public class ParseException
        : ArgumentException
    {
        public ParseException(string argument, string message)
            : base(message, argument)
        {
        }
    }
}
