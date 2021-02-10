using System;
using System.Collections.Generic;
using UnitTest;
using Xunit;
using Xunit.Abstractions;

namespace TestMain
{
    [Collection("Long Time Task Collection")]
    public class PatientShould:IClassFixture<LongTimeTaskFixture>,IDisposable
    {
        private readonly ITestOutputHelper _output;
        private readonly Patient _patient;
        private readonly LongTermTask _task;

        public PatientShould(ITestOutputHelper output,LongTimeTaskFixture fixture)
        {
            _output = output;
            _patient=new Patient();
            _task=fixture.Task;
        }

        [Fact]
        [Trait("Category","New")]
        public void BeNewWhenCreated()
        {
            _output.WriteLine("first test");
            
            //arrange
            //var patient = new Patient();

            //act
            var result = _patient.IsNew;

            //assert
            Assert.True(result);
        }

        [Fact]
        [Trait("Category", "Name")]
        public void HaveCorrectFullName()
        {
            //var patient = new Patient
            //{
            //    FirstName = "Nick",
            //    LastName = "Carter"
            //};

            _patient.FirstName = "Nick";
            _patient.LastName = "Carter";
            
            var fullName = _patient.FullName;

            Assert.Equal("Nick Carter", fullName); 
            Assert.StartsWith("Ni",fullName);
            Assert.EndsWith("er",fullName);
            Assert.Contains("Nick Cart", fullName);
            Assert.NotEqual("NICK CARTER",fullName);
            Assert.Matches("[A-Z]{1}[a-z]+ [A-Z]{1}[a-z]+",fullName);
        }

        [Fact]
        [Trait("Category", "New")]
        public void HaveDefaultBloodSugarWhenCreated()
        {
            //var  p= new Patient();
            var bloodSugar = _patient.BloodSugar;
            Assert.Equal(4.9f,bloodSugar,precision:5);
            Assert.InRange(bloodSugar,3.9f,6.1f);
        }

        [Fact]
        [Trait("Category", "New")]
        [Trait("Category", "Name")]
        public void HaveNoNameWhenCreated()
        {
            //var p =new Patient();
            Assert.Null(_patient.FirstName);
        }

        [Fact(Skip = "no need")]
        public void HaveHadAColdBefore()
        {
            //var p = new Patient();
            var diseases = new List<string>
            {
                "flu",
                "AIDS",
                "HB",
                "HA"
            };
            _patient.History.Add("flu");
            _patient.History.Add("AIDS");
            _patient.History.Add("HB");
            _patient.History.Add("HA");

            Assert.Contains("flu", _patient.History);
            Assert.DoesNotContain("Heart Disease", _patient.History);

            //Predicate
            Assert.Contains(_patient.History, x => x.StartsWith("A"));
            Assert.All(_patient.History,x=>Assert.True(x.Length>=2));

            Assert.Equal(diseases, _patient.History);
        }

        [Fact]
        public void BeAPerson()
        {
            //var p  = new Patient();
            var p = new Patient();

            Assert.IsType<Patient>(_patient);
            Assert.IsNotType<Person>(_patient);

            Assert.IsAssignableFrom<Person>(_patient);

            Assert.NotSame(_patient, p);
        }

        [Fact]
        public void ThrowExceptionsWhenErrorOccurred()
        {
            //var p = new Patient();

            var ex = Assert.Throws<InvalidOperationException>(() => _patient.NotAllowed());

            Assert.Equal("Not able to create",ex.Message);
        }

        [Fact]
        public void RaiseSleptEvent()
        {
            //var p = new Patient();

            Assert.Raises<EventArgs>(
                handler => _patient.PatientSleep += handler,
                handler => _patient.PatientSleep -= handler,
                ()=> _patient.Sleep()
            );
        }

        [Fact]
        public void RaisePropertyChangedEvent()
        {
            //var p = new Patient();

            Assert.PropertyChanged(_patient, nameof(_patient.HeartBeatRate),()=> _patient.IncreaseHeartBearRate());
        }

        public void Dispose()
        {
            _output.WriteLine("dispose resource");
        }
    }
}
