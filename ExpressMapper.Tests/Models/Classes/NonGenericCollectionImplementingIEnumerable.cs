namespace ExpressMapper.Tests.Models.Classes {
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    using ExpressMapper.Tests.Models.ViewModels;

    public class NonGenericCollectionImplementingIEnumerable: IEnumerable<TestViewModel> {
    private readonly List<TestViewModel> _models;

    public NonGenericCollectionImplementingIEnumerable( IEnumerable<TestViewModel> models ) {
      this._models = models.ToList();
    }

    public IEnumerator<TestViewModel> GetEnumerator() {
      return this._models.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
      return this.GetEnumerator();
    }
  }
}