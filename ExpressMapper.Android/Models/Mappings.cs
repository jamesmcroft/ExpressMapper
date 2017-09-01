namespace ExpressMapper.Android.Models
{
    public static class Mappings
    {
        public static void Register()
        {
            Mapper.Register<NestedTestModel, NestedTestModel>();
            Mapper.Register<AnotherNestedModel, AnotherNestedModel>();
            Mapper.Register<TestModel, TestModelWrapper>();
            Mapper.Register<TestModelWrapper, TestModel>();
        }
    }
}