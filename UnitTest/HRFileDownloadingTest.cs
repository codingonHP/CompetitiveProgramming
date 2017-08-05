using System;
using System.IO;
using System.Text;
using CpForCompetitiveProgrammingHRDownloadFile;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestHRFileDownloadingTest
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
    public class UnitTestForHRFileDownloadingTest
    {
        private readonly Settings _settings = new Settings
        {
            ShowAll = false,
            OnlyFailed = true,
            OnlyPassed = false,
            IterationCount = 1,
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
                int n = random.Next(2, 100);
                int[,] array = new int[n, 2];

                for (int i = 0; i < _settings.SubIterationCount; i++)
                {
                    try
                    {
                        var time = 0;
                        for (int j = 0; j < n; j++)
                        {
                            var speed = random.Next(_settings.MinRandom, _settings.MaxRandom);

                            array[j, 0] = time;
                            array[j, 1] = speed;

                            var nextTime = random.Next(_settings.MinRandom, _settings.MaxRandom);
                            while(nextTime <= time)
                            {
                                nextTime = random.Next(1, 1000);
                            }

                            time = nextTime;
                        }

                        var fileSize = random.Next(0, 1000);

                        sb.Append("New Test Case");
                        sb.Append(Environment.NewLine);

                        sb.Append($"{n} {fileSize}");
                        sb.Append(Environment.NewLine);

                        for (int j = 0; j < n; j++)
                        {
                            sb.Append($"{array[j, 0]} {array[j, 1]}");
                            sb.Append(Environment.NewLine);
                        }


                        ++_totalTestCase;

                        //TEST CASE RUN
                        var got = HRDownloadFile.SolveFast(array, n, fileSize);
                        var expected = HRDownloadFile.Solve(array, n, fileSize);

                        try
                        {
                            Assert.AreEqual(expected, got);
                            ++_passed;

                            if (_settings.ShowAll || _settings.OnlyPassed)
                            {
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
                        sb.Append("Exception");
                        sb.Append(Environment.NewLine);

                        foreach (var wval in array)
                        {
                            sb.Append(wval + " ");
                        }

                        sb.Append(Environment.NewLine);

                        sb.Append($" --> Exception in TEST CASE : Exception : {exception.Message}, {exception.StackTrace}");
                        sb.Append(Environment.NewLine);
                    }
                }
            }

            sb.Insert(0, $"Total Test cases run = {_totalTestCase}, Passed = {_passed}, Failed = {_falied}");
            File.WriteAllText("unit-test-input.txt", sb.ToString());

        }

        [TestMethod]
        public void GenerateTestCases()
        {
            StringBuilder sb = new StringBuilder();

            sb.Append(Environment.NewLine);

            Random random = new Random(9997);


            for (int i = 0; i < 1; i++)
            {
                int n = random.Next(100000, 100000);
                int[,] array = new int[n, 2];

                var time = 0;
                for (int j = 0; j < n; j++)
                {
                    var speed = random.Next(1,10);

                    array[j, 0] = time;
                    array[j, 1] = speed;

                    var nextTime = random.Next(time + 1, time + 10000);
                    time = nextTime;
                }

                var fileSize = 1000000;

                sb.Append($"{n} {fileSize}");
                sb.Append(Environment.NewLine);

                for (int j = 0; j < n; j++)
                {
                    sb.Append($"{array[j, 0]} {array[j, 1]}");
                    sb.Append(Environment.NewLine);
                }

                sb.Append(Environment.NewLine);
                File.AppendAllText("unit-test-input.txt", sb.ToString());
            }
            
        }
    }
}
