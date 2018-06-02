namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class CompositionViewModel : IEquatable<CompositionViewModel>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public BookingViewModel Booking { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((CompositionViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Booking != null ? this.Booking.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(CompositionViewModel other)
        {
            return this.Id == other.Id && this.Name == other.Name
                                       && ((this.Booking == null && other.Booking == null)
                                           || this.Booking.Equals(other.Booking));
        }
    }
}