namespace ExpressMapper.Android.Models
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class TestModel
    {
        public TestModel()
        {
            this.NestedModels = new List<NestedTestModel>();
        }

        public Guid ID { get; set; }

        public string Text1 { get; set; }

        public string Text2 { get; set; }

        public string Text3 { get; set; }

        public string Text4 { get; set; }

        public string Text5 { get; set; }

        public bool Bool1 { get; set; }

        public List<NestedTestModel> NestedModels { get; set; }

        public List<AnotherNestedModel> AnotherNestedModels { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();

            if (string.IsNullOrWhiteSpace(this.Text3) == false)
            {
                sb.Append(this.Text3);
                sb.Append(" ");
            }

            if (string.IsNullOrWhiteSpace(this.Text1) == false)
            {
                sb.Append(this.Text1);
                if (string.IsNullOrWhiteSpace(this.Text2) == false)
                {
                    sb.Append(", ");
                }
            }

            if (string.IsNullOrWhiteSpace(this.Text2) == false)
            {
                sb.Append(this.Text2);
            }

            if (sb.Length == 0)
            {
                return "Unknown";
            }

            return sb.ToString();
        }

        public override bool Equals(object obj)
        {
            if (obj == null) return false;

            if (base.Equals(obj)) return true;

            var otherOfficer = obj as TestModel;
            if (otherOfficer == null) return false;

            if (this.Text1 == null && otherOfficer.Text1 == null)
            {
                return true;
            }

            if (this.Text1 == null && otherOfficer.Text1 != null)
            {
                return false;
            }

            if (this.Text1 != null && otherOfficer.Text1 == null)
            {
                return false;
            }

            return this.Text1 != null && this.Text1.Equals(otherOfficer.Text1);
        }

        public override int GetHashCode()
        {
            return !string.IsNullOrWhiteSpace(this.Text3) ? this.Text3.GetHashCode() : 0;
        }
    }
}