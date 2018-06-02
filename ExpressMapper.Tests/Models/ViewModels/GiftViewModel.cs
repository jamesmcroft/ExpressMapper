namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class GiftViewModel
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public PersonViewModel Recipient { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GiftViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Recipient != null ? this.Recipient.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(GiftViewModel other)
        {
            return this.Id == other.Id && this.Name == other.Name && (this.Recipient == null || this.Recipient.Equals(other.Recipient));
        }
    }
}
