namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using ExpressMapper.Tests.Models.Enums;

    public class TestViewModel : IEquatable<TestViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int NotNullable { get; set; }
        public int? Nullable { get; set; }
        public decimal? Weight { get; set; }
        public long Height { get; set; }
        public bool BoolValue { get; set; }
        public CountryViewModel Country { get; set; }
        public List<SizeViewModel> Sizes { get; set; }
        public DateTime Created { get; set; }
        public GenderTypes Gender { get; set; }
        public string NullableGender { get; set; }
        public int GenderIndex { get; set; }
        public List<String> StringCollection { get; set; }
        public string CaSeInSeNsItIvE { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((TestViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.Age;
                hashCode = (hashCode * 397) ^ this.NotNullable;
                hashCode = (hashCode * 397) ^ this.Nullable.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Weight.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Height.GetHashCode();
                hashCode = (hashCode * 397) ^ this.BoolValue.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Country != null ? this.Country.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Sizes != null ? this.Sizes.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.Created.GetHashCode();
                hashCode = (hashCode * 397) ^ (int)this.Gender;
                hashCode = (hashCode * 397) ^ (this.NullableGender != null ? this.NullableGender.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.GenderIndex;
                hashCode = (hashCode * 397) ^ (this.StringCollection != null ? this.StringCollection.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.CaSeInSeNsItIvE != null ? this.CaSeInSeNsItIvE.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(TestViewModel other)
        {
            var sizes = true;
            if (this.Sizes != null && other.Sizes != null)
            {
                if (this.Sizes.Count == other.Sizes.Count)
                {
                    if (this.Sizes.Where((t, i) => !t.Equals(other.Sizes[i])).Any())
                    {
                        sizes = false;
                    }
                }
            }

            return this.Id == other.Id && this.Name == other.Name && this.Age == other.Age && this.NotNullable == other.NotNullable && this.Nullable.GetValueOrDefault() == other.Nullable.GetValueOrDefault() && this.Weight == other.Weight && this.BoolValue == other.BoolValue && this.Gender == other.Gender && this.NullableGender == other.NullableGender && this.GenderIndex == other.GenderIndex && this.CaSeInSeNsItIvE == other.CaSeInSeNsItIvE &&
                   this.Created == other.Created && ((this.Country == null && other.Country == null) || this.Country.Equals(other.Country)) && sizes;
        }
    }
}
