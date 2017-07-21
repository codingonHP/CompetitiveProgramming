using System;
using System.IO;
using System.Text;
using CpForKCHAR;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CompetitiveProgrammingUnitTestForKchartest
    {
        [TestMethod]
        public void Test()
        {

            StringBuilder sb = new StringBuilder();
            Random random = new Random(10);
            string sref = string.Empty;

            for (int k = 0; k < 10; ++k)
            {
                int n = random.Next(1, 20);
               
                for (int i = 0; i < n; i++)
                {
                    var n1 = random.Next(1000, 100000);

                    //TEST CASE RUN
                    sref = string.Empty;
                    var got = KCHAR.GetChar(n1);
                    var expected = KCHAR.GetChar2(n1, ref sref);

                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);

                    sb.Append($"NEW TEST CASE: N = {n}");

                    sb.Append(Environment.NewLine);
                    sb.Append("MESSAGE");
                    sb.Append(Environment.NewLine);

                    sb.Append($"got : {got} vs expected: {expected}");

                    try
                    {
                        Assert.AreEqual(expected, got);
                        sb.Append($" --> PASSED");
                    }
                    catch (Exception)
                    {
                        sb.Append($" --> FAILED TEST CASE : got : {got} vs expected: {expected} for k = {n1} and s ={sref}");
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
