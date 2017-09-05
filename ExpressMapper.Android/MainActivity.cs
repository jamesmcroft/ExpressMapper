using Android.App;
using Android.Widget;
using Android.OS;

namespace ExpressMapper.Android
{
    using System.Collections.Generic;
    using System.Linq;

    using ExpressMapper.Android.Models;

    [Activity(Label = "ExpressMapper.Android", MainLauncher = true)]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Mappings.Register();

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var models = new List<TestModel>
                             {
                                 new TestModel(),
                                 new TestModel { Text1 = "Hey!" },
                                 new TestModel
                                     {
                                         AnotherNestedModels =
                                             new List<AnotherNestedModel>
                                                 {
                                                     new
                                                         AnotherNestedModel()
                                                 }
                                     }
                             };

            var mapResults = models.Select(Mapper.Map<TestModel, TestModelWrapper>).ToList();

            var model = new TestModel { Text1 = "Hey!" };

            TestModel originalInstance = Mapper.Map<TestModel, TestModel>(model);

            System.Diagnostics.Debug.WriteLine(originalInstance.ToString());
        }
    }
}