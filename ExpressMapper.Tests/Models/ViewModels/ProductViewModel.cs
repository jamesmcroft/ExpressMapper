namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ExpressMapper.Tests.Models.Enums;

    public class ProductViewModel : IEquatable<ProductViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<ProductOptionViewModel> Options { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? WarehouseOn { get; set; }
        public string Ean { get; set; }
        public GenderTypes? OptionalGender { get; set; }
        public BrandViewModel Brand { get; set; }
        public SupplierViewModel Supplier { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ProductViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Description != null ? this.Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Options != null ? this.Options.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.CreatedOn.GetHashCode();
                hashCode = (hashCode * 397) ^ this.WarehouseOn.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Ean != null ? this.Ean.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.OptionalGender.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Brand != null ? this.Brand.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Supplier != null ? this.Supplier.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(ProductViewModel other)
        {
            var optionsEquals = ((this.Options != null && other.Options != null) && this.Options.Count == other.Options.Count) || ((other.Options == null && this.Options == null));
            if (optionsEquals && this.Options != null && other.Options != null && this.Options.Where((t, i) => !t.Equals(other.Options[i])).Any())
            {
                optionsEquals = false;
            }

            return this.Id == other.Id && this.Name == other.Name && this.Description == other.Description && optionsEquals &&
                   this.CreatedOn == other.CreatedOn && this.WarehouseOn == other.WarehouseOn && this.Ean == this.Ean &&
                   this.OptionalGender == other.OptionalGender &&
                   ((this.Brand == null && other.Brand == null) || this.Brand.Equals(other.Brand)) &&
                   ((this.Supplier == null && other.Supplier == null) || this.Supplier.Equals(other.Supplier));
        }
    }
}
