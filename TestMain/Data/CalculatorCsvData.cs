using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace TestMain.Data
{
    public class CalculateCsvData
    {
        public static IEnumerable<object[]> TestData
        {
            get
            {
                string[] csvLines = File.ReadAllLines("Data\\TestData.csv");
                var testCases = new List<object[]>();
                foreach (var line in csvLines)
                {
                    IEnumerable<int> values = line.Split(',').Select(int.Parse);
                    object[] testCase = values.Cast<object>().ToArray();
                    testCases.Add(testCase);

                }

                return testCases;
            }
        }
    }
}
