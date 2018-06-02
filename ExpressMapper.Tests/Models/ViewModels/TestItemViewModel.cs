namespace ExpressMapper.Tests.Models.ViewModels
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

    public class TestItemViewModel
    {
        public TestCollectionViewModel[] Array { get; set; }
        public ICollection<TestCollectionViewModel> Collection { get; set; }
        public IList<TestCollectionViewModel> List { get; set; }
        public IEnumerable<TestCollectionViewModel> Enumerable { get; set; }
        public IQueryable<TestCollectionViewModel> Queryable { get; set; }
        public ObservableCollection<TestCollectionViewModel> ObservableCollection { get; set; }
    }
}
