using System;
using System.IO;
using System.Text;
using CpForRIDDLE99;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CompetitiveProgrammingUnitTestForRiddle99UnitTest
    {
        [TestMethod]
        public void Test()
        {

            StringBuilder sb = new StringBuilder();
            Random random = new Random(10);

            for (int k = 0; k < 10; ++k)
            {
                int n = random.Next(1, 20);

                for (int i = 0; i < n; i++)
                {
                    long a = random.Next(1, 100);
                    long b = random.Next((int)a, 100);
                    long m = random.Next(1, 100);

                    //TEST CASE RUN
                    var got = RIDDLE99.GetCount(a, b, m);
                    var expected = RIDDLE99.GetCount2(a, b, m);

                    sb.Append(Environment.NewLine);

                    sb.Append($"NEW TEST CASE: N = {n}, a = {a}, b = {b} , m = {m}");

                    sb.Append(Environment.NewLine);
                    sb.Append($"got : {got} vs expected: {expected}");

                    try
                    {
                        Assert.AreEqual(expected, got);
                    }
                    catch (Exception e)
                    {
                        //sb.Append($" --> FAILED TEST CASE : got : {got} vs expected: {expected}");
                        sb.Append(Environment.NewLine);
                        File.WriteAllText("unit-test-input.txt", sb.ToString());
                    }

                    sb.Append(Environment.NewLine);
                }
            }

            File.WriteAllText("unit-test-input.txt", sb.ToString());

        }
    }
}
