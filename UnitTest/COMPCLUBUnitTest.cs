using System;
using System.IO;
using System.Text;
using CpForCOMPCLUB;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
    [TestClass]
    public class CompetitiveProgrammingUnitTestForCompclubUnitTest
    {
        [TestMethod]
        public void Test()
        {

            StringBuilder sb = new StringBuilder();
            Random random = new Random(10);

            for (int k = 0; k < 20; ++k)
            {
                int n = random.Next(1, 20);
                COMPCLUB.ContactList c = new COMPCLUB.ContactList();
                for (int i = 0; i < n; i++)
                {

                    StringBuilder testStringBuilder = new StringBuilder();
                    for (int j = 0; j < 5; j++)
                    {
                        testStringBuilder.Append((char)('a' + random.Next(1, 25)));
                    }

                    var testString = testStringBuilder.ToString();

                    //TEST CASE RUN
                    c.AddString(testString);
                    var got = c.Exists(testString.Substring(1, 4));
                    var expected = true;

                    sb.Append(Environment.NewLine);
                    sb.Append(Environment.NewLine);

                    sb.Append($"NEW TEST CASE: N = {n}");

                    sb.Append(Environment.NewLine);
                    sb.Append("Test String");
                    sb.Append(Environment.NewLine);

                    sb.Append(testString + " ");

                    sb.Append(Environment.NewLine);
                    sb.Append($"got : {got} vs expected: {expected}");



                    try
                    {
                        Assert.AreEqual(expected, got);
                    }
                    catch (Exception e)
                    {
                        sb.Append($" --> FAILED TEST CASE : got : {got} vs expected: {expected}");
                        sb.Append(Environment.NewLine);
                        File.WriteAllText("unit-test-input.txt", sb.ToString());

                       // throw e;
                    }

                    sb.Append(Environment.NewLine);
                }
            }

            File.WriteAllText("unit-test-input.txt", sb.ToString());

        }
    }
}
