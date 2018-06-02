namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class FashionProductViewModel : ProductViewModel, IEquatable<FashionProductViewModel>
    {
        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FashionProductViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Start.GetHashCode() * 397) ^ this.End.GetHashCode();
            }
        }

        public bool Equals(FashionProductViewModel other)
        {
            var parentEquals = base.Equals(other);
            return this.Start == other.Start && this.End == other.End && parentEquals;
        }
    }
}