namespace ExpressMapper.Tests.Models.Classes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ExpressMapper.Tests.Models.Enums;

    public class TestModel : IEquatable<TestModel>
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public int? NotNullable { get; set; }

        public int Nullable { get; set; }

        public decimal? Weight { get; set; }

        public long Height { get; set; }

        public bool BoolValue { get; set; }

        public Country Country { get; set; }

        public List<Size> Sizes { get; set; }

        public DateTime Created { get; set; }

        public string Gender { get; set; }

        public GenderTypes? NullableGender { get; set; }

        public string[] StringCollection { get; set; }

        public string CaseInsensitive { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TestModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.Age;
                hashCode = (hashCode * 397) ^ this.NotNullable.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Nullable;
                hashCode = (hashCode * 397) ^ this.Weight.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Height.GetHashCode();
                hashCode = (hashCode * 397) ^ this.BoolValue.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Country != null ? this.Country.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Sizes != null ? this.Sizes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.Created.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Gender != null ? this.Gender.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.NullableGender.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.StringCollection != null ? this.StringCollection.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.CaseInsensitive != null ? this.CaseInsensitive.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(TestModel other)
        {
            bool sizes = !(this.Sizes != null && other.Sizes != null && this.Sizes.Count == other.Sizes.Count
                           && this.Sizes.Where((t, i) => !t.Equals(other.Sizes[i])).Any());

            return this.Id == other.Id && this.Name == other.Name && this.Age == other.Age
                   && this.NotNullable.GetValueOrDefault() == other.NotNullable.GetValueOrDefault()
                   && this.Nullable == other.Nullable && this.Weight == other.Weight
                   && this.BoolValue == other.BoolValue && this.Gender == other.Gender
                   && this.NullableGender == other.NullableGender && this.Created == other.Created
                   && ((this.Country == null && other.Country == null) || this.Country.Equals(other.Country)) && sizes;
        }
    }
}