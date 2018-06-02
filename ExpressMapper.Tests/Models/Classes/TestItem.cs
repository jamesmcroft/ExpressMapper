namespace ExpressMapper.Tests.Models.Classes
{
    using System.Collections.Generic;
    using System.Linq;

    public class TestItem
    {
        public TestCollection[] Array { get; set; }
        public ICollection<TestCollection> Collection { get; set; }
        public IList<TestCollection> List { get; set; }
        public IEnumerable<TestCollection> Enumerable { get; set; }
        public IQueryable<TestCollection> Queryable { get; set; }
    }
}
