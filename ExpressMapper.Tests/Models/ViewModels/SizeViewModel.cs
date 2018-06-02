namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;

    public class SizeViewModel : IEquatable<SizeViewModel>
    {
        private readonly Func<string, string> _fullNameFunc;

        public SizeViewModel()
        {
        }

        public SizeViewModel(Func<string, string> fullNameFunc)
        {
            this._fullNameFunc = fullNameFunc;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public int NotNullable { get; set; }

        public int? Nullable { get; set; }

        public bool BoolValue { get; set; }

        public string Fullname
        {
            get
            {
                return this._fullNameFunc == null
                           ? string.Format("{0} - FULL NAME - {1}", this.Alias, this.Name)
                           : this._fullNameFunc(this.Name);
            }
        }

        public int SortOrder { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((SizeViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Alias != null ? this.Alias.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ this.NotNullable;
                hashCode = (hashCode * 397) ^ this.Nullable.GetHashCode();
                hashCode = (hashCode * 397) ^ this.BoolValue.GetHashCode();
                hashCode = (hashCode * 397) ^ this.SortOrder;
                return hashCode;
            }
        }

        public bool Equals(SizeViewModel other)
        {
            return this.Id == other.Id && this.Name == other.Name && this.Alias == other.Alias
                   && this.NotNullable == other.NotNullable
                   && this.Nullable.GetValueOrDefault() == other.Nullable.GetValueOrDefault()
                   && this.SortOrder == other.SortOrder && this.Fullname == other.Fullname
                   && this.BoolValue == other.BoolValue;
        }
    }
}