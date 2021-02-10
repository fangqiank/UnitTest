using TestMain.Data;
using UnitTest;
using Xunit;

namespace TestMain
{
    [Trait("Calculator", "calc")]
    [Collection("Long Time Task Collection")]
    public class CalculatorTests
    {
        [Theory]
        //[InlineData(1,2,3)]
        //[InlineData(2, 3, 5)]
        //[InlineData(3, 4, 7)]
        //[InlineData(4, 2, 6)]
        // [MemberData(nameof(CalculateTestData.TestData),MemberType = typeof(CalculateTestData))]
       // [MemberData(nameof(CalculateCsvData.TestData), MemberType = typeof(CalculateCsvData))]
        [CalculatorData]
        public void ShouldAdd(int x,int y,int expected)
        {
            //Arrange
            var sut = new Calculator();

            //Act
            var result = sut.Add(x, y);

            //Assert
            Assert.Equal(expected, result);

        }

      
    }
}
