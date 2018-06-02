namespace ExpressMapper
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Text;

    public class CultureAwareStringComparer : IEqualityComparer<string>
    {
        private readonly StringComparison _options;

        public CultureAwareStringComparer(StringComparison options)
        {
            _options = options;
        }

        public bool Equals(string x, string y)
        {
            return x.Equals(y, this._options);
        }

        public int GetHashCode(string obj)
        {
            return obj.GetHashCode();
        }
    }
}
