namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class MailViewModel : IEquatable<MailViewModel>
    {
        public string From { get; set; }

        public ContactViewModel Contact { get; set; }
        public ContactViewModel StandardContactVM { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MailViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (this.From != null ? this.From.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Contact != null ? this.Contact.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.StandardContactVM != null ? this.StandardContactVM.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(MailViewModel other)
        {
            return this.From == other.From && (this.Contact == null || this.Contact.Equals(other.Contact) && (this.StandardContactVM == null || this.StandardContactVM.Equals(other.StandardContactVM)));
        }
    }
}
