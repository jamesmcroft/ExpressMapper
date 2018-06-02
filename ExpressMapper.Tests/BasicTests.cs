namespace ExpressMapper.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;

    using ExpressMapper.Tests.Common;
    using ExpressMapper.Tests.Models.Classes;
    using ExpressMapper.Tests.Models.Enums;
    using ExpressMapper.Tests.Models.Generator;
    using ExpressMapper.Tests.Models.Mappers;
    using ExpressMapper.Tests.Models.Structs;
    using ExpressMapper.Tests.Models.ViewModels;
    using ExpressMapper.Tests.Models.ViewModels.Structs;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    using Moq;

    [TestClass]
    public class BasicTests : BaseTestClass
    {
        [TestMethod]
        public void EnumToAnotherEnumMapTest()
        {
            Mapper.Register<UnitOfWork, UnitOfWorkViewModel>();
            Mapper.Compile();

            UnitOfWork unitOfWork = new UnitOfWork { Id = Guid.NewGuid(), State = States.InProgress };

            UnitOfWorkViewModel unitOfWorkViewModel = Mapper.Map<UnitOfWork, UnitOfWorkViewModel>(unitOfWork);
            Assert.AreEqual((int)unitOfWork.State, (int)unitOfWorkViewModel.State);
            Assert.AreEqual(unitOfWork.Id, unitOfWorkViewModel.Id);
        }

        [TestMethod]
        public void ParallelPrecompileCollectionTest()
        {
            Mapper.Register<Composition, CompositionViewModel>().Member(dest => dest.Booking, src => src.Booking);
            Mapper.Register<Booking, BookingViewModel>();
            Mapper.Compile();

            List<Action> actions = new List<Action>();

            for (int i = 0; i < 100; i++)
            {
                actions.Add(Mapper.PrecompileCollection<List<Booking>, IEnumerable<BookingViewModel>>);
            }

            Parallel.Invoke(actions.ToArray());
        }

        [TestMethod]
        public void DefaultPrimitiveTypePropertyToStringTest()
        {
            Mapper.Register<TestDefaultDecimal, TestDefaultDecimalToStringViewModel>().Member(
                dest => dest.TestString,
                src => src.TestDecimal);
            Mapper.RegisterCustom<decimal, string>(src => src.ToString("#0.00", CultureInfo.InvariantCulture));
            Mapper.Compile();
            TestDefaultDecimal test = new TestDefaultDecimal() { TestDecimal = default(decimal) };
            test.TestDecimal = default(decimal);
            TestDefaultDecimalToStringViewModel result =
                Mapper.Map<TestDefaultDecimal, TestDefaultDecimalToStringViewModel>(test);
            Assert.AreEqual("0.00", result.TestString);
        }

        [TestMethod]
        public void MemberMappingsHasHigherPriorityThanCaseSensetiveTest()
        {
            Mapper.Register<Source, TargetViewModel>().Member(t => t.Enabled, s => s.enabled == "Y");
            Mapper.Compile();

            Source source = new Source { enabled = "N" };
            TargetViewModel result = Mapper.Map<Source, TargetViewModel>(source);
            Assert.AreEqual(false, result.Enabled);
        }

        [TestMethod]
        public void FieldsTest()
        {
            Mapper.Register<Brand, BrandViewModel>();
            Mapper.Register<Table, TableViewModel>();
            Mapper.Register<Size, SizeViewModel>();
            Mapper.Register<Country, CountryViewModel>();
            Mapper.Compile();

            KeyValuePair<Table, TableViewModel> srcAndDest = Functional.FieldsTestMap();

            TableViewModel bvm = Mapper.Map<Table, TableViewModel>(srcAndDest.Key);

            Assert.AreEqual(bvm, srcAndDest.Value);
        }

        [TestMethod]
        public void CustomMemberAndFunctionFieldsTest()
        {
            Mapper.Register<Brand, BrandViewModel>();
            Mapper.Register<Table, TableViewModel>().Member(t => t.Id, t => t.Id).Function(t => t.Name, t => t.Name);
            Mapper.Register<Size, SizeViewModel>();
            Mapper.Register<Country, CountryViewModel>();
            Mapper.Compile();

            KeyValuePair<Table, TableViewModel> srcAndDest = Functional.FieldsTestMap();

            TableViewModel bvm = Mapper.Map<Table, TableViewModel>(srcAndDest.Key);

            Assert.AreEqual(bvm, srcAndDest.Value);
        }

        [TestMethod]
        public void MappingOrderLessTest()
        {
            Mapper.Register<Composition, CompositionViewModel>().Member(dest => dest.Booking, src => src.Booking);
            Mapper.Register<Booking, BookingViewModel>();

            KeyValuePair<Booking, BookingViewModel> srcAndDest = Functional.RecursiveCompilationAssociationTestMap();

            BookingViewModel bvm = Mapper.Map<Booking, BookingViewModel>(srcAndDest.Key);

            Assert.AreEqual(bvm, srcAndDest.Value);
        }

        [TestMethod]
        public void RecursiveCompilationDirectCollectionTest()
        {
            Mapper.Register<Employee, EmployeeViewModel>();
            Mapper.Compile();

            KeyValuePair<Employee, EmployeeViewModel> srcAndDest =
                Functional.RecursiveCompilationDirectCollectionTestMap();

            EmployeeViewModel bvm = Mapper.Map<Employee, EmployeeViewModel>(srcAndDest.Key);

            Assert.AreEqual(bvm, srcAndDest.Value);
        }

        [TestMethod]
        public void RecursiveCompilationDirectAssociationTest()
        {
            Mapper.Register<Person, PersonViewModel>();
            Mapper.Compile();

            KeyValuePair<Person, PersonViewModel>
                srcAndDest = Functional.RecursiveCompilationDirectAssociationTestMap();
            PersonViewModel bvm = Mapper.Map<Person, PersonViewModel>(srcAndDest.Key);

            Assert.AreEqual(bvm, srcAndDest.Value);
        }

        [TestMethod]
        public void RecursiveCompilationAssociationTest()
        {
            Mapper.Register<Booking, BookingViewModel>();
            Mapper.Register<Composition, CompositionViewModel>();
            Mapper.Compile();

            KeyValuePair<Booking, BookingViewModel> srcAndDest = Functional.RecursiveCompilationAssociationTestMap();

            BookingViewModel bvm = Mapper.Map<Booking, BookingViewModel>(srcAndDest.Key);

            Assert.AreEqual(bvm, srcAndDest.Value);
        }

        [TestMethod]
        public void RecursiveCompilationCollectionTest()
        {
            Mapper.Register<Engine, EngineViewModel>();
            Mapper.Register<Cylinder, CylinderViewModel>();
            Mapper.Compile();

            KeyValuePair<Engine, EngineViewModel> srcAndDest = Functional.RecursiveCompilationCollectionTestMap();
            EngineViewModel engineViewModel = Mapper.Map<Engine, EngineViewModel>(srcAndDest.Key);
            Assert.AreEqual(engineViewModel, srcAndDest.Value);
        }

        [TestMethod]
        public void DynamicMapCollectionPropertyTest()
        {
            Mapper.Register<Engine, EngineViewModel>();
            Mapper.Compile();

            KeyValuePair<Engine, EngineViewModel> srcAndDest = Functional.RecursiveCompilationCollectionTestMap();
            EngineViewModel engineViewModel = Mapper.Map<Engine, EngineViewModel>(srcAndDest.Key);
            Assert.AreEqual(engineViewModel, srcAndDest.Value);
        }


        [TestMethod]
        public void DirectDynamicMapTest()
        {
            KeyValuePair<Engine, EngineViewModel> srcAndDest = Functional.RecursiveCompilationCollectionTestMap();
            EngineViewModel engineViewModel = Mapper.Map<Engine, EngineViewModel>(srcAndDest.Key);
            Assert.AreEqual(engineViewModel, srcAndDest.Value);
        }


        [TestMethod]
        public void CompilelessMap()
        {
            Mapper.Register<TestModel, TestViewModel>();
            Mapper.Register<Size, SizeViewModel>();
            Mapper.Register<Country, CountryViewModel>();

            KeyValuePair<TestModel, TestViewModel> test = Functional.AutoMemberMap();

            TestViewModel testViewModel = Mapper.Map<TestModel, TestViewModel>(test.Key);

            Assert.AreEqual(testViewModel, test.Value);
        }

        [TestMethod]
        public void DynamicMapTest()
        {
            KeyValuePair<TestModel, TestViewModel> test = Functional.AutoMemberMap();
            TestViewModel testViewModel = Mapper.Map<TestModel, TestViewModel>(test.Key);
            Assert.AreEqual(testViewModel, test.Value);
        }

        [TestMethod]
        public void AutoMemberMap()
        {
            Mapper.Register<TestModel, TestViewModel>();
            Mapper.Register<Size, SizeViewModel>();
            Mapper.Register<Country, CountryViewModel>();
            Mapper.Compile();

            KeyValuePair<TestModel, TestViewModel> test = Functional.AutoMemberMap();

            TestViewModel testViewModel = Mapper.Map<TestModel, TestViewModel>(test.Key);

            Assert.AreEqual(testViewModel, test.Value);
        }

        [TestMethod]
        public void AutoMemberMapDeepCopy()
        {
            Mapper.Register<TestModel, TestModel>();
            Mapper.Register<Size, Size>();
            Mapper.Register<Country, Country>();
            Mapper.Compile();

            KeyValuePair<TestModel, TestViewModel> test = Functional.AutoMemberMap();

            TestModel deepCopy = Mapper.Map<TestModel, TestModel>(test.Key);

            Assert.AreNotEqual(deepCopy.GetHashCode(), test.Key.GetHashCode());
            Assert.AreNotEqual(deepCopy.Country.GetHashCode(), test.Key.Country.GetHashCode());
            for (int i = 0; i < deepCopy.Sizes.Count; i++)
            {
                Assert.AreNotEqual(deepCopy.Sizes[i].GetHashCode(), test.Key.Sizes[i].GetHashCode());
            }

            Assert.AreEqual(deepCopy, test.Key);
        }

        [TestMethod]
        public void ManualPrimitiveMemberMap()
        {
            Mapper.Register<Size, SizeViewModel>()
                .Member(src => src.Name, dest => string.Format("Full - {0} - Size", dest.Alias))
                .Member(src => src.SortOrder, dest => dest.Id.GetHashCode())
                .Member(src => src.Nullable, dest => dest.Nullable)
                .Member(src => src.NotNullable, dest => dest.NotNullable).Member(
                    src => src.BoolValue,
                    dest => dest.BoolValue);
            Mapper.Compile();

            KeyValuePair<Size, SizeViewModel> sizeResult = Functional.ManualPrimitiveMemberMap();

            SizeViewModel result = Mapper.Map<Size, SizeViewModel>(sizeResult.Key);

            Assert.AreEqual(result, sizeResult.Value);
        }

        [TestMethod]
        public void HiddenInheritedMemberMap()
        {
            Mapper.Register<SpecialPerson, SpecialPersonViewModel>();

            IMemberConfiguration<SpecialGift, SpecialGiftViewModel> mapConfig =
                Mapper.Register<SpecialGift, SpecialGiftViewModel>();
            MapBaseMember(mapConfig);
            mapConfig.Member(src => src.Recipient, dst => dst.Recipient);

            Mapper.Compile();

            KeyValuePair<SpecialGift, SpecialGiftViewModel> srcDst = Functional.HiddenInheritedMemberMap();

            SpecialGiftViewModel result = Mapper.Map<SpecialGift, SpecialGiftViewModel>(srcDst.Key);

            Assert.AreEqual(result, srcDst.Value);
        }

        private void MapBaseMember<T, TN>(IMemberConfiguration<T, TN> mapConfig)
            where T : Gift where TN : GiftViewModel
        {
            mapConfig.Member(src => src.Recipient, dst => dst.Recipient);
        }

        [TestMethod]
        public void ManualConstantMemberMap()
        {
            Mapper.Register<Size, SizeViewModel>().Value(src => src.SortOrder, 123).Value(src => src.BoolValue, true);
            Mapper.Compile();

            KeyValuePair<Size, SizeViewModel> sizeResult = Functional.ManualPrimitiveMemberMap();

            SizeViewModel result = Mapper.Map<Size, SizeViewModel>(sizeResult.Key);

            Assert.AreEqual(result.SortOrder, 123);
            Assert.AreEqual(result.BoolValue, true);
        }

        [TestMethod]
        public void ManualNestedNotNullMemberMap()
        {
            Mapper.Register<Trip, TripViewModel>().Member(src => src.Name, dest => dest.Category.Name)
                .Ignore(x => x.Category);
            Mapper.Compile();

            Trip source = new Trip() { Category = new CategoryTrip() { Name = "TestCat123" }, Name = "abc" };

            TripViewModel result = Mapper.Map<Trip, TripViewModel>(source);

            Assert.IsNull(result.Category);
            Assert.AreEqual(result.Name, "TestCat123");
        }

        [TestMethod]
        public void ManualNestedNullMemberMap()
        {
            Mapper.Register<Trip, TripViewModel>().Member(src => src.Name, dest => dest.Category.Name)
                .Ignore(x => x.Category);
            Mapper.Compile();

            Trip source = new Trip() { Name = "abc" };

            TripViewModel result = Mapper.Map<Trip, TripViewModel>(source);

            Assert.IsNull(result.Category);
            Assert.IsNull(result.Name);
        }

        [TestMethod]
        public void InstantiateMap()
        {
            Mapper.Register<Size, SizeViewModel>().Instantiate(
                src => new SizeViewModel(s => string.Format("{0} - Full name - {1}", src.Id, s)));
            Mapper.Compile();

            KeyValuePair<Size, SizeViewModel> sizeResult = Functional.InstantiateMap();

            SizeViewModel result = Mapper.Map<Size, SizeViewModel>(sizeResult.Key);

            Assert.AreEqual(result, sizeResult.Value);
        }

        [TestMethod]
        public void InstantiateFuncMap()
        {
            Mapper.Register<Size, SizeViewModel>().InstantiateFunc(
                src => { return new SizeViewModel(s => string.Format("{0} - Full name - {1}", src.Id, s)); });
            Mapper.Compile();

            KeyValuePair<Size, SizeViewModel> sizeResult = Functional.InstantiateMap();

            SizeViewModel result = Mapper.Map<Size, SizeViewModel>(sizeResult.Key);

            Assert.AreEqual(result, sizeResult.Value);
        }

        [TestMethod]
        public void IgnoreMap()
        {
            Mapper.Register<Size, SizeViewModel>().Ignore(dest => dest.Name);
            Mapper.Compile();

            KeyValuePair<Size, SizeViewModel> sizeResult = Functional.IgnoreMap();
            SizeViewModel result = Mapper.Map<Size, SizeViewModel>(sizeResult.Key);
            Assert.AreEqual(result, sizeResult.Value);
        }

        [TestMethod]
        public void BeforeMap()
        {
            Mapper.Register<Size, SizeViewModel>().Before((src, dest) => dest.Name = src.Name)
                .Ignore(dest => dest.Name);
            Mapper.Compile();

            KeyValuePair<Size, SizeViewModel> sizeResult = Functional.BeforeMap();
            SizeViewModel result = Mapper.Map<Size, SizeViewModel>(sizeResult.Key);
            Assert.AreEqual(result, sizeResult.Value);
        }

        [TestMethod]
        public void BeforeMapDuplicateTest()
        {
            var exception = Assert.ThrowsException<InvalidOperationException>(
                () => Mapper.Register<Size, SizeViewModel>().Before((src, dest) => dest.Name = src.Name)
                    .Before((src, dest) => dest.Name = src.Name).Ignore(dest => dest.Name));
            Assert.AreEqual($"BeforeMap already registered for {typeof(Size).FullName}", exception.Message);

            KeyValuePair<Size, SizeViewModel> sizeResult = Functional.BeforeMap();
            SizeViewModel result = Mapper.Map<Size, SizeViewModel>(sizeResult.Key);
            Assert.AreEqual(result, sizeResult.Value);
        }

        [TestMethod]
        public void AfterMap()
        {
            Mapper.Register<Size, SizeViewModel>().After((src, dest) => dest.Name = "OVERRIDE BY AFTER MAP");
            Mapper.Compile();
            KeyValuePair<Size, SizeViewModel> sizeResult = Functional.AfterMap();
            SizeViewModel result = Mapper.Map<Size, SizeViewModel>(sizeResult.Key);
            Assert.AreEqual(result, sizeResult.Value);
        }

        [TestMethod]
        public void AfterMapDuplicateTest()
        {
            var exception = Assert.ThrowsException<InvalidOperationException>(
                () => Mapper.Register<Size, SizeViewModel>().After((src, dest) => dest.Name = "OVERRIDE BY AFTER MAP")
                    .After((src, dest) => dest.Name = "Duplicate map"));
            Assert.AreEqual($"AfterMap already registered for {typeof(Size).FullName}", exception.Message);
            KeyValuePair<Size, SizeViewModel> sizeResult = Functional.AfterMap();
            SizeViewModel result = Mapper.Map<Size, SizeViewModel>(sizeResult.Key);
            Assert.AreEqual(result, sizeResult.Value);
        }


        [TestMethod]
        public void CustomMapWithSupportedCollectionMaps()
        {
            Mapper.RegisterCustom<Size, SizeViewModel, SizeMapper>();
            Mapper.Compile();
            KeyValuePair<Size, SizeViewModel> sizeResult = Functional.CustomMap();
            SizeViewModel result = Mapper.Map<Size, SizeViewModel>(sizeResult.Key);
            Assert.AreEqual(result, sizeResult.Value);
        }

        [TestMethod]
        public void CustomMapWithSupportedNestedCollectionMaps()
        {
            Mapper.Register<TestModel, TestViewModel>();
            Mapper.Register<Country, CountryViewModel>();
            Mapper.RegisterCustom<Size, SizeViewModel, SizeMapper>();
            Mapper.Compile();
            Tuple<TestModel, TestViewModel, TestViewModel> sizeResult = Functional.CustomNestedCollectionMap();
            var result = Mapper.Map<TestModel, TestViewModel>(sizeResult.Item1);
            Assert.AreEqual(result, sizeResult.Item3);
        }

        [TestMethod]
        public void DeepCopyCustomMapWithSupportedNestedCollectionMaps()
        {
            Mapper.Register<TestModel, TestModel>();
            Mapper.Register<Country, Country>();
            Mapper.RegisterCustom<Size, Size, DeepCopySizeMapper>();
            Mapper.Compile();
            Tuple<TestModel, TestViewModel, TestViewModel> sizeResult = Functional.CustomNestedCollectionMap();
            TestModel deepCopy = Mapper.Map<TestModel, TestModel>(sizeResult.Item1);

            Assert.AreNotEqual(deepCopy.GetHashCode(), sizeResult.Item1.GetHashCode());
            Assert.AreNotEqual(deepCopy.Country.GetHashCode(), sizeResult.Item1.Country.GetHashCode());
            for (int i = 0; i < deepCopy.Sizes.Count; i++)
            {
                Assert.AreNotEqual(deepCopy.Sizes[i].GetHashCode(), sizeResult.Item1.Sizes[i].GetHashCode());
            }

            Assert.AreEqual(deepCopy, sizeResult.Item1);
        }

        [TestMethod]
        public void CustomMapListWithSupportedNestedCollectionMaps()
        {
            Mapper.Register<TestModel, TestViewModel>();
            Mapper.Register<Country, CountryViewModel>();
            Mapper.RegisterCustom<List<Size>, List<SizeViewModel>, SizeListMapper>();
            Mapper.Compile();
            Tuple<TestModel, TestViewModel, TestViewModel> sizeResult = Functional.CustomNestedCollectionMap();
            TestViewModel result = Mapper.Map<TestModel, TestViewModel>(sizeResult.Item1);
            Assert.AreEqual(result, sizeResult.Item3);
        }

        [TestMethod]
        public void ExistingDestCustomMapWithSupportedNestedCollectionMaps()
        {
            Mapper.Register<TestModel, TestViewModel>();
            Mapper.Register<Country, CountryViewModel>();
            Mapper.RegisterCustom<Size, SizeViewModel, SizeMapper>();
            Mapper.Compile();
            Tuple<TestModel, TestViewModel, TestViewModel> sizeResult = Functional.CustomNestedCollectionMap();
            TestViewModel result = Mapper.Map(sizeResult.Item1, sizeResult.Item2);
            Assert.AreEqual(result, sizeResult.Item3);
        }

        [TestMethod]
        public void ExistingDestCustomMapListWithSupportedNestedCollectionMaps()
        {
            Mapper.Register<TestModel, TestViewModel>();
            Mapper.Register<Country, CountryViewModel>();
            Mapper.RegisterCustom<List<Size>, List<SizeViewModel>, SizeListMapper>();
            Mapper.Compile();
            Tuple<TestModel, TestViewModel, TestViewModel> sizeResult = Functional.CustomNestedCollectionMap();
            TestViewModel result = Mapper.Map(sizeResult.Item1, sizeResult.Item2);
            Assert.AreEqual(result, sizeResult.Item3);
        }

        [TestMethod]
        public void NullPropertyAndNullCollectionPropertyMaps()
        {
            Mapper.Register<TestModel, TestViewModel>();
            Mapper.Register<Country, CountryViewModel>();
            Mapper.Register<Size, SizeViewModel>();

            Mapper.Compile();
            KeyValuePair<TestModel, TestViewModel> sizeResult = Functional.NullPropertyAndNullCollectionMap();
            TestViewModel result = Mapper.Map<TestModel, TestViewModel>(sizeResult.Key);
            Assert.AreEqual(result, sizeResult.Value);
        }

        [TestMethod]
        public void OnlyGetPropertyMaps()
        {
            Mapper.Register<Supplier, SupplierViewModel>();
            Mapper.Register<Size, SizeViewModel>();
            Mapper.Compile();
            KeyValuePair<Supplier, SupplierViewModel> supplierResult = Functional.GetPropertyMaps();
            SupplierViewModel result = Mapper.Map<Supplier, SupplierViewModel>(supplierResult.Key);
            Assert.AreEqual(result, supplierResult.Value);
        }

        [TestMethod]
        public void OnlyGetWithManualPropertyMaps()
        {
            Mapper.Register<Supplier, SupplierViewModel>().Member(dest => dest.Rank, src => src.Rank);
            Mapper.Register<Size, SizeViewModel>();
            Mapper.Compile();
            KeyValuePair<Supplier, SupplierViewModel> supplierResult = Functional.GetPropertyMaps();
            SupplierViewModel result = Mapper.Map<Supplier, SupplierViewModel>(supplierResult.Key);
            Assert.AreEqual(result, supplierResult.Value);
        }

        [TestMethod]
        public void CustomMap()
        {
            Mapper.RegisterCustom<GenderTypes, string>(g => g.ToString());
            Mapper.Compile();

            string result = Mapper.Map<GenderTypes, string>(GenderTypes.Men);
            Assert.AreEqual(result, GenderTypes.Men.ToString());
        }

        [TestMethod]
        public void AutoMemberStructMap()
        {
            Mapper.Register<Item, ItemViewModel>();
            Mapper.Compile();
            KeyValuePair<Item, ItemViewModel> testData = Functional.AutoMemberStructMap();

            ItemViewModel result = Mapper.Map<Item, ItemViewModel>(testData.Key);
            Assert.AreEqual(result, testData.Value);
        }

        [TestMethod]
        public void StructWithCollectionMap()
        {
            Mapper.Register<Feature, FeatureViewModel>();
            Mapper.Register<Item, ItemViewModel>().Member(dest => dest.FeatureList, src => src.Features);
            Mapper.Compile();
            KeyValuePair<Item, ItemViewModel> testData = Functional.StructWithCollectionMap();

            ItemViewModel result = Mapper.Map<Item, ItemViewModel>(testData.Key);
            Assert.AreEqual(result, testData.Value);
        }

        [TestMethod]
        public void ComplexMap()
        {
            Mapper.Register<FashionProduct, FashionProductViewModel>().Function(
                dest => dest.OptionalGender,
                src =>
                    {
                        GenderTypes? optionalGender;
                        switch (src.Gender)
                        {
                            case GenderTypes.Unisex:
                                optionalGender = null;
                                break;
                            default:
                                optionalGender = src.Gender;
                                break;
                        }

                        return optionalGender;
                    });
            Mapper.Register<ProductOption, ProductOptionViewModel>();
            Mapper.Register<Size, SizeViewModel>();
            Mapper.Register<Feature, FeatureViewModel>();
            Mapper.Register<City, CityViewModel>().Member(dest => dest.FeaturesList, src => src.Features);
            Mapper.Register<Supplier, SupplierViewModel>();
            Mapper.Register<Brand, BrandViewModel>();

            Mapper.Compile();
            KeyValuePair<FashionProduct, FashionProductViewModel> testData = Functional.ComplexMap();

            FashionProductViewModel result = Mapper.Map<FashionProduct, FashionProductViewModel>(testData.Key);
            bool valid = result.Equals(testData.Value);
            Assert.IsTrue(valid);
        }

        [TestMethod]
        public void ListToArray()
        {
            Mapper.Register<TestCollection, TestCollectionViewModel>();
            Mapper.Register<TestItem, TestItemViewModel>().Member(dest => dest.Array, src => src.List)
                .Ignore(dest => dest.Collection).Ignore(dest => dest.Enumerable).Ignore(dest => dest.List)
                .Ignore(dest => dest.Queryable);

            Mapper.Compile();

            KeyValuePair<TestItem, TestItemViewModel> typeCollTest = Functional.CollectionTypeMap();
            TestItemViewModel result = Mapper.Map<TestItem, TestItemViewModel>(typeCollTest.Key);
            Assert.AreEqual(result.Array.Length, typeCollTest.Key.List.Count);
        }

        [TestMethod]
        public void ListNullToArray()
        {
            Mapper.Register<TestCollection, TestCollectionViewModel>();
            Mapper.Register<TestItem, TestItemViewModel>().Member(dest => dest.Array, src => src.List)
                .Ignore(dest => dest.Collection).Ignore(dest => dest.Enumerable).Ignore(dest => dest.List)
                .Ignore(dest => dest.Queryable);

            Mapper.Compile();

            TestItem typeCollTest = new TestItem() { List = null };

            TestItemViewModel result = Mapper.Map<TestItem, TestItemViewModel>(typeCollTest);

            Assert.IsNull(result.Array);
        }

        [TestMethod]
        public void ListToQueriable()
        {
            Mapper.Register<TestCollection, TestCollectionViewModel>();
            Mapper.Register<TestItem, TestItemViewModel>().Member(dest => dest.Queryable, src => src.List)
                .Ignore(dest => dest.Collection).Ignore(dest => dest.List).Ignore(dest => dest.Array)
                .Ignore(dest => dest.Enumerable);

            Mapper.Compile();

            KeyValuePair<TestItem, TestItemViewModel> typeCollTest = Functional.CollectionTypeMap();
            TestItemViewModel result = Mapper.Map<TestItem, TestItemViewModel>(typeCollTest.Key);
            Assert.AreEqual(result.Queryable.Count(), typeCollTest.Key.List.Count());
        }

        [TestMethod]
        public void EnumerableToQueriable()
        {
            Mapper.Register<TestCollection, TestCollectionViewModel>();
            Mapper.Register<TestItem, TestItemViewModel>().Member(dest => dest.Queryable, src => src.Enumerable)
                .Ignore(dest => dest.Collection).Ignore(dest => dest.List).Ignore(dest => dest.Array)
                .Ignore(dest => dest.Enumerable);

            Mapper.Compile();

            KeyValuePair<TestItem, TestItemViewModel> typeCollTest = Functional.CollectionTypeMap();
            TestItemViewModel result = Mapper.Map<TestItem, TestItemViewModel>(typeCollTest.Key);
            Assert.AreEqual(result.Queryable.Count(), typeCollTest.Key.Enumerable.Count());
        }

        [TestMethod]
        public void QueryableToArray()
        {
            Mapper.Register<TestCollection, TestCollectionViewModel>();
            Mapper.Register<TestItem, TestItemViewModel>().Member(dest => dest.Array, src => src.Queryable)
                .Ignore(dest => dest.Collection).Ignore(dest => dest.List).Ignore(dest => dest.Queryable)
                .Ignore(dest => dest.Enumerable);

            Mapper.Compile();

            KeyValuePair<TestItem, TestItemViewModel> typeCollTest = Functional.CollectionTypeMap();
            TestItemViewModel result = Mapper.Map<TestItem, TestItemViewModel>(typeCollTest.Key);
            Assert.AreEqual(result.Array.Count(), typeCollTest.Key.Queryable.Count());
        }

        [TestMethod]
        public void NonGenericSimpleMap()
        {
            Mapper.Register<TestModel, TestViewModel>();
            Mapper.Register<Size, SizeViewModel>();
            Mapper.Register<Country, CountryViewModel>();
            Mapper.Compile();

            KeyValuePair<TestModel, TestViewModel> test = Functional.AutoMemberMap();

            TestViewModel testViewModel =
                Mapper.Map(test.Key, typeof(TestModel), typeof(TestViewModel)) as TestViewModel;

            Assert.AreEqual(testViewModel, test.Value);
        }

        [TestMethod]
        public void NonGenericSimpleWithDestinationMap()
        {
            Mapper.Register<TestModel, TestViewModel>();
            Mapper.Register<Size, SizeViewModel>();
            Mapper.Register<Country, CountryViewModel>();
            Mapper.Compile();

            KeyValuePair<TestModel, TestViewModel> test = Functional.AutoMemberMap();

            int resultInstanceHash = test.Value.GetHashCode();
            TestViewModel testViewModel =
                Mapper.Map(test.Key, test.Value, typeof(TestModel), typeof(TestViewModel)) as TestViewModel;

            Assert.AreEqual(testViewModel.GetHashCode(), resultInstanceHash);
            Assert.AreEqual(testViewModel, test.Value);
        }

        [TestMethod]
        public void CustomMapNonGeneric()
        {
            Mapper.RegisterCustom<Size, SizeViewModel, SizeMapper>();
            Mapper.Compile();
            KeyValuePair<Size, SizeViewModel> sizeResult = Functional.CustomMap();
            object result = Mapper.Map(sizeResult.Key, typeof(Size), typeof(SizeViewModel));
            Assert.AreEqual(result, sizeResult.Value);
        }

        [TestMethod]
        public void AccessSourceNestedProperty()
        {
            Mapper.Register<TestModel, TestViewModel>().Member(
                dest => dest.Name,
                src => $"Test - {src.Country.Name} - and date: {DateTime.Now} plus {src.Country.Code}");
            Mapper.Register<Country, CountryViewModel>();
            Mapper.Register<Size, SizeViewModel>();

            Mapper.Compile();

            KeyValuePair<TestModel, TestViewModel> sizeResult = Functional.AccessSourceNestedProperty();

            TestViewModel result = Mapper.Map<TestModel, TestViewModel>(sizeResult.Key);
            Assert.AreEqual(result, sizeResult.Value);
        }

        [TestMethod]
        public void AccessSourceManyNestedProperties()
        {
            Mapper.Register<Trip, TripViewModel>().Member(
                    dest => dest.Name,
                    src =>
                        $"Type: {src.Category.Catalog.TripType.Name}, Catalog: {src.Category.Catalog.Name}, Category: {src.Category.Name}")
                .Ignore(dest => dest.Category);

            Mapper.Compile();

            KeyValuePair<Trip, TripViewModel> sizeResult = Functional.AccessSourceManyNestedProperties();

            TripViewModel result = Mapper.Map<Trip, TripViewModel>(sizeResult.Key);
            Assert.AreEqual(result, sizeResult.Value);
        }

        [TestMethod]
        public void ExistingDestinationSimple()
        {
            Mapper.Register<TestModel, TestViewModel>();
            Mapper.Register<Country, CountryViewModel>();
            Mapper.Register<Size, SizeViewModel>();

            KeyValuePair<TestModel, TestViewModel> sizeResult = Functional.ExistingDestinationSimpleMap();

            int testObjHash = sizeResult.Value.GetHashCode();
            int countryHash = sizeResult.Value.Country.GetHashCode();
            int sizesHash = sizeResult.Value.Sizes.GetHashCode();
            List<int> sizeHashesList = new List<int>(sizeResult.Value.Sizes.Count);
            sizeHashesList.AddRange(sizeResult.Value.Sizes.Select(size => size.GetHashCode()));

            var result = Mapper.Map(sizeResult.Value, sizeResult.Key);
            Assert.AreEqual(result, sizeResult.Value);
            Assert.AreEqual(result.GetHashCode(), testObjHash);
            Assert.AreEqual(result.Country.GetHashCode(), countryHash);
            Assert.AreEqual(result.Sizes.GetHashCode(), sizesHash);

            for (int i = 0; i < result.Sizes.Count; i++)
            {
                Assert.AreEqual(result.Sizes[i].GetHashCode(), sizeHashesList[i]);
            }
        }

        [TestMethod]
        public void ExistingDestCollEquals()
        {
            Mapper.Register<TestCollection, TestCollectionViewModel>();
            Mapper.Register<TestItem, TestItemViewModel>().Member(dest => dest.Array, src => src.Queryable)
                .Ignore(dest => dest.Queryable);

            Mapper.Compile();
            Tuple<TestItem, TestItemViewModel, TestItemViewModel> testResult = Functional.ExistingDestCollEquals();

            int testItemHash = testResult.Item2.GetHashCode();
            int arrayHash = testResult.Item2.Array.GetHashCode();
            List<int> testArr = new List<int>(testResult.Item2.Array.Length);
            testArr.AddRange(testResult.Item2.Array.Select(tc => tc.GetHashCode()));


            TestItemViewModel result = Mapper.Map(testResult.Item1, testResult.Item2);
            Assert.AreEqual(result, testResult.Item2);
            Assert.AreEqual(result.GetHashCode(), testItemHash);
            Assert.AreEqual(result.Array.GetHashCode(), arrayHash);

            for (int i = 0; i < result.Array.Length; i++)
            {
                Assert.AreEqual(result.Array[i].GetHashCode(), testArr[i]);
                Assert.AreEqual(result.Array[i], testResult.Item3.Array[i]);
            }
        }

        [TestMethod]
        public void ExistingDestCollEqualsWithNullElement()
        {
            Mapper.Register<TestCollection, TestCollectionViewModel>();
            Mapper.Register<TestItem, TestItemViewModel>().Member(dest => dest.Array, src => src.Queryable)
                .Ignore(dest => dest.Queryable);

            Mapper.Compile();
            Tuple<TestItem, TestItemViewModel, TestItemViewModel> testResult =
                Functional.ExistingDestCollEqualsWithNullElement();

            int testItemHash = testResult.Item2.GetHashCode();
            int arrayHash = testResult.Item2.Array.GetHashCode();
            List<int?> testArr = new List<int?>(testResult.Item2.Array.Length);
            testArr.AddRange(testResult.Item2.Array.Select(tc => tc == null ? (int?)null : tc.GetHashCode()));

            TestItemViewModel result = Mapper.Map(testResult.Item1, testResult.Item2);
            Assert.AreEqual(result, testResult.Item2);
            Assert.AreEqual(result.GetHashCode(), testItemHash);
            Assert.AreEqual(result.Array.GetHashCode(), arrayHash);

            for (int i = 0; i < result.Array.Length; i++)
            {
                if (i == 3)
                {
                    Assert.AreEqual(null, result.Array[i]);
                    Assert.AreEqual(null, testArr[i]);
                }
                else
                {
                    Assert.AreEqual(result.Array[i].GetHashCode(), testArr[i]);
                    Assert.AreEqual(result.Array[i], testResult.Item3.Array[i]);
                }
            }
        }

        [TestMethod]
        public void OtherCollectionTypesTest()
        {
            Mapper.Register<TestCollection, TestCollectionViewModel>();
            Mapper.Register<TestItem, TestItemViewModel>().Member(dest => dest.ObservableCollection, src => src.Array)
                .Ignore(dest => dest.Array);

            Mapper.Compile();
            KeyValuePair<TestItem, TestItemViewModel> testResult = Functional.OtherCollectionMapTest();

            TestItemViewModel result = Mapper.Map<TestItem, TestItemViewModel>(testResult.Key);
            Assert.AreEqual(result.ObservableCollection.Count, testResult.Key.Array.Length);

            for (int i = 0; i < result.ObservableCollection.Count; i++)
            {
                Assert.AreEqual(result.ObservableCollection[i], testResult.Value.ObservableCollection[i]);
            }
        }

        [TestMethod]
        public void ExistingSrcCollGreater()
        {
            Mapper.Register<TestCollection, TestCollectionViewModel>();
            Mapper.Register<TestItem, TestItemViewModel>().Member(dest => dest.Collection, src => src.Array)
                .Ignore(dest => dest.Array).Ignore(dest => dest.List).Ignore(dest => dest.Enumerable)
                .Ignore(dest => dest.ObservableCollection).Ignore(dest => dest.Queryable);

            Mapper.Compile();
            Tuple<TestItem, TestItemViewModel, TestItemViewModel> testResult = Functional.ExistingDestSrcCollGreater();

            int testItemHash = testResult.Item2.GetHashCode();
            int collectionHash = testResult.Item2.Collection.GetHashCode();
            List<int> testColl = new List<int>(testResult.Item2.Collection.Count);
            testColl.AddRange(testResult.Item2.Collection.Select(tc => tc.GetHashCode()));

            TestItemViewModel result = Mapper.Map(testResult.Item1, testResult.Item2);
            Assert.AreEqual(result, testResult.Item2);
            Assert.AreEqual(result.GetHashCode(), testItemHash);
            Assert.AreEqual(result.Collection.GetHashCode(), collectionHash);

            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(result.Collection.ElementAt(i).GetHashCode(), testColl[i]);
            }

            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(result.Collection.ElementAt(i), testResult.Item3.Collection.ElementAt(i));
            }
        }

        [TestMethod]
        public void ExistingDestDestCollGreater()
        {
            Mapper.Register<TestCollection, TestCollectionViewModel>();
            Mapper.Register<TestItem, TestItemViewModel>().Member(dest => dest.List, src => src.Collection)
                .Ignore(dest => dest.Collection);

            Mapper.Compile();
            Tuple<TestItem, TestItemViewModel, TestItemViewModel> testResult = Functional.ExistingDestCollGreater();

            int testItemHash = testResult.Item2.GetHashCode();
            int listHash = testResult.Item2.List.GetHashCode();
            List<int> testList = new List<int>(testResult.Item2.List.Count);
            testList.AddRange(testResult.Item2.List.Select(tc => tc.GetHashCode()));


            TestItemViewModel result = Mapper.Map(testResult.Item1, testResult.Item2);
            Assert.AreEqual(result, testResult.Item2);
            Assert.AreEqual(result.GetHashCode(), testItemHash);
            Assert.AreEqual(result.List.GetHashCode(), listHash);

            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(result.List.ElementAt(i).GetHashCode(), testList[i + 4]);
            }

            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(result.List[i], testResult.Item3.List[i]);
            }
        }

        [TestMethod]
        public void NewDestinationTest()
        {
            Mapper.Register<TestCollection, TestCollectionViewModel>();
            Mapper.Register<TestItem, TestItemViewModel>().Member(dest => dest.List, src => src.Collection)
                .Ignore(dest => dest.Collection);

            Mapper.Compile();
            Tuple<TestItem, TestItemViewModel, TestItemViewModel> testResult = Functional.ExistingDestCollGreater();

            List<int> testList = new List<int>(testResult.Item2.List.Count);
            testList.AddRange(testResult.Item2.List.Select(tc => tc.GetHashCode()));


            TestItemViewModel result = Mapper.Map(testResult.Item1, new TestItemViewModel());

            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(result.List[i], testResult.Item3.List[i]);
            }
        }

        [TestMethod]
        public void ExistingDestCollNotEqual()
        {
            Mapper.Register<TestCollection, TestCollectionViewModel>();
            Mapper.Register<TestItem, TestItemViewModel>().Member(dest => dest.Array, src => src.Collection)
                .Ignore(dest => dest.Collection);

            Mapper.Compile();
            Tuple<TestItem, TestItemViewModel, TestItemViewModel> testResult = Functional.ExistingDestCollNotEqual();

            int testItemHash = testResult.Item2.GetHashCode();
            int listHash = testResult.Item2.Array.GetHashCode();
            List<int> testList = new List<int>(testResult.Item2.Array.Length);
            testList.AddRange(testResult.Item2.Array.Select(tc => tc.GetHashCode()));

            TestItemViewModel result = Mapper.Map(testResult.Item1, testResult.Item2);

            Assert.AreEqual(result, testResult.Item2);
            Assert.AreEqual(result.GetHashCode(), testItemHash);
            Assert.AreNotEqual(result.Array.GetHashCode(), listHash);

            for (int i = 0; i < 6; i++)
            {
                Assert.AreEqual(result.Array[i].GetHashCode(), testList[i]);
                Assert.AreEqual(result.Array[i], testResult.Item3.Array[i]);
            }
        }

        [TestMethod]
        public void ExistingDestinationComplex()
        {
            Mapper.Register<ItemModel, ItemModelViewModel>();
            Mapper.Register<SubItem, SubItemViewModel>();
            Mapper.Register<Unit, UnitViewModel>();
            Mapper.Register<SubUnit, SubUnitViewModel>();
            Mapper.Compile();

            KeyValuePair<ItemModel, ItemModelViewModel> sizeResult = Functional.ExistingDestinationComplex();

            int itemModelHash = sizeResult.Value.GetHashCode();
            int itemModelSubItemsHash = sizeResult.Value.SubItems.GetHashCode();

            List<int> subItemsHashes = new List<int>(sizeResult.Value.SubItems.Length);
            Dictionary<int, int> subItemUnitsCollHashes = new Dictionary<int, int>();
            Dictionary<int, List<int>> subItemUnitsHashes = new Dictionary<int, List<int>>();
            Dictionary<int, List<int>> subItemUnitSubUnitCollHashes = new Dictionary<int, List<int>>();
            Dictionary<int, Dictionary<int, List<int>>> subItemUnitSubUnitsHashes =
                new Dictionary<int, Dictionary<int, List<int>>>();

            foreach (SubItemViewModel subItem in sizeResult.Value.SubItems)
            {
                int sbHash = subItem.GetHashCode();
                subItemsHashes.Add(sbHash);
                subItemUnitsCollHashes.Add(sbHash, subItem.Units.GetHashCode());
                subItemUnitsHashes.Add(sbHash, new List<int>());
                subItemUnitSubUnitCollHashes.Add(sbHash, new List<int>());
                subItemUnitSubUnitsHashes.Add(sbHash, new Dictionary<int, List<int>>());

                foreach (UnitViewModel unit in subItem.Units.Skip(1))
                {
                    subItemUnitsHashes[sbHash].Add(unit.GetHashCode());
                    subItemUnitSubUnitCollHashes[sbHash].Add(unit.SubUnits.GetHashCode());
                    subItemUnitSubUnitsHashes[sbHash][unit.GetHashCode()] = new List<int>();

                    foreach (SubUnitViewModel subUnit in unit.SubUnits)
                    {
                        subItemUnitSubUnitsHashes[sbHash][unit.GetHashCode()].Add(subUnit.GetHashCode());
                    }
                }
            }

            ItemModelViewModel result = Mapper.Map(sizeResult.Key, sizeResult.Value);

            Assert.AreEqual(result, sizeResult.Value);
            Assert.AreEqual(result.GetHashCode(), itemModelHash);
            Assert.AreEqual(result.SubItems.GetHashCode(), itemModelSubItemsHash);
            for (int i = 0; i < result.SubItems.Length; i++)
            {
                Assert.AreEqual(result.SubItems[i].GetHashCode(), subItemsHashes[i]);
                Assert.AreEqual(
                    result.SubItems[i].Units.GetHashCode(),
                    subItemUnitsCollHashes[result.SubItems[i].GetHashCode()]);

                for (int j = 0; j < 4; j++)
                {
                    Assert.AreEqual(
                        result.SubItems[i].Units[j].GetHashCode(),
                        subItemUnitsHashes[result.SubItems[i].GetHashCode()][j]);
                    Assert.AreEqual(
                        result.SubItems[i].Units[j].SubUnits.GetHashCode(),
                        subItemUnitSubUnitCollHashes[result.SubItems[i].GetHashCode()][j]);
                    for (int k = 0; k < 3; k++)
                    {
                        Assert.AreEqual(
                            result.SubItems[i].Units[j].SubUnits[k].GetHashCode(),
                            subItemUnitSubUnitsHashes[result.SubItems[i].GetHashCode()][result.SubItems[i].Units[j]
                                .GetHashCode()][k]);
                    }
                }
            }
        }

        [TestMethod]
        public void ExistingDestinationMedium()
        {
            Mapper.Register<TripType, TripTypeViewModel>();
            Mapper.Register<TripCatalog, TripCatalogViewModel>();
            Mapper.Register<CategoryTrip, CategoryTripViewModel>();
            Mapper.Register<Trip, TripViewModel>();

            Mapper.Compile();
            KeyValuePair<Trip, TripViewModel> tripResult = Functional.ExistingDestinationMediumMap();

            int tripHash = tripResult.Value.GetHashCode();
            int tripCatHash = tripResult.Value.Category.GetHashCode();
            int tripCatCtlHash = tripResult.Value.Category.Catalog.GetHashCode();
            int tripCatCtlTypeHash = tripResult.Value.Category.Catalog.TripType.GetHashCode();


            TripViewModel result = Mapper.Map<Trip, TripViewModel>(tripResult.Key, tripResult.Value);
            Assert.AreEqual(result, tripResult.Value);
            Assert.AreEqual(result.GetHashCode(), tripHash);
            Assert.AreEqual(result.Category.GetHashCode(), tripCatHash);
            Assert.AreEqual(result.Category.Catalog.GetHashCode(), tripCatCtlHash);
            Assert.AreEqual(result.Category.Catalog.TripType.GetHashCode(), tripCatCtlTypeHash);
        }

        [TestMethod]
        public void EnumMap()
        {
            Mapper.Register<TestModel, TestViewModel>().Ignore(x => x.Country).Ignore(x => x.Sizes)
                .Member(x => x.GenderIndex, x => x.NullableGender);
            Mapper.Compile();

            TestModel test =
                new TestModel() { Gender = GenderTypes.Men.ToString(), NullableGender = GenderTypes.Women };

            TestViewModel testViewModel = Mapper.Map<TestModel, TestViewModel>(test);

            Assert.AreEqual(GenderTypes.Men, testViewModel.Gender);
            Assert.AreEqual(GenderTypes.Women.ToString(), testViewModel.NullableGender);
            Assert.AreEqual((int)GenderTypes.Women, testViewModel.GenderIndex);
        }

        [TestMethod]
        public void ConvertibleMap()
        {
            Mapper.Register<TestModel, TestViewModel>().Ignore(x => x.Country).Ignore(x => x.Sizes)
                .Member(x => x.GenderIndex, x => x.BoolValue).Member(x => x.NotNullable, x => x.Height);
            Mapper.Compile();

            TestModel test = new TestModel() { BoolValue = true, Height = 123 };

            TestViewModel testViewModel = Mapper.Map<TestModel, TestViewModel>(test);

            Assert.AreEqual(1, testViewModel.GenderIndex);
            Assert.AreEqual(123, testViewModel.NotNullable);
        }

        [TestMethod]
        public void MemberCaseInSensitivityDefaultMapTest()
        {
            Mapper.Register<TypoCase, TypoCaseViewModel>();
            Mapper.Compile();

            TypoCase typoCase = new TypoCase { id = Guid.NewGuid(), NaME = "Test name!", TestId = 5 };

            TypoCaseViewModel typoCaseViewModel = Mapper.Map<TypoCase, TypoCaseViewModel>(typoCase);
            Assert.AreEqual(typoCase.id, typoCaseViewModel.Id);
            Assert.AreEqual(typoCase.NaME, typoCaseViewModel.Name);
            Assert.AreEqual(typoCase.TestId, typoCaseViewModel.TestId);
        }

        [TestMethod]
        public void MemberCaseSensitivityGlobalMapTest()
        {
            Mapper.MemberCaseSensitiveMap(true);
            Mapper.Register<TypoCase, TypoCaseViewModel>();
            Mapper.Compile();

            TypoCase typoCase = new TypoCase { id = Guid.NewGuid(), NaME = "Test name!", TestId = 5 };

            TypoCaseViewModel typoCaseViewModel = Mapper.Map<TypoCase, TypoCaseViewModel>(typoCase);

            Assert.AreEqual(typoCaseViewModel.Id, Guid.Empty);
            Assert.AreEqual(typoCaseViewModel.Name, null);
            Assert.AreEqual(typoCase.TestId, typoCaseViewModel.TestId);
        }

        [TestMethod]
        public void MemberCaseSensitivityLocalMapTest()
        {
            Mapper.Register<TypoCase, TypoCaseViewModel>().CaseSensitive(true);
            Mapper.Compile();

            TypoCase typoCase = new TypoCase { id = Guid.NewGuid(), NaME = "Test name!", TestId = 5 };

            TypoCaseViewModel typoCaseViewModel = Mapper.Map<TypoCase, TypoCaseViewModel>(typoCase);

            Assert.AreEqual(typoCaseViewModel.Id, Guid.Empty);
            Assert.AreEqual(typoCaseViewModel.Name, null);
            Assert.AreEqual(typoCase.TestId, typoCaseViewModel.TestId);
        }

        [TestMethod]
        public void MemberCaseInSensitivityGlobalOverrideMapTest()
        {
            Mapper.MemberCaseSensitiveMap(true);
            Mapper.Register<TypoCase, TypoCaseViewModel>().CaseSensitive(false);
            Mapper.Compile();

            TypoCase typoCase = new TypoCase { id = Guid.NewGuid(), NaME = "Test name!", TestId = 5 };

            TypoCaseViewModel typoCaseViewModel = Mapper.Map<TypoCase, TypoCaseViewModel>(typoCase);
            Assert.AreEqual(typoCase.id, typoCaseViewModel.Id);
            Assert.AreEqual(typoCase.NaME, typoCaseViewModel.Name);
            Assert.AreEqual(typoCase.TestId, typoCaseViewModel.TestId);
        }

        [TestMethod]
        public void InheritanceFuncMap()
        {
            Mapper.Register<Contact, ContactViewModel>();
            Mapper.Register<Mail, MailViewModel>().Member(x => x.Contact, x => ResolveContact(x.Contact))
                .Member(x => x.StandardContactVM, x => x.StandardContact);
            Mapper.Compile();

            Guid contactId = Guid.NewGuid();

            Mail test = new Mail() { From = "from", Contact = new Organization() { Id = contactId, Name = "org" } };

            MailViewModel testViewModel = Mapper.Map<Mail, MailViewModel>(test);

            Assert.AreEqual("from", testViewModel.From);
            Assert.AreEqual(contactId, testViewModel.Contact.Id);
        }

        private static ContactViewModel ResolveContact(Contact contact)
        {
            ContactViewModel destination = null;

            if (contact.IsOrganization)
            {
                destination = new OrganizationViewModel();

                Mapper.Map(contact, destination);
            }
            else if (contact.IsPerson)
            {
                destination = new PersonViewModel();

                Mapper.Map(contact, destination);
            }
            else
            {
                destination = new ContactViewModel();

                Mapper.Map(contact, destination);
            }

            return destination;
        }

        [TestMethod]
        public void ClearErrorDuringDynamicMappingTest()
        {
            Ticket ticket = new Ticket
                                {
                                    Id = Guid.NewGuid(),
                                    Name = "Ticket",
                                    Venue = new Venue { Id = Guid.NewGuid(), Name = "Venue" }
                                };


            Assert.ThrowsException<ExpressMapperException>(() => { Mapper.Map<Ticket, TicketViewModel>(ticket); });
        }

        [TestMethod]
        public void ClearErrorDuringCompileTimeTest()
        {
            Mapper.Register<Ticket, TicketViewModel>();

            Assert.ThrowsException<ExpressMapperException>(() => Mapper.Compile());
        }

        [TestMethod]
        public void MapExistsTest()
        {
            Mapper.Register<Father, FlattenFatherSonGrandsonDto>();

            Assert.IsTrue(Mapper.MapExists(typeof(Father), typeof(FlattenFatherSonGrandsonDto)));
            Assert.IsFalse(Mapper.MapExists(typeof(FlattenFatherSonGrandsonDto), typeof(Father)));
        }

        [TestMethod]
        public void DoNotUpdateUnchangedPropertyValuesTest()
        {
            Brand srcBrand = new Brand { Id = Guid.NewGuid(), Name = "brand" };

            var existingBrandMock = new Mock<Brand>().SetupAllProperties();
            existingBrandMock.Object.Name = "brand";

            var destBrand = Mapper.Map(srcBrand, existingBrandMock.Object);
            existingBrandMock.VerifySet(x => x.Name = It.IsAny<string>(), Times.Once());

            Assert.AreEqual(destBrand.Id, srcBrand.Id);
            Assert.AreEqual(destBrand.Name, srcBrand.Name);
        }


        [TestMethod]
        public void InheritanceIncludeTest()
        {
            Mapper.Register<BaseControl, BaseControlViewModel>().Member(dst => dst.id_ctrl, src => src.Id)
                .Member(dst => dst.name_ctrl, src => src.Name).Include<TextBox, TextBoxViewModel>();
            Mapper.Compile();

            TextBox textBox = new TextBox
                                  {
                                      Id = Guid.NewGuid(),
                                      Name = "Just a text box",
                                      Description = "Just a text box - very simple description",
                                      Text = "Hello World!"
                                  };

            BaseControlViewModel baseControlViewModel = Mapper.Map<BaseControl, BaseControlViewModel>(textBox);
            Assert.AreEqual(baseControlViewModel.id_ctrl, textBox.Id);
            Assert.AreEqual(baseControlViewModel.name_ctrl, textBox.Name);
            Assert.IsTrue(baseControlViewModel is TextBoxViewModel);
        }

        [TestMethod]
        public void NestedInheritanceIncludeTest()
        {
            Mapper.Register<BaseControl, BaseControlViewModel>().Member(dst => dst.id_ctrl, src => src.Id)
                .Member(dst => dst.name_ctrl, src => src.Name).Include<TextBox, TextBoxViewModel>();
            Mapper.Register<UserInterface, UserInterfaceViewModel>().Member(
                dest => dest.ControlViewModel,
                src => src.Control);
            Mapper.Compile();

            TextBox textBox = new TextBox
                                  {
                                      Id = Guid.NewGuid(),
                                      Name = "Just a text box",
                                      Description = "Just a text box - very simple description",
                                      Text = "Hello World!"
                                  };

            UserInterface userInterface = new UserInterface { Control = textBox };

            UserInterfaceViewModel uiViewModel = Mapper.Map<UserInterface, UserInterfaceViewModel>(userInterface);
            Assert.IsNotNull(uiViewModel.ControlViewModel);
            Assert.IsTrue(uiViewModel.ControlViewModel is TextBoxViewModel);
            Assert.AreEqual(uiViewModel.ControlViewModel.Description, textBox.Description);
            Assert.AreEqual(uiViewModel.ControlViewModel.id_ctrl, textBox.Id);
            Assert.AreEqual(uiViewModel.ControlViewModel.name_ctrl, textBox.Name);
            Assert.AreEqual(((TextBoxViewModel)uiViewModel.ControlViewModel).Text, textBox.Text);
        }

        [TestMethod]
        public void MapNullSourceReturnNullDest()
        {
            Mapper.Register<object, object>();
            Mapper.Compile();

            Assert.IsNull(Mapper.Map<object, object>(null));
            Assert.IsNull(Mapper.Map<object, object>(null, (object)null));
        }

        #region Duplicate property names in the class hierarchy

        [TestMethod]
        public void MapDuplicatePropertyNamesInHierarchyTest()
        {
            Mapper.Register<A, AN>();
            Mapper.Register<AN, A>();

            Mapper.Register<T, TNT>();
            Mapper.Register<TNT, T>();
            Mapper.Compile();


            TNT tnt = new TNT { Foo = new AN { Id = 4 } };

            T t = new T { Foo = new A { Id = 5 } };

            var tntResult = Mapper.Map<T, TNT>(t);
            var tResult = Mapper.Map<TNT, T>(tnt);

            Assert.AreEqual(5, tntResult.Foo.Id);
            Assert.AreEqual(4, tResult.Foo.Id);
        }

        class A
        {
            public int Id { get; set; }
        }

        class AN : A
        {
            public new int Id { get; set; }
        }

        class T
        {
            public A Foo { get; set; }
        }

        class TN : T
        {
            public new AN Foo { get; set; }
        }

        class TNT : TN
        {
            public new AN Foo { get; set; }
        }

        #endregion
    }
}