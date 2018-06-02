namespace ExpressMapper.Tests.Models.Mappers
{
    using System.Collections.Generic;

    using ExpressMapper.Tests.Models.Classes;
    using ExpressMapper.Tests.Models.ViewModels;

    public class TestMapper : ICustomTypeMapper<List<TestModel>, List<TestViewModel>>
    {
        public List<TestViewModel> Map(IMappingContext<List<TestModel>, List<TestViewModel>> context)
        {
            var testViewModels = context.Destination ?? new List<TestViewModel>();
            foreach (var testModel in context.Source)
            {
                testViewModels.Add(Mapper.Map<TestModel, TestViewModel>(testModel));
            }

            return testViewModels;
        }
    }
}