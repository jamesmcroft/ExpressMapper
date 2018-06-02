namespace ExpressMapper.Tests.Models.Classes {
    using System.Collections.Generic;

    using ExpressMapper.Tests.Models.ViewModels;

    public class NonGenericCollectionInhertedFromList: List<TestViewModel> {

    public NonGenericCollectionInhertedFromList( IEnumerable<TestViewModel> models ) : base( models ) { }
  }
}