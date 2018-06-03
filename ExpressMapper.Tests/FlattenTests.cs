namespace ExpressMapper.Tests
{
    using ExpressMapper.Tests.Common;
    using ExpressMapper.Tests.Models.Classes;
    using ExpressMapper.Tests.Models.ViewModels;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class FlattenTests : BaseTestClass
    {
        [TestMethod]
        public void FlattenFatherSonGrandsonDtoOk()
        {
            //SETUP
            Mapper.Register<Father, FlattenFatherSonGrandsonDto>().Flatten();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var father = Father.CreateOne();
            var dto = Mapper.Map<Father, FlattenFatherSonGrandsonDto>(father);

            //VERIFY   
            Assert.AreEqual("Father", dto.MyString); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual(1, dto.MyInt); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual("Son", dto.SonMyString);
            Assert.AreEqual(2, dto.SonMyInt);
            Assert.AreEqual("Grandson", dto.SonGrandsonMyString);
            Assert.AreEqual(3, dto.SonGrandsonMyInt);
        }

        [TestMethod]
        public void FlattenFatherSonGrandsonDtoWithDestinationOk()
        {
            //SETUP
            Mapper.Register<Father, FlattenFatherSonGrandsonDto>().Flatten();
            Mapper.Compile(CompilationTypes.Destination);

            //ATTEMPT
            var dto = new FlattenFatherSonGrandsonDto { MyInt = 29387465, MyString = "whjqegfqwjehfg" };
            var father = Father.CreateOne();
            Mapper.Map(father, dto);

            //VERIFY   
            Assert.AreEqual("Father", dto.MyString); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual(1, dto.MyInt); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual("Son", dto.SonMyString);
            Assert.AreEqual(2, dto.SonMyInt);
            Assert.AreEqual("Grandson", dto.SonGrandsonMyString);
            Assert.AreEqual(3, dto.SonGrandsonMyInt);
        }

        [TestMethod]
        public void FatherLowerCaseFlattenFatherSonGrandsonDtoOk()
        {
            //SETUP
            Mapper.Register<FatherLowerCase, FlattenFatherSonGrandsonDto>().Flatten();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var dto = new FlattenFatherSonGrandsonDto();
            Mapper.Map(FatherLowerCase.CreateOne(), dto);

            //VERIFY   
            Assert.AreEqual("Father", dto.MyString); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual(1, dto.MyInt); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual("Son", dto.SonMyString);
            Assert.AreEqual(2, dto.SonMyInt);
            Assert.AreEqual("Grandson", dto.SonGrandsonMyString);
            Assert.AreEqual(3, dto.SonGrandsonMyInt);
        }

        [TestMethod]
        public void FlattenFatherSonDtoForGrandsonDtoOk()
        {
            //SETUP
            Mapper.Register<Father, FlattenFatherSonDtoForGrandsonDto>().Flatten();
            Mapper.Register<Grandson, FlattenSimpleClass>();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var father = Father.CreateOne();
            var dto = Mapper.Map<Father, FlattenFatherSonDtoForGrandsonDto>(father);

            //VERIFY   
            Assert.AreEqual("Father", dto.MyString); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual(1, dto.MyInt); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual("Son", dto.SonMyString);
            Assert.AreEqual(2, dto.SonMyInt);
            Assert.AreEqual("Grandson", dto.SonGrandson.MyString);
            Assert.AreEqual(3, dto.SonGrandson.MyInt);
        }

        [TestMethod]
        public void FlattenFatherSonDtoForGrandsonDtoForgetFlattenSimpleClassOk()
        {
            //SETUP
            //Mapper.Register<Grandson, FlattenSimpleClass>();      //If you don't supply the mapping it does not happen
            Mapper.Register<Father, FlattenFatherSonDtoForGrandsonDto>().Flatten();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var father = Father.CreateOne();
            var dto = Mapper.Map<Father, FlattenFatherSonDtoForGrandsonDto>(father);

            //VERIFY   
            Assert.AreEqual("Father", dto.MyString); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual(1, dto.MyInt); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual("Son", dto.SonMyString);
            Assert.AreEqual(2, dto.SonMyInt);
            Assert.AreEqual(null, dto.SonGrandson);
        }

        [TestMethod]
        public void FlattenFatherSonGrandsonLowerCaseDtoOk()
        {
            //SETUP
            Mapper.Register<Father, FlattenFatherSonGrandsonLowerCaseDto>().Flatten();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var father = Father.CreateOne();
            var dto = Mapper.Map<Father, FlattenFatherSonGrandsonLowerCaseDto>(father);

            //VERIFY   
            Assert.AreEqual("Father", dto.mystring); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual(1, dto.myInt); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual("Son", dto.sonMyString);
            Assert.AreEqual(2, dto.Sonmyint);
            Assert.AreEqual("Grandson", dto.SongrandsonmYstring);
            Assert.AreEqual(3, dto.sonGrandsonMyInt);
        }

        [TestMethod]
        public void FlattenFatherSonGrandsonDtoCaseSensativeOk()
        {
            //SETUP
            Mapper.Register<Father, FlattenFatherSonGrandsonDto>().CaseSensitive(true).Flatten();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var father = Father.CreateOne();
            var dto = Mapper.Map<Father, FlattenFatherSonGrandsonDto>(father);

            //VERIFY   
            Assert.AreEqual("Father", dto.MyString); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual(1, dto.MyInt); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual("Son", dto.SonMyString);
            Assert.AreEqual(2, dto.SonMyInt);
            Assert.AreEqual("Grandson", dto.SonGrandsonMyString);
            Assert.AreEqual(3, dto.SonGrandsonMyInt);
        }

        [TestMethod]
        public void FlattenFatherSonGrandsonLowerCaseDtoCaseSensativeOk()
        {
            //SETUP
            Mapper.Register<Father, FlattenFatherSonGrandsonLowerCaseDto>().CaseSensitive(true).Flatten();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var dto = Mapper.Map<Father, FlattenFatherSonGrandsonLowerCaseDto>(Father.CreateOne());

            //VERIFY   
            Assert.AreEqual(null, dto.mystring); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual(0, dto.myInt); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual(null, dto.sonMyString);
            Assert.AreEqual(0, dto.Sonmyint);
            Assert.AreEqual(null, dto.SongrandsonmYstring);
            Assert.AreEqual(null, dto.sonGrandsonMyInt);
        }

        [TestMethod]
        public void FlattenFatherSonGrandsonDtoNoSon()
        {
            //SETUP
            Mapper.Register<Father, FlattenFatherSonGrandsonDto>().Flatten();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var src = new Father { MyString = "Father", MyInt = 1 };

            var dto = Mapper.Map<Father, FlattenFatherSonGrandsonDto>(src);

            //VERIFY   
            Assert.AreEqual("Father", dto.MyString); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual(1, dto.MyInt); //This is mapped by the normal ExpressMapper 
            Assert.AreEqual(null, dto.SonMyString);
            Assert.AreEqual(0, dto.SonMyInt);
            Assert.AreEqual(null, dto.SonGrandsonMyString);
            Assert.AreEqual(null, dto.SonGrandsonMyInt);
        }

        [TestMethod]
        public void FlattenFatherSonGrandsonDtoOverrideSonGrandsonMyStringOk()
        {
            //SETUP
            Mapper.Register<Father, FlattenFatherSonGrandsonDto>()
                .Member(dest => dest.SonGrandsonMyString, src => src.MyString).Flatten();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var father = Father.CreateOne();
            var dto = Mapper.Map<Father, FlattenFatherSonGrandsonDto>(father);

            //VERIFY   
            Assert.AreEqual("Father", dto.MyString);
            Assert.AreEqual("Son", dto.SonMyString);
            Assert.AreEqual("Father", dto.SonGrandsonMyString);
        }

        [TestMethod]
        public void FlattenFatherSonGrandsonDtoIgnoreSonMyStringOk()
        {
            //SETUP
            Mapper.Register<Father, FlattenFatherSonGrandsonDto>().Ignore(dest => dest.SonMyString).Flatten();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var dto = Mapper.Map<Father, FlattenFatherSonGrandsonDto>(Father.CreateOne());

            //VERIFY   
            Assert.AreEqual("Father", dto.MyString);
            Assert.AreEqual(null, dto.SonMyString);
            Assert.AreEqual("Grandson", dto.SonGrandsonMyString);
        }

        [TestMethod]
        public void FlattenFatherSonsCountDtoOk()
        {
            //SETUP
            Mapper.Register<FatherSons, FlattenFatherSonsCountDto>().Flatten();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var fatherSons = FatherSons.CreateOne();
            var dto = Mapper.Map<FatherSons, FlattenFatherSonsCountDto>(fatherSons);

            //VERIFY  
            Assert.AreEqual("Father", dto.MyString);
            Assert.AreEqual(5, dto.SonsCount);
        }

        [TestMethod]
        public void FlattenLinqCollectionMethodsDtoOk()
        {
            //SETUP
            Mapper.Register<FatherSons, FlattenLinqCollectionMethodsDto>().Flatten();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var dto = Mapper.Map<FatherSons, FlattenLinqCollectionMethodsDto>(FatherSons.CreateOne());

            //VERIFY  
            Assert.AreEqual(true, dto.SonsAny);
            Assert.AreEqual(5, dto.SonsCount);
            Assert.AreEqual(5, dto.SonsLongCount);
            Assert.AreEqual("Son", dto.SonsFirstOrDefault.MyString);
        }

        [TestMethod]
        public void FlattenCircularReferenceDtoOk()
        {
            //SETUP
            Mapper.Register<FlattenCircularReference, FlattenCircularReferenceDto>().Flatten();
            Mapper.Compile(CompilationTypes.Source);

            //ATTEMPT
            var dto = Mapper.Map<FlattenCircularReference, FlattenCircularReferenceDto>(
                FlattenCircularReference.CreateOne());

            //VERIFY  
            Assert.AreEqual("Outer", dto.MyString);
            Assert.AreEqual("Son", dto.SonMyString);
            Assert.AreEqual("Inner", dto.CircularRefMyString);
        }

        //----------------------------------
        //Failure tests

        [TestMethod]
        public void FlattenFatherSonsCountBadDtoOk()
        {
            //SETUP
            Mapper.Reset();
            Mapper.Register<FatherSons, FlattenFatherSonsCountBadDto>().Flatten();

            //ATTEMPT
            var ex = Assert.ThrowsException<ExpressMapperException>(() => Mapper.Compile(CompilationTypes.Source));

            //VERIFY  
            Assert.AreEqual(
                $"Error error occured trying to compile mapping for: source {typeof(FatherSons).FullName}, destination {typeof(FlattenFatherSonsCountBadDto).FullName}. See the inner exception for details.",
                ex.Message);
            Assert.AreEqual(
                "We could not find the Method Count() which matched the property SonsCount of type System.String.",
                ex.InnerException.Message);
        }
    }
}