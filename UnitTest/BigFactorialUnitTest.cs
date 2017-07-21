using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CpForBigFactorial;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestBigFactorialUnitTest
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
    public class UnitTestForBigFactorialUnitTest
    {
        private readonly Settings _settings = new Settings
        {
            ShowAll = true,
            OnlyFailed = false,
            OnlyPassed = false,
            IterationCount = 2,
            SubIterationCount = 1,
            MinRandom = 0,
            MaxRandom = 10000
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
            Dictionary<string, string> map = new Dictionary<string, string>();

            for (int k = 0; k < _settings.IterationCount; ++k)
            {

                for (int i = 0; i < _settings.SubIterationCount; i++)
                {

                    try
                    {

                        var next = random.Next(_settings.MinRandom, _settings.MaxRandom);
                        ++_totalTestCase;

                        try
                        {
                            //TEST CASE RUN
                            var got = BigFactorial.Fact(next + "", map);

                            ++_passed;

                            if (_settings.ShowAll || _settings.OnlyPassed)
                            {
                                sb.Append(Environment.NewLine);
                                sb.Append($"NEW TEST CASE: N = {next}");
                                sb.Append(Environment.NewLine);
                                sb.Append("MESSAGE");
                                sb.Append(Environment.NewLine);

                                sb.Append($"got : {got}");
                                sb.Append($" --> PASSED TEST CASE");
                                sb.Append(Environment.NewLine);
                            }

                        }
                        catch (Exception exception)
                        {
                            ++_falied;

                            sb.Append(Environment.NewLine);
                            sb.Append($"NEW TEST CASE: N = {next}");
                            sb.Append(Environment.NewLine);
                            sb.Append("MESSAGE");
                            sb.Append(Environment.NewLine);
                            sb.Append($" --> Exception in TEST CASE : Exception : {exception.StackTrace}");
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
