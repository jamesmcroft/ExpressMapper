using System;

namespace ExpressMapper
{
    public class ExpressMapperException : Exception
    {
        public ExpressMapperException()
        {
        }

        public ExpressMapperException(string message)
            : base(message)
        {
        }

        public ExpressMapperException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
