using System;
using System.IO;
using System.Text;
using CpForPCAKE;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CompetitiveProgrammingUnitTestForPcakeTest
    {


        [TestMethod]
        public void Test()
        {
            bool ShowPassed = false;
            StringBuilder sb = new StringBuilder();
            Random random = new Random(10);

            for (int k = 0; k < 10; ++k)
            {
                int n = random.Next(1, 10000);

                for (int i = 0; i < 20; i++)
                {
                    try
                    {
                        long[] array = new long[n];
                        for (int j = 0; j < n; j++)
                        {
                            array[j] = random.Next(1, 1);
                        }

                        if (ShowPassed)
                        {
                            sb.Append(Environment.NewLine);
                            sb.Append(Environment.NewLine);
                            sb.Append($"NEW TEST CASE: N = {n}");

                            sb.Append(Environment.NewLine);
                            sb.Append("Array:");
                            sb.Append(Environment.NewLine);


                            foreach (var wval in array)
                            {
                                sb.Append(wval + " ");
                            }
                        }

                        //TEST CASE RUN
                        var got = PCAKE.GetNumberOfWays(array);
                        var expected = PCAKE.GetNumberOfWaysB(array);

                        try
                        {
                            Assert.IsTrue(expected == got);

                            if (ShowPassed)
                            {
                                sb.Append(Environment.NewLine);
                                sb.Append($"got : {got} vs expected: {expected}");
                                sb.Append($" --> PASSED");
                                sb.Append(Environment.NewLine);
                            }
                        }
                        catch (Exception e)
                        {
                            sb.Append($"NEW TEST CASE: N = {n}");
                            foreach (var wval in array)
                            {
                                sb.Append(wval + " ");
                            }

                            sb.Append(Environment.NewLine);
                            sb.Append($"got : {got} vs expected: {expected}  --> FAILED");
                            sb.Append(Environment.NewLine);
                        }

                        sb.Append(Environment.NewLine);
                    }
                    catch (Exception e)
                    {
                        sb.Append($" --> Exception");
                        sb.Append(e.StackTrace);
                        sb.Append(Environment.NewLine);
                    }
                }
            }

            File.WriteAllText("unit-test-input.txt", sb.ToString());

        }

        [TestMethod]
        public void MultiplyTest()
        {
            var m = PCAKE.Multiply("32432", "23243");
            long t = 32432 * 23243;

            Assert.AreSame(m, t);
        }

        [TestMethod]
        public void DecTest()
        {
            string number = "100";
            var result = PCAKE.Dec(number);
            Assert.IsTrue(result == "99");

            number = "1000";
            result = PCAKE.Dec(number);
            Assert.IsTrue(result == "999");

            number = "10000";
            result = PCAKE.Dec(number);
            Assert.IsTrue(result == "9999");

            number = "3324";
            result = PCAKE.Dec(number);
            Assert.IsTrue(result == "3323");

            number = "1111";
            result = PCAKE.Dec(number);
            Assert.IsTrue(result == "1110");

            number = "0";
            result = PCAKE.Dec(number);
            Assert.IsTrue(result == "-1");
        }
    }
}
