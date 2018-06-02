namespace ExpressMapper.Tests.Models.Classes
{
    public class Organization : Contact
    {
        public Organization()
        {
            this.IsOrganization = true;
            this.IsPerson = false;
        }

        public string Name { get; set; }
        public Person Relative { get; set; }
    }
}
