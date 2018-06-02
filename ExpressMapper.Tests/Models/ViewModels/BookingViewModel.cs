namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class BookingViewModel : IEquatable<BookingViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public CompositionViewModel Composition { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((BookingViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Composition != null ? this.Composition.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(BookingViewModel other)
        {
            return this.Id == other.Id && this.Name == other.Name && ((this.Composition == null && other.Composition == null) || this.Composition.Equals(other.Composition));
        }
    }
}
