using System;

namespace ExpressMapper
{
    public class MapNotImplementedException : ExpressMapperException
    {
        public MapNotImplementedException()
        {
        }

        public MapNotImplementedException(string message)
            : base(message)
        {
        }

        public MapNotImplementedException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}