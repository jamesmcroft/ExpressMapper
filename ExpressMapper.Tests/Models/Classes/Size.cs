namespace ExpressMapper.Tests.Models.Classes
{
    using System;

    public class Size
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public int? NotNullable { get; set; }

        public int Nullable { get; set; }

        public int SortOrder { get; set; }

        public bool BoolValue { get; set; }

        protected bool Equals(Size other)
        {
            return this.Id.Equals(other.Id) && string.Equals(this.Name, other.Name)
                                            && string.Equals(this.Alias, other.Alias)
                                            && this.NotNullable == other.NotNullable && this.Nullable == other.Nullable
                                            && this.SortOrder == other.SortOrder && this.BoolValue == other.BoolValue;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Size)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Alias != null ? this.Alias.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.NotNullable.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Nullable;
                hashCode = (hashCode * 397) ^ this.SortOrder;
                hashCode = (hashCode * 397) ^ this.BoolValue.GetHashCode();
                return hashCode;
            }
        }
    }
}