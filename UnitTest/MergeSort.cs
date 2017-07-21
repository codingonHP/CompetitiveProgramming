using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CompetitiveProgramming;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CompetitiveProgrammingUnitTestForMergeSort
    {
        [TestMethod]
        public void Test()
        {

            StringBuilder sb = new StringBuilder();
            Random random = new Random(10);
            Dictionary<int,int> mpa = new Dictionary<int, int>();

            for (int k = 0; k < 1000; ++k)
            {
                int n = random.Next(1, 100);

                int[] arry = new int[n];
                int[] sorted = new int[n];
                for (int i = 0; i < n; i++)
                {
                    for (int j = 0; j < n; j++)
                    {
                        arry[j] = random.Next(0, 10000);
                    }

                    for (int j = 0; j < n; j++)
                    {
                        sorted[j] = arry[j];
                    }

                    //TEST CASE RUN
                    Sort.MergeSort(arry, true);
                    sorted = sorted.OrderBy(c => c).ToArray();

                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);

                    sb.Append($"NEW TEST CASE: N = {n}");

                    sb.Append(Environment.NewLine);
                    sb.Append("Array:");
                    sb.Append(Environment.NewLine);

                    foreach (var wval in sorted)
                    {
                        sb.Append(wval + " ");
                    }

                    sb.Append(Environment.NewLine);
                    sb.Append("Sorting:");
                    sb.Append(Environment.NewLine);

                    foreach (var wval in arry)
                    {
                        sb.Append(wval + " ");
                    }

                    sb.Append(Environment.NewLine);

                    int jLoop = 0;
                    try
                    {

                        for (jLoop = 0; jLoop < arry.Length; jLoop++)
                        {
                            Assert.AreEqual(sorted[jLoop], arry[jLoop]);
                        }
                    }
                    catch (Exception e)
                    {
                        sb.Append($" --> FAILED TEST CASE : got : {arry[jLoop]} vs expected: {sorted[jLoop]}");
                        sb.Append(Environment.NewLine);
                        File.WriteAllText("unit-test-input.txt", sb.ToString());

                        throw e;
                    }

                    sb.Append(Environment.NewLine);
                }
            }

            File.WriteAllText("unit-test-input.txt", sb.ToString());

        }
    }
}
