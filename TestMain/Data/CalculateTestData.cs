using System.Collections.Generic;
using System.Net;

namespace TestMain.Data
{
    public class CalculateTestData
    {
        private static readonly List<object[]> Data = new List<object[]>
        {
            new object[]{1,2,3},
            new object[]{1,3,4},
            new object[]{2,3,5},
            new object[]{3,4,7}
        };

        public static IEnumerable<object[]> TestData => Data;

    }
    
}
