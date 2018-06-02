namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class TripCatalogViewModel : IEquatable<TripCatalogViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public TripTypeViewModel TripType { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TripCatalogViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.TripType != null ? this.TripType.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(TripCatalogViewModel other)
        {
            return this.Id == other.Id && this.Name == other.Name && this.TripType.Equals(other.TripType);
        }
    }
}
