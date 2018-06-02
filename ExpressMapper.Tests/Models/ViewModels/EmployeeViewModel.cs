namespace ExpressMapper.Tests.Models.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class EmployeeViewModel : IEquatable<EmployeeViewModel>
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public List<EmployeeViewModel> Employees { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((EmployeeViewModel)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = this.Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (this.FirstName != null ? this.FirstName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.LastName != null ? this.LastName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (this.Employees != null ? this.Employees.GetHashCode() : 0);
                return hashCode;
            }
        }

        public bool Equals(EmployeeViewModel other)
        {
            var subItems = true;
            if (this.Employees != null && other.Employees != null)
            {
                if (this.Employees.Count == other.Employees.Count)
                {
                    if (this.Employees.Where((t, i) => !t.Equals(other.Employees[i])).Any())
                    {
                        subItems = false;
                    }
                }
            }
            return this.Id == other.Id && this.FirstName == other.FirstName && this.LastName == other.LastName && subItems;
        }
    }
}
