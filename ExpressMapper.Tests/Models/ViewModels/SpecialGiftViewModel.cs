namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class SpecialGiftViewModel : GiftViewModel, IEquatable<SpecialGiftViewModel>
    {
        public new SpecialPersonViewModel Recipient { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SpecialGiftViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ (this.Recipient != null ? this.Recipient.GetHashCode() : 0);
            }
        }

        public bool Equals(SpecialGiftViewModel other)
        {
            return this.Recipient.Equals(other.Recipient) && base.Equals(other);
        }
    }
}
