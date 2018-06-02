namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class TripTypeViewModel: IEquatable<TripTypeViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TripTypeViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (this.Id.GetHashCode() * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
            }
        }

        public bool Equals(TripTypeViewModel other)
        {
            return this.Id == other.Id && this.Name == other.Name;
        }
    }
}
