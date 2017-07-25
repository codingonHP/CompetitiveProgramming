using System;
using System.IO;
using System.Text;
using CpForCompetitiveProgrammingRMQ;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestRMQUnitTest
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
    public class UnitTestForRMQUnitTest
    {
        private readonly Settings _settings = new Settings
        {
            ShowAll = false,
            OnlyFailed = true,
            OnlyPassed = false,
            IterationCount = 1000,
            SubIterationCount = 100,
            MinRandom = -10000,
            MaxRandom = 100000
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
                int n = random.Next(1, 1000);
                var array = new long[n];

                for (int j = 0; j < n; j++)
                {
                    var next = random.Next(_settings.MinRandom, _settings.MaxRandom);
                    array[j] = next;
                }

                for (int i = 0; i < _settings.SubIterationCount; i++)
                {
                    try
                    {
                        long l = random.Next(0, n);
                        long r = random.Next(0, n);

                        var temp = r;
                        if (l > r)
                        {
                            r = l;
                            l = temp;
                        }
                            


                        ++_totalTestCase;

                        //TEST CASE RUN
                        var got = RMQ.Solve(array, l, r);
                        var expected = RMQ.SolveBf(array, l, r);

                        try
                        {
                            Assert.AreEqual(expected, got);
                            ++_passed;

                            if (_settings.ShowAll || _settings.OnlyPassed)
                            {
                                sb.Append(Environment.NewLine);
                                sb.Append($"NEW TEST CASE: N = {n}");
                                sb.Append(Environment.NewLine);
                                sb.Append("MESSAGE");
                                sb.Append(Environment.NewLine);

                                foreach (var wval in array)
                                {
                                    sb.Append(wval + " ");
                                }

                                sb.Append(Environment.NewLine);
                                sb.Append(Environment.NewLine);

                                sb.Append($"got : {got} vs expected: {expected}");
                                sb.Append($" --> PASSED TEST CASE");
                                sb.Append(Environment.NewLine);
                            }

                        }
                        catch (Exception e)
                        {
                            ++_falied;

                            sb.Append(Environment.NewLine);
                            sb.Append($"NEW TEST CASE: N = {n}, L = {l}, R = {r}");
                            sb.Append(Environment.NewLine);
                            sb.Append("MESSAGE");
                            sb.Append(Environment.NewLine);

                            foreach (var wval in array)
                            {
                                sb.Append(wval + " ");
                            }

                            sb.Append(Environment.NewLine);
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
                        sb.Append($"NEW TEST CASE: N = {n}");
                        sb.Append(Environment.NewLine);
                        sb.Append("MESSAGE");
                        sb.Append(Environment.NewLine);

                        foreach (var wval in array)
                        {
                            sb.Append(wval + " ");
                        }

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
