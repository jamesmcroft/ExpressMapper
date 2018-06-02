namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class ProductOptionViewModel : IEquatable<ProductOptionViewModel>
    {
        public Guid Id { get; set; }
        public string Color { get; set; }
        public SizeViewModel Size { get; set; }
        public decimal Price { get; set; }
        public decimal Discount { get; set; }
        public decimal? Weight { get; set; }
        public int Stock { get; set; }
        public decimal DiscountedPrice { get; set; }
        public List<CityViewModel> Cities { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProductOptionViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Color != null ? this.Color.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Size != null ? this.Size.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.Price.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Discount.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Weight.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Stock;
                hashCode = (hashCode * 397) ^ this.DiscountedPrice.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Cities != null ? this.Cities.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(ProductOptionViewModel other)
        {
            var citiesEqual = ((this.Cities != null && other.Cities != null) && this.Cities.Count == other.Cities.Count) || ((other.Cities == null && this.Cities == null));
            if (citiesEqual && this.Cities != null && other.Cities != null && this.Cities.Where((t, i) => !t.Equals(other.Cities[i])).Any())
            {
                citiesEqual = false;
            }

            return this.Id == other.Id && this.Color == other.Color &&
                   (this.Size == null && other.Size == null || this.Size.Equals(other.Size)) && this.Price == other.Price &&
                   this.Discount == other.Discount && this.Weight == other.Weight && this.Stock == other.Stock && this.DiscountedPrice == other.DiscountedPrice && citiesEqual;
        }
    }
}
