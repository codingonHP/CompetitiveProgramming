using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
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
    public class CompetitiveProgrammingUnitTestForMaxSubArrayTest
    {
        private readonly Settings _settings = new Settings
        {
            ShowAll = false,
            OnlyFailed = false,
            OnlyPassed = false,
            IterationCount = 10,
            SubIterationCount = 1,
            MinRandom = -10,
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
                int n = random.Next(1, 1000);

                for (int i = 0; i < _settings.SubIterationCount; i++)
                {
                    int[] array = new int[n];

                    try
                    {
                        for (int j = 0; j < n; j++)
                        {
                            var next = random.Next(_settings.MinRandom, _settings.MaxRandom);
                            while (next == 0)
                            {
                                next = random.Next(-10, 10);
                            }

                            array[j] = next;
                        }

                        ++_totalTestCase;

                        //TEST CASE RUN
                        //var got = 
                        //var expected = 

                        try
                        {
                            //Assert.AreEqual(expected, got);
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

                                //sb.Append($"got : {got} vs expected: {expected}");
                                sb.Append($" --> PASSED TEST CASE");
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

                            foreach (var wval in array)
                            {
                                sb.Append(wval + " ");
                            }

                            sb.Append(Environment.NewLine);
                            sb.Append(Environment.NewLine);

                            //sb.Append($"got : {got} vs expected: {expected}");
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
