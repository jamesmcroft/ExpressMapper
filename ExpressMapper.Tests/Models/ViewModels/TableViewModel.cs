namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class TableViewModel : IEquatable<TableViewModel>
    {
        public Guid Id;
        public string Name;
        public List<BrandViewModel> Brands;
        public CountryViewModel Manufacturer;
        public List<SizeViewModel> Sizes;

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TableViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Brands != null ? this.Brands.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Manufacturer != null ? this.Manufacturer.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Sizes != null ? this.Sizes.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(TableViewModel other)
        {
            var equals = ((this.Sizes != null && other.Sizes != null) && this.Sizes.Count == other.Sizes.Count) || ((other.Sizes == null && this.Sizes == null));
            if (equals && this.Sizes != null && other.Sizes != null && this.Sizes.Where((t, i) => !t.Equals(other.Sizes[i])).Any())
            {
                equals = false;
            }

            var equalsBrands = ((this.Brands != null && other.Brands != null) && this.Brands.Count == other.Brands.Count) || ((other.Brands == null && this.Brands == null));
            if (equalsBrands && this.Brands != null && other.Brands != null && this.Brands.Where((t, i) => !t.Equals(other.Brands[i])).Any())
            {
                equalsBrands = false;
            }

            return this.Id == other.Id && this.Name == other.Name && equals && equalsBrands && this.Manufacturer.Equals(other.Manufacturer);
        }
    }
}
