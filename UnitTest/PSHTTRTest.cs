using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using CpForCompetitiveProgrammingPSHTTR;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestPSHTTR
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
    public class UnitTestForPshttr
    {
        private readonly Settings _settings = new Settings
        {
            ShowAll = true,
            OnlyFailed = false,
            OnlyPassed = false,
            IterationCount = 10,
            SubIterationCount = 1,
            MinRandom = 1,
            MaxRandom = 10
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
                int n = random.Next(1, 10);

                do
                {
                    n = random.Next(1, 10);

                } while (n < 3);

                for (int i = 0; i < _settings.SubIterationCount; i++)
                {
                    try
                    {
                        PSHTTR.Path path = new PSHTTR.Path(n);

                        sb.Append(Environment.NewLine);
                        sb.Append($"Graph for {n} nodes.");
                        sb.Append(Environment.NewLine);

                        Dictionary<string, string> nodeDictionary = new Dictionary<string, string>();

                        for (int j = 0; j < n - 1; j++)
                        {
                            long u = 0, v = 0, c = 0;
                            string key = string.Empty;

                            int count = 0;
                            while (string.IsNullOrEmpty(key) || nodeDictionary.ContainsKey(key) || nodeDictionary.ContainsKey(new string(key.Reverse().ToArray())))
                            {
                                ++count;
                                do
                                {
                                    u = random.Next(_settings.MinRandom, n);
                                    v = random.Next(_settings.MinRandom, n);
                                } while (u == v);

                                c = random.Next(_settings.MinRandom, _settings.MaxRandom);

                                key = $"{u}{v}";
                            }

                            nodeDictionary.Add(key, string.Empty);

                            path.AddNode(u, v, c);

                            sb.Append($"{u} {v} {c}");
                            sb.Append(Environment.NewLine);
                        }

                        //Print the graph
                        sb.Append(Environment.NewLine);
                        sb.Append(Environment.NewLine);

                        long m = random.Next(1, 10);

                        sb.Append($"Query for {m} times");
                        sb.Append(Environment.NewLine);

                        for (long j = 0; j < m; j++)
                        {
                            ++_totalTestCase;

                            var u = random.Next(1, n);
                            var v = random.Next(1, n);
                            long kmin = random.Next(1, 10);
                            kmin = 1000;

                            try
                            {
                                //TEST CASE RUN
                                var got = PSHTTR.Solve(path, u, v, kmin);
                                //var expected = 

                                //Assert.AreEqual(expected, got);

                                ++_passed;

                                if (_settings.ShowAll || _settings.OnlyPassed)
                                {
                                    sb.Append(Environment.NewLine);
                                    sb.Append($"Query: N = {n}, U = {u}, V = {v}, K = {kmin}");
                                    sb.Append(Environment.NewLine);
                                    sb.Append($"got : {got}");
                                    sb.Append(Environment.NewLine);
                                }
                            }
                            catch (Exception exception)
                            {
                                ++_falied;

                                sb.Append(Environment.NewLine);
                                sb.Append("Falied" + exception.StackTrace + " " + exception.Message);
                                sb.Append(Environment.NewLine);

                                //sb.Append($"got : {got} vs expected: {expected}");
                                sb.Append(Environment.NewLine);
                            }

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
