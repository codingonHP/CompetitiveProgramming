using System;
using System.IO;
using System.Text;
using CpForCompetitiveProgramming;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestCALCTest
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
    public class UnitTestForCALCTest
    {
        private readonly Settings _settings = new Settings
        {
            ShowAll = false,
            OnlyFailed = true,
            OnlyPassed = false,
            IterationCount = 1000000,
            SubIterationCount = 1,
            MinRandom = 1,
            MaxRandom = 1000000000
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
                        int n = random.Next(_settings.MinRandom, _settings.MaxRandom);
                        var b = random.Next(_settings.MinRandom, _settings.MaxRandom);

                        ++_totalTestCase;

                        //TEST CASE RUN
                        var got = CALC.Solve(n, b);
                        var expected = CALC.SolveBf(n, b);

                        try
                        {
                            Assert.AreEqual(expected, got);
                            ++_passed;

                            if (_settings.ShowAll || _settings.OnlyPassed)
                            {
                                if (expected > 0)
                                {
                                    sb.Append(Environment.NewLine);
                                    sb.Append($"NEW TEST CASE: N = {n}, B = {b}");

                                    sb.Append(Environment.NewLine);
                                    sb.Append("OUTPUT");
                                    sb.Append(Environment.NewLine);


                                    sb.Append($"got : {expected} vs expected: {got}");
                                    sb.Append($" --> PASSED TEST CASE");
                                    sb.Append(Environment.NewLine);
                                }

                            }

                        }
                        catch (Exception e)
                        {
                            ++_falied;

                            sb.Append(Environment.NewLine);
                            sb.Append($"NEW TEST CASE: N = {n}, B = {b}");
                            sb.Append(Environment.NewLine);
                            sb.Append($"got : {expected} vs expected: {got}");
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
