namespace ExpressMapper.Tests.Models.ViewModels.Structs
{
    using System;

    public class FeatureViewModel : IEquatable<FeatureViewModel>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Rank { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((FeatureViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Description != null ? this.Description.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.Rank.GetHashCode();
                return hashCode;
            }
        }

        public bool Equals(FeatureViewModel other)
        {
            return this.Id == other.Id && this.Name == other.Name && this.Description == other.Description
                   && this.Rank == other.Rank;
        }
    }
}