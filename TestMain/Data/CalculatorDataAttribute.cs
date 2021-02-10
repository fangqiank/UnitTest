using System.Collections.Generic;
using System.Reflection;
using Xunit.Sdk;

namespace TestMain.Data
{
    public class CalculatorDataAttribute:DataAttribute
    {
        public override IEnumerable<object[]> GetData(MethodInfo testMethod)
        {
            yield return  new object[] {0,100,100};
            yield return new object[] { 1, 99, 100 };
            yield return new object[] { 50, 50, 100 };
            yield return new object[] { 10, 100, 110 };
        }
    }
}
