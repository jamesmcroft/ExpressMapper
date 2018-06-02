namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class SupplierViewModel : IEquatable<SupplierViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime AgreementDate { get; set; }

        public List<SizeViewModel> Sizes { get; set; }
        public int Rank
        {
            get { return this.AgreementDate.Subtract(DateTime.Now).TotalDays < 1 ? 2 : 10; }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SupplierViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.AgreementDate.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Sizes != null ? this.Sizes.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(SupplierViewModel other)
        {
            var equals = ((this.Sizes != null && other.Sizes != null) && this.Sizes.Count == other.Sizes.Count) || ((other.Sizes == null && this.Sizes == null));
            if (equals && this.Sizes != null && other.Sizes != null && this.Sizes.Where((t, i) => !t.Equals(other.Sizes[i])).Any())
            {
                equals = false;
            }
            return this.Id == other.Id && this.Name == other.Name && this.AgreementDate == other.AgreementDate && this.Rank == other.Rank && equals;
        }
    }
}
