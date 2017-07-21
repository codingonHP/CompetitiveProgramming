using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CompetitiveProgramming;
using CpForCompetitiveProgrammingIPCTRAIN;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestIPCTRAINTest
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
    public class UnitTestForIPCTRAINTest
    {
        private readonly Settings _settings = new Settings
        {
            ShowAll = false,
            OnlyFailed = true,
            OnlyPassed = false,
            IterationCount = 100,
            SubIterationCount = 50,
            MinRandom = 1000,
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

            for (int k = 0; k < _settings.IterationCount; ++k)
            {
                int n = random.Next(_settings.MinRandom, _settings.MaxRandom);
                int d = random.Next(_settings.MinRandom, _settings.MaxRandom);

                for (int i = 0; i < _settings.SubIterationCount; i++)
                {
                    var actual = new List<IPCTRAIN.Teacher>();
                    var teacherList = new List<IPCTRAIN.Teacher>();
                    var tempTeacherList = new List<IPCTRAIN.Teacher>();

                    try
                    {
                        for (int j = 0; j < n; j++)
                        {
                            var ad = random.Next(1, d);
                            var cc = random.Next(1, d);
                            var sl = random.Next(_settings.MinRandom, _settings.MaxRandom);

                            var teacher = new IPCTRAIN.Teacher
                            {
                                ClassCount = cc,
                                ArrivalDay = ad,
                                SadnessLevel = sl
                            };

                            var teacher2 = new IPCTRAIN.Teacher
                            {
                                ClassCount = cc,
                                ArrivalDay = ad,
                                SadnessLevel = sl
                            };

                            var teacher3 = new IPCTRAIN.Teacher
                            {
                                ClassCount = cc,
                                ArrivalDay = ad,
                                SadnessLevel = sl
                            };

                            teacherList.Add(teacher);
                            tempTeacherList.Add(teacher2);
                            actual.Add(teacher3);
                        }

                        ++_totalTestCase;

                        //TEST CASE RUN
                        var got = IPCTRAIN.Solve(n, d, teacherList);
                        var expected = IPCTRAIN.SolveBf(n, d, tempTeacherList);

                        try
                        {
                            Assert.AreEqual(expected, got);
                            ++_passed;

                            if (_settings.ShowAll || _settings.OnlyPassed)
                            {
                                sb.Append(Environment.NewLine);
                                sb.Append($"PASSED TEST CASE: N = {n}, D = {d}");
                                sb.Append(Environment.NewLine);
                                sb.Append($"got : {got} vs expected: {expected}");
                                sb.Append(Environment.NewLine);
                                sb.Append("TEST CASE DATA");
                                sb.Append(Environment.NewLine);
                                sb.Append(Environment.NewLine);


                                sb.Append($"{n} {d}");
                                sb.Append(Environment.NewLine);
                                foreach (var teacher in actual)
                                {
                                    sb.Append($"{teacher.ArrivalDay} {teacher.ClassCount} {teacher.SadnessLevel}");
                                    sb.Append(Environment.NewLine);
                                }
                            }

                        }
                        catch (Exception)
                        {
                            ++_falied;

                            sb.Append(Environment.NewLine);
                            sb.Append($"FAILED TEST CASE: N = {n}, D = {d}");
                            sb.Append(Environment.NewLine);
                            sb.Append($"got : {got} vs expected: {expected}");
                            sb.Append(Environment.NewLine);
                            sb.Append("TEST CASE DATA");
                            sb.Append(Environment.NewLine);
                            sb.Append(Environment.NewLine);


                            sb.Append($"{n} {d}");
                            sb.Append(Environment.NewLine);
                            foreach (var teacher in actual)
                            {
                                sb.Append($"{teacher.ArrivalDay} {teacher.ClassCount} {teacher.SadnessLevel}");
                                sb.Append(Environment.NewLine);
                            }
                        }
                    }
                    catch (Exception exception)
                    {
                        ++_falied;

                        sb.Append(Environment.NewLine);
                        sb.Append($"NEW TEST CASE: N = {n}, D = {d}");
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
