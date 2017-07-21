using System;
using System.IO;
using System.Linq;
using System.Text;
using Heap;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CompetitiveProgrammingUnitTestForHeapTest
    {
        [TestMethod]
        public void Test()
        {
            StringBuilder sb = new StringBuilder();

            Random random = new Random(10);

            for (int k = 0; k < 10; ++k)
            {
                int n = random.Next(1, 10);

                for (int i = 0; i < n; i++)
                {
                    try
                    {
                        int[] array = new int[n];
                        for (int j = 0; j < n; j++)
                        {
                            array[j] = random.Next(0, 10);
                        }

                        int k1 = random.Next(0, 10);

                        //TEST CASE RUN
                        var got = Solution.GetCount1(array, k1);
                        var expected = Solution.GetCount2(array, k1);

                        sb.Append(Environment.NewLine);
                        sb.Append(Environment.NewLine);

                        sb.Append($"NEW TEST CASE: N = {n}, K = {k1}");

                        sb.Append(Environment.NewLine);
                        sb.Append("Array");
                        sb.Append(Environment.NewLine);

                        array = array.OrderBy(c => c).ToArray();
                        foreach (var wval in array)
                        {
                            sb.Append(wval + " ");
                        }

                        sb.Append(Environment.NewLine);
                        sb.Append(Environment.NewLine);
                        sb.Append($"got : {got} vs expected: {expected} ");

                        Assert.AreEqual(expected, got);
                        sb.Append($" PASSED");
                    }
                    catch (Exception)
                    {
                        sb.Append($" FAILED");
                        sb.Append(Environment.NewLine);
                    }
                }
            }


            sb.Append(Environment.NewLine);
            File.WriteAllText("unit-test-input.txt", sb.ToString());
        }
    }
}