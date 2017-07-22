using System;
using System.IO;
using System.Text;
using CpForCompetitiveProgrammingLCA;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTestLcaUnitTest
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
    public class UnitTestForLcaUnitTest
    {
        private readonly Settings _settings = new Settings
        {
            ShowAll = false,
            OnlyFailed = true,
            OnlyPassed = false,
            IterationCount = 1,
            SubIterationCount = 1,
            MinRandom = 10000,
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
            LCA.Tree tree = null;
            int n = 0;

            for (int k = 0; k < _settings.IterationCount; ++k)
            {
                for (int i = 0; i < _settings.SubIterationCount; i++)
                {

                    try
                    {
                        n = random.Next(100, 500);

                        tree = new LCA.Tree();
                        tree = LCA.Tree.RandomTree(n);

                        ++_totalTestCase;


                        //TEST CASE RUN
                        var got = tree.Query(n);
                        var expected = tree.QueryBf(n);

                        try
                        {
                            Assert.AreEqual(expected, got);
                            ++_passed;

                            if (_settings.ShowAll || _settings.OnlyPassed)
                            {
                                sb.Append(Environment.NewLine);
                                sb.Append($"NEW TEST CASE: N = {n}");
                                sb.Append(Environment.NewLine);
                                sb.Append("Tree");
                                sb.Append(Environment.NewLine);

                                foreach (var wval in tree.Nodes)
                                {
                                    sb.Append("value = " + wval.Value.Value + " weight = " + wval.Value.Weight);
                                    sb.Append(Environment.NewLine);
                                    sb.Append($"Reachable Nodes: ");
                                    sb.Append(Environment.NewLine);
                                    foreach (var valueReachableNode in wval.Value.ReachableNodes)
                                    {
                                        sb.Append("value = " + valueReachableNode.Value.Value + " weight = " + valueReachableNode.Value.Weight);
                                        sb.Append(Environment.NewLine);
                                    }

                                    sb.Append(Environment.NewLine);
                                    sb.Append(Environment.NewLine);
                                }

                                sb.Append(Environment.NewLine);

                                sb.Append($"got : {got}, expected = {expected}");
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
                            sb.Append("Tree Exception");
                            sb.Append(Environment.NewLine);

                            foreach (var wval in tree.Nodes)
                            {
                                sb.Append("value = " + wval.Value.Value + " weight = " + wval.Value.Weight);
                                sb.Append(Environment.NewLine);
                                sb.Append($"Reachable Nodes: ");
                                sb.Append(Environment.NewLine);
                                foreach (var valueReachableNode in wval.Value.ReachableNodes)
                                {
                                    sb.Append("value = " + valueReachableNode.Value.Value + " weight = " + valueReachableNode.Value.Weight);
                                    sb.Append(Environment.NewLine);
                                }

                                sb.Append(Environment.NewLine);
                            }

                            sb.Append(Environment.NewLine);
                            sb.Append(Environment.NewLine);
                            sb.Append("Tree End");

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

                        foreach (var wval in tree.Nodes)
                        {
                            sb.Append("value" + wval.Value.Value + " weight = " + wval.Value.Weight);
                            sb.Append(Environment.NewLine);
                            sb.Append($"Reachable Nodes: ");
                            sb.Append(Environment.NewLine);
                            foreach (var valueReachableNode in wval.Value.ReachableNodes)
                            {
                                sb.Append("value" + valueReachableNode.Value.Value + " weight = " + valueReachableNode.Value.Weight);
                                sb.Append(Environment.NewLine);
                            }

                            sb.Append(Environment.NewLine);
                            sb.Append(Environment.NewLine);
                        }

                        sb.Append(Environment.NewLine);
                        sb.Append(Environment.NewLine);
                        sb.Append("Tree End");

                        sb.Append($" --> Exception in TEST CASE : Exception : {exception.Message}, {exception.StackTrace}");
                        sb.Append(Environment.NewLine);
                    }
                }
            }

            sb.Insert(0, $"Total Test cases run = {_totalTestCase}, Passed = {_passed}, Failed = {_falied}");
            File.WriteAllText("unit-test-input.txt", sb.ToString());

        }
    }
}
