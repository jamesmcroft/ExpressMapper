namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;
    using System.Linq;

    public class ItemModelViewModel : IEquatable<ItemModelViewModel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public SubItemViewModel[] SubItems { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((ItemModelViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.Name != null ? this.Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.SubItems != null ? this.SubItems.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(ItemModelViewModel other)
        {
            var subItems = true;
            if (this.SubItems != null && other.SubItems != null)
            {
                if (this.SubItems.Length == other.SubItems.Length)
                {
                    if (this.SubItems.Where((t, i) => !t.Equals(other.SubItems[i])).Any())
                    {
                        subItems = false;
                    }
                }
            }
            return this.Id == other.Id && this.Name == other.Name && subItems;
        }
    }
}
