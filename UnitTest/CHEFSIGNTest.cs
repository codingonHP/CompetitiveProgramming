using System;
using System.IO;
using System.Linq;
using System.Text;
using CpForCHEFSIGN;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestCHEFSIGNTest
{
    class Settings
    {
        public bool ShowAll { get; set; }
        public bool OnlyFailed { get; set; }
        public bool OnlyPassed { get; set; }
        public long IterationCount { get; set; }
        public long SubIterationCount { get; set; }
        public int MinRandom { get; set; }
        public int MaxRandom { get; set; }
    }

    [TestClass]
    public class UnitTestForCHEFSIGNTest
    {
        private readonly Settings _settings = new Settings
        {
            ShowAll = true,
            OnlyFailed = false,
            OnlyPassed = false,
            IterationCount = 1000,
            SubIterationCount = 1,
            MinRandom = 1,
            MaxRandom = 1000000
        };

        private int _totalTestCase;
        private int _passed;
        private int _falied;

        [TestMethod]
        public void Test()
        {

            StringBuilder sb = new StringBuilder();

            sb.Append(Environment.NewLine);
            sb.Append(Environment.NewLine);

            Random random = new Random(9997);

            for (int k = 0; k < _settings.IterationCount; ++k)
            {
                for (int i = 0; i < _settings.SubIterationCount; i++)
                {

                    try
                    {
                        int n = random.Next(1, 100000);
                        

                        StringBuilder sb1 = new StringBuilder();

                        for (int j = 0; j < n; j++)
                        {
                            var next = random.Next(_settings.MinRandom, _settings.MaxRandom);
                            if (next % 2 == 0)
                            {
                                sb1.Append('<');
                            }
                            else if(n % 3 == 0 || n % 5 == 0 || n % 7 == 0)
                            {
                                sb1.Append('=');
                            }
                            else
                            {
                                sb1.Append('>');
                            }
                        }

                        var str = sb1.ToString();
                        var sArry = str.Where(c => c != '=').ToArray();
                        var newStr = new string(sArry);

                        ++_totalTestCase;

                        //TEST CASE RUN
                        var got = CHEFSIGN.Solve(str);
                        var expected = CHEFSIGN.Solve(newStr);

                        try
                        {
                            Assert.IsTrue(expected == got);
                            ++_passed;

                            if (_settings.ShowAll || _settings.OnlyPassed)
                            {
                                sb.Append(Environment.NewLine);
                                sb.Append($"NEW TEST CASE: Str = {str}");
                                sb.Append(Environment.NewLine);
                                sb.Append($"Expected: {expected} vs got = {got}");
                                sb.Append(Environment.NewLine);
                            }
                        }
                        catch (Exception e)
                        {
                            ++_falied;

                            sb.Append(Environment.NewLine);
                            sb.Append($"NEW TEST CASE: N = {n}");
                            sb.Append(Environment.NewLine);
                            sb.Append("MESSAGE");
                            sb.Append(Environment.NewLine);

                            sb.Append($"got : {got} vs expected: {expected}");
                            sb.Append($" --> Failed TEST CASE");
                            sb.Append(Environment.NewLine);
                        }
                    }
                    catch (Exception exception)
                    {
                        ++_falied;

                        sb.Append(Environment.NewLine);
                        sb.Append("MESSAGE");
                        sb.Append(Environment.NewLine);

                        sb.Append($" --> Exception in TEST CASE : Exception : {exception.StackTrace}");
                        sb.Append(Environment.NewLine);
                    }
                }
            }

            sb.Insert(0, $"Total Test cases run = {_totalTestCase}, Passed = {_passed}, Failed = {_falied}");
            File.WriteAllText("unit-test-input.txt", sb.ToString());

        }
    }
}
