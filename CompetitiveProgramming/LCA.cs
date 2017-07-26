﻿#pragma warning disable IDE0018
#define TESTCASES
//Tools and default solution template belongs to https://www.hackerearth.com/@christophe_savard
//Solution to the problem belongs to me
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace CpForCompetitiveProgrammingLCA
{
    public static class LCA
    {
        #region Main

        private const long Mod = 1000000007L;
        private const long MaxArrySize = 100000000L;
        private static ConsoleHelper Console { get; set; }

        static LCA()
        {
            Console = new ConsoleHelper();
        }

        public static void Main_Solver(string[] args)
        //public static void Main(string[] args)
        {
#if DEBUG
            Stopwatch timer = Stopwatch.StartNew();
#endif
            using (Console)
            {
#if TESTCASES
                TestCases();
#else // !TESTCASES
                Solve();
#endif
            }
#if DEBUG
            timer.Stop();
            System.Console.SetOut(File.AppendText(@"../../output.txt"));
            System.Console.Write($"\r\n\r\nTotal run time: {timer.Elapsed.TotalSeconds:0.000}s at {DateTime.Now}");
            System.Console.Out.Dispose();
#endif
        }



        #endregion

        #region Solution

#if TESTCASES

        private static void TestCases()
        {
            var nodes = Console.NextInt(true);


            Tree tree = new Tree();
            for (int i = 1; i < nodes; i++)
            {
                var input = Console.NextInts(2);
          
                tree.AddNode(input[0], input[1]);
            }

            var values = Console.NextLongs(nodes);

            for (var index = 0; index < values.Length; index++)
            {
                long value = values[index];
                tree.Nodes[index + 1].Weight = value;
            }

            var output = tree.RmqQuery(nodes);

            Console.WriteLine(output);
        }

#endif

        public class Node
        {
            public Node Parent { get; set; }
            public Dictionary<long, Node> ReachableNodes { get; set; } = new Dictionary<long, Node>();
            public long Value { get; set; }
            public long Weight { get; set; }
            public bool IsRoot { get; set; }
            public long InternalValue { get; set; } = -1;

        }

        public class Tree
        {
            public Node Root { get; private set; }
            public readonly Dictionary<long, Node> Nodes = new Dictionary<long, Node>();
            public readonly Dictionary<string, long> SumDictionary = new Dictionary<string, long>();
            public readonly Dictionary<long, long> FeDictionary = new Dictionary<long, long>
            {
                {0, 1},
                {1, 1}
            };

            public readonly List<long> DfsRmq = new List<long>();
            public readonly Dictionary<long, long> FirstEncounter = new Dictionary<long, long>();
            public readonly Dictionary<long, long> MappingDictionary = new Dictionary<long, long>();
            public Dictionary<long, List<long>> RmqDictionary = new Dictionary<long, List<long>>();
            private long _counter = 0;

            public long Query(long n)
            {
                long s = 0;
                Dfs();

                for (int i = 1; i <= n; i++)
                {
                    for (int j = i; j <= n; j++)
                    {
                        var lvalue = GetLValue(i, j);

                        if (i != j)
                        {
                            lvalue *= 2;
                        }

                        s += lvalue;

                        if (s >= Mod)
                        {
                            s %= Mod;
                        }

                    }
                }

                return s;
            }

            public long QueryBf(long n)
            {
                long s = 0;

                for (int i = 1; i <= n; i++)
                {
                    for (int j = i; j <= n; j++)
                    {
                        Stack<Node> stack = new Stack<Node>();
                        bool reached = false;

                        Travel(this, i, i, j, ref reached, stack);

                        var lvalue = GetFValue(stack.Sum(sv => sv.Weight));
                        if (i != j)
                        {
                            lvalue *= 2;
                        }

                        s += lvalue;

                        if (s >= Mod)
                        {
                            s %= Mod;
                        }

                    }
                }

                return s;
            }

            private long GetLValue(int a, int b)
            {
                checked
                {
                    var key = string.Format("{0}-{1}", a, b);

                    if (SumDictionary.ContainsKey(key))
                    {
                        return GetFValue(SumDictionary[key]);
                    }

                    var aKey = string.Format("{0}-{1}", 1, a);
                    var bKey = string.Format("{0}-{1}", 1, b);

                    var sumA = SumDictionary[aKey];
                    var sumB = SumDictionary[bKey];

                    var lca = Lca(a, b);

                    var valueOfLca = SumDictionary[string.Format("{0}-{1}", lca, lca)];
                    var sumTillLca = SumDictionary[string.Format("{0}-{1}", 1, lca)];

                    var sum = sumA - sumTillLca + sumB - sumTillLca + valueOfLca;

                    return GetFValue(sum);
                }
            }

            public void AddNode(long a, long b)
            {
                Node nodea, nodeb;
                bool nodeaparent = false, nodebparent = false;

                if (Nodes.ContainsKey(a))
                {
                    nodea = Nodes[a];
                    if (nodea.IsRoot || nodea.Parent != null)
                    {
                        nodeaparent = true;
                    }
                }
                else
                {
                    nodea = new Node { Value = a, Weight = a };
                    Nodes.Add(a, nodea);
                }

                if (Nodes.ContainsKey(b))
                {
                    nodeb = Nodes[b];
                    if (nodeb.IsRoot || nodeb.Parent != null)
                    {
                        nodebparent = true;
                    }
                }
                else
                {
                    nodeb = new Node { Value = b, Weight = b };
                    Nodes.Add(b, nodeb);
                }

                nodea.ReachableNodes.Add(b, nodeb);
                nodeb.ReachableNodes.Add(a, nodea);

                if (Root == null)
                {
                    Root = nodea;
                    nodea.Parent = null;
                    nodea.IsRoot = true;
                    nodeb.Parent = nodea;
                }
                else
                {
                    if (nodeaparent)
                    {
                        nodeb.Parent = nodea;
                        foreach (var reachableNode in nodeb.ReachableNodes)
                        {
                            if (!reachableNode.Value.IsRoot && reachableNode.Value.Parent == null)
                            {
                                reachableNode.Value.Parent = nodeb;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else if (nodebparent)
                    {
                        nodea.Parent = nodeb;

                        foreach (var reachableNode in nodea.ReachableNodes)
                        {
                            if (!reachableNode.Value.IsRoot && reachableNode.Value.Parent == null)
                            {
                                reachableNode.Value.Parent = nodea;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                }
            }

            public long? Lca(int a, int b)
            {
                bool found = false;
                return TraverseForLca(Root, null, a, b, ref found);
            }

            private long? TraverseForLca(Node node, Node prev, long a, long b, ref bool found)
            {
                if (node == null)
                {
                    return null;
                }

                if (node.Value == a)
                {
                    return a;
                }

                if (node.Value == b)
                {
                    return b;
                }


                long? f = null;

                foreach (KeyValuePair<long, Node> reachableNode in node.ReachableNodes)
                {
                    var n = reachableNode.Value;

                    if (prev != null && n.Value == prev.Value)
                    {
                        continue;
                    }

                    long? lca = TraverseForLca(n, node, a, b, ref found);

                    if (found)
                    {
                        return lca;
                    }

                    if (lca != null && f == null)
                    {
                        f = lca;
                    }
                    else if (lca != null)
                    {
                        found = true;
                        return node.Value;
                    }
                }

                return f;
            }

            public void Dfs()
            {
                TravelForDfs(Root, null, 0);
            }


            public long RmqQuery(long n)
            {
                RmqDfs(Root, null);
                var array = DfsRmq.ToArray();
                const int inf = (int)1e9;

                var segmentTree = new AdvancedDataStructure.SegmentTree<long>(array, inf,
                    (value, index) => value, (nodeLeft, nodeRight) => Math.Min(nodeLeft.Value, nodeRight.Value));

                long s = 0;

                for (long i = 1; i <= n; i++)
                {
                    for (long j = i; j <= n; j++)
                    {
                        var lvalue = RmqTravel(i, j, segmentTree);
                        lvalue = GetFValue(lvalue);

                        if (i != j)
                        {
                            lvalue *= 2;
                        }

                        s += lvalue;

                        if (s >= Mod)
                        {
                            s %= Mod;
                        }

                    }
                }

                return s;
            }

            private void Swap(ref long low, ref long high)
            {
                var temp = high;
                if (low > high)
                {
                    high = low;
                    low = temp;
                }
            }

            private long RmqTravel(long from, long to, AdvancedDataStructure.SegmentTree<long> segmentTree)
            {
                var fromNode = Nodes[from];
                var toNode = Nodes[to];

                if (from == to)
                {
                    return fromNode.Weight;
                }

                var l = fromNode.InternalValue;
                var r = toNode.InternalValue;
                Swap(ref l, ref r);

                var lca = segmentTree.Query(l, to, Math.Min, node => node.Value);
                long weight = 0;

                var temp = fromNode;
                var lcaNode = Nodes[MappingDictionary[lca]];

                Node prevNode = null;
                while (temp != null && temp.InternalValue != lcaNode.InternalValue)
                {
                    weight += temp.Weight;
                    prevNode = temp;
                    temp = temp.Parent;
                }

                //weight += temp.Weight;

               

                weight += RmqTravel(temp, toNode, prevNode, lca, segmentTree);

                return weight;
            }

            private long RmqTravel(Node fromNode, Node toNode, Node prev, long lca, AdvancedDataStructure.SegmentTree<long> segmentTree)
            {
                if (fromNode == null || toNode == null)
                {
                    return 0;
                }

                long sum = fromNode.Weight;

                foreach (KeyValuePair<long, Node> reachableNode in fromNode.ReachableNodes)
                {
                    var nextNode = reachableNode.Value;

                    if (nextNode.InternalValue == toNode.InternalValue)
                    {
                        return sum + nextNode.Weight;
                    }

                    if (prev != null && nextNode.Value == prev.Value)
                    {
                        continue;
                    }

                    var l = toNode.InternalValue;
                    var r = nextNode.InternalValue;
                    Swap(ref l, ref r);

                    var nextLca = segmentTree.Query(l, r, Math.Min, node => node.Value);

                    if (nextLca == lca)
                    {
                        continue;
                    }
                   
                    sum += RmqTravel(nextNode, toNode, fromNode, lca, segmentTree);
                }

                return sum;
            }

            private void RmqDfs(Node node, Node prev)
            {
                if (node == null)
                {
                    return;
                }

                if (node.InternalValue == -1)
                {
                    node.InternalValue = _counter++;
                    FirstEncounter.Add(node.InternalValue, DfsRmq.Count);
                    MappingDictionary.Add(node.InternalValue, node.Value);
                }

                DfsRmq.Add(node.InternalValue);

                foreach (KeyValuePair<long, Node> reachableNode in node.ReachableNodes)
                {
                    var nextNode = reachableNode.Value;

                    if (prev != null && nextNode.Value == prev.Value)
                    {
                        continue;
                    }
                    
                    RmqDfs(nextNode, node);

                    DfsRmq.Add(node.InternalValue);
                }

                if (node.ReachableNodes.Count >  1)
                {
                    DfsRmq.RemoveAt(DfsRmq.Count - 1);
                }
            }

            private void TravelForDfs(Node node, Node prev, long recSum)
            {
                if (node == null)
                {
                    return;
                }

                var key = string.Format("{0}-{1}", 1, node.Value);
                var iKey = string.Format("{0}-{1}", node.Value, node.Value);

                var weight = node.Weight;

                if (!SumDictionary.ContainsKey(iKey))
                {
                    SumDictionary.Add(iKey, weight);
                }

                checked
                {
                    weight = recSum + node.Weight;
                }

                foreach (KeyValuePair<long, Node> reachableNode in node.ReachableNodes)
                {
                    var n = reachableNode.Value;

                    if (prev != null && n.Value == prev.Value)
                    {
                        if (!SumDictionary.ContainsKey(key))
                        {
                            SumDictionary.Add(key, weight);
                        }

                        continue;
                    }

                    if (!SumDictionary.ContainsKey(key))
                    {
                        SumDictionary.Add(key, weight);
                    }

                    TravelForDfs(n, node, weight);
                }
            }

            public long GetFValue(long n)
            {

                if (n <= 0)
                {
                    return 1;
                }

                if (FeDictionary.ContainsKey(n))
                {
                    return FeDictionary[n];
                }

                long k = n / 2;
                long f;

                if (n % 2 == 0)
                {
                    f = (GetFValue(k) * GetFValue(k) + GetFValue(k - 1) * GetFValue(k - 1)) % Mod;
                    FeDictionary.Add(n, f);
                    return f;
                }

                f = (GetFValue(k) * GetFValue(k + 1) + GetFValue(k - 1) * GetFValue(k)) % Mod;
                FeDictionary.Add(n, f);
                return f;
            }

            public static Tree RandomTree(long nodeCount)
            {
                Queue<long> nodeQueue = new Queue<long>();

                Random random = new Random(1997);

                for (int i = 1; i <= nodeCount; i++)
                {
                    nodeQueue.Enqueue(i);
                }

                var nodeA = nodeQueue.Dequeue();
                var nodeB = nodeQueue.Dequeue();

                Tree tree = new Tree();
                tree.AddNode(nodeA, nodeB);
                List<long> treeNodeList = new List<long> { nodeA, nodeB };

                while (nodeQueue.Count > 0)
                {
                    var nextNode = nodeQueue.Dequeue();
                    var randomSelect = random.Next(0, tree.Nodes.Count - 1);
                    tree.AddNode(nextNode, treeNodeList[randomSelect]);
                    treeNodeList.Add(nextNode);
                }

                return tree;
            }

            public void Travel(Tree path, long prev, long from, long to, ref bool reached, Stack<Node> stack)
            {
                stack.Push(path.Nodes[from]);
                var fromNode = path.Nodes[from];

                if (fromNode.Value == to)
                {
                    reached = true;
                    return;
                }

                foreach (var reachableNode in fromNode.ReachableNodes)
                {
                    if (reachableNode.Value.Value == prev)
                    {
                        continue;
                    }

                    Travel(path: path,
                        prev: from,
                        from: reachableNode.Value.Value,
                        to: to,
                        reached: ref reached,
                        stack: stack
                    );

                    if (reached)
                    {
                        break;
                    }
                }

                if (!reached)
                {
                    stack.Pop();
                }
            }

            public void FillSparseTable(long[] array)
            {
                var len = 1;


            }

            public long PrevPowerOf2(long n)
            {
                var pow = Math.Floor(Math.Log(n, 2));
                return (long)Math.Pow(2, pow);
            }

        }



        #endregion
    }


    //Tools belongs to https://www.hackerearth.com/@christophe_savard
    #region Tools
    [DebuggerStepThrough]
    public sealed class ConsoleHelper : IDisposable
    {
        #region Constants
        private const int BaseSize = 1048576;
        private static readonly char[] NumBuffer = new char[20];
        #endregion

        #region Fields
        private readonly BufferedStream _inStream;
        private readonly StreamWriter _outStream;

        private readonly byte[] _inBuffer;
        private int _inputIndex;
        private int _bufferEnd;
        #endregion

        #region Properties
        public int BufferSize { get; set; }

        public bool Open { get; private set; }
        #endregion

        #region Constructors

        public ConsoleHelper() : this(BaseSize) { }


        public ConsoleHelper(int bufferSize)
        {
            // Open the input/output streams
#if DEBUG
            // Test mode
            this._inStream = new BufferedStream(File.OpenRead(@"../../input.txt"), bufferSize);
            this._outStream = new StreamWriter(File.Create(@"../../output.txt", bufferSize), Encoding.ASCII, bufferSize);
#else // !DEBUG
            // Submission mode
            this._inStream = new BufferedStream(Console.OpenStandardInput(bufferSize), bufferSize); // Submission stream
            this._outStream = new StreamWriter(Console.OpenStandardOutput(bufferSize), Encoding.ASCII, bufferSize);
#endif

            // Set fields
            this._inBuffer = new byte[bufferSize];
            this._inputIndex = this._bufferEnd = 0;
            this.BufferSize = bufferSize;
            this.Open = true;
        }
        #endregion

        #region Static methods

        public static bool ValidateChar(int i)
        {
            return i >= ' ';
        }


        public static bool ValidateCharNoSpace(int i)
        {
            return i > ' ';
        }


        public static bool ValidateNumber(int i)
        {
            return i >= '0' && i <= '9';
        }


        public static bool IsEndline(int i)
        {
            return i == '\n' || i == '\0';
        }


        private static int GetIntBuffer(int n)
        {
            int head = 20;
            bool neg;
            if (n < 0)
            {
                neg = true;
                n = -n;
            }
            else { neg = false; }

            do
            {
                NumBuffer[--head] = (char)((n % 10) + 48);
                n /= 10;
            }
            while (n > 0);

            if (neg) { NumBuffer[--head] = '-'; }
            return head;
        }


        private static int GetLongBuffer(long n)
        {
            int head = 20;
            bool neg;
            if (n < 0L)
            {
                neg = true;
                n = -n;
            }
            else { neg = false; }

            do
            {
                NumBuffer[--head] = (char)((n % 10L) + 48L);
                n /= 10L;
            }
            while (n > 0L);

            if (neg) { NumBuffer[--head] = '-'; }
            return head;
        }
        #endregion

        #region Methods

        public byte Read()
        {
            CheckBuffer();
            return this._inBuffer[this._inputIndex++];
        }


        public byte Peek()
        {
            CheckBuffer();
            return this._inBuffer[this._inputIndex];
        }


        public void Skip(int n = 1)
        {
            this._inputIndex += n;
        }


        private void CheckBuffer()
        {
            // If we reach the end of the buffer, load more data
            if (this._inputIndex >= this._bufferEnd)
            {
                this._inputIndex = this._inputIndex - this._bufferEnd;
                this._bufferEnd = this._inStream.Read(this._inBuffer, 0, this.BufferSize);

                // If nothing was added, add a null char at the start
                if (this._bufferEnd < 1) { this._inBuffer[this._bufferEnd++] = 0; }
            }
        }


        public char NextChar()
        {
            return (char)Read();
        }


        public string Next()
        {
            byte b = SkipInvalid();
            ValidateEndline(b);

            // Append all characters
            StringBuilder sb = new StringBuilder().Append((char)b);
            b = Peek();
            while (ValidateCharNoSpace(b))
            {
                // Peek to not consume terminator
                sb.Append((char)b);
                Skip();
                b = Peek();
            }

            return sb.ToString();
        }


        public int NextInt(bool moveToNextLine = false)
        {
            // Skip invalids
            byte b = SkipInvalid();
            ValidateEndline(b);

            // Verify for negative
            bool neg = false;
            if (b == '-')
            {
                neg = true;
                b = Read();
            }

            // Get first digit
            if (!ValidateNumber(b)) { throw new FormatException("Integer parsing has failed because the string contained invalid characters"); }

            int n = b - '0';
            b = Peek();
            while (ValidateNumber(b))
            {
                // Peek to not consume terminator, and check for overflow
                n = checked((n << 3) + (n << 1) + (b - '0'));
                Skip();
                b = Peek();
            }
            // If the character causing the exit is a valid ASCII character, the integer isn't correct formatted
            if (ValidateCharNoSpace(b)) { throw new FormatException("Integer parsing has failed because the string contained invalid characters"); }

            if (moveToNextLine)
            {
                SkipToNextLine();
            }

            return neg ? -n : n;
        }


        public long NextLong(bool moveToNextLine = false)
        {
            byte b = SkipInvalid();
            ValidateEndline(b);

            // Verify negative
            bool neg = false;
            if (b == '-')
            {
                neg = true;
                b = Read();
            }

            // Get first digit
            if (!ValidateNumber(b)) { throw new FormatException("Integer parsing has failed because the string contained invalid characters"); }

            long n = b - '0';
            b = Peek();
            while (ValidateNumber(b))
            {
                // Peek to not consume terminator, and check for overflow
                n = checked((n << 3) + (n << 1) + (b - '0'));
                Skip();
                b = Peek();
            }
            // If the character causing the exit is a valid ASCII character, the long isn't correct formatted
            if (ValidateCharNoSpace(b)) { throw new FormatException("Long parsing has failed because the string contained invalid characters"); }

            if (moveToNextLine)
            {
                SkipToNextLine();
            }

            return neg ? -n : n;
        }


        public double NextDouble()
        {
            return double.Parse(Next());
        }


        public int[] NextInts(int n)
        {
            int[] array = new int[n];
            for (int i = 0; i < n; i++)
            {
                array[i] = NextInt();
            }

            SkipToNextLine();
            return array;
        }


        public long[] NextLongs(int n)
        {
            long[] array = new long[n];
            for (int i = 0; i < n; i++)
            {
                array[i] = NextLong();
            }

            SkipToNextLine();
            return array;
        }


        public int[,] NextMatrix(int n, int m)
        {
            int[,] matrix = new int[n, m];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < m; j++)
                {
                    matrix[i, j] = NextInt();
                }

                SkipToNextLine();
            }

            return matrix;
        }


        public string NextLine()
        {
            byte b = SkipInvalid();
            if (b == 0)
            {
                // Consume newline and return empty string
                Skip();
                return string.Empty;
            }

            // Read all the characters until the next linefeed
            StringBuilder sb = new StringBuilder().Append((char)b);
            b = Read();
            while (!IsEndline(b))
            {
                // Don't append special characters, but don't exit
                if (ValidateChar(b))
                {
                    sb.Append((char)b);
                }
                b = Read();
            }

            return sb.ToString();
        }


        public void SkipNext()
        {
            byte b = SkipInvalid();
            ValidateEndline(b);

            for (b = Peek(); ValidateCharNoSpace(b); b = Peek())
            {
                Skip();
            }
        }


        public void SkipToNextLine()
        {
            for (byte b = Read(); !IsEndline(b); b = Read()) { }
        }


        public IEnumerable<int> EnumerateInts(int n)
        {
            while (n-- > 0)
            {
                yield return NextInt();
            }

            SkipToNextLine();
        }


        public IEnumerable<char> EnumerateLine()
        {
            for (char c = NextChar(); !IsEndline(c); c = NextChar())
            {
                if (ValidateChar(c))
                {
                    yield return c;
                }
            }
        }


        private void ValidateEndline(byte b)
        {
            // If empty char
            if (b == 0)
            {
                // Go back a char and throw
                this._inputIndex--;
                throw new InvalidOperationException("No values left on line");
            }
        }


        private byte SkipInvalid()
        {
            byte b = Peek();
            if (IsEndline(b)) { return 0; }

            while (!ValidateCharNoSpace(b))
            {
                Skip();
                b = Peek();
                // Return empty char if we meet an linefeed or empty char
                if (IsEndline(b)) { return 0; }
            }

            return Read();
        }


        public void Write(char c)
        {
            this._outStream.Write(c);
        }


        public void Write(char[] buffer)
        {
            this._outStream.Write(buffer);
        }


        public void Write(string s)
        {
            this._outStream.Write(s);
        }


        public void Write(int n)
        {
            int head = GetIntBuffer(n);
            this._outStream.Write(NumBuffer, head, 20 - head);
        }


        public void Write(long n)
        {
            int head = GetLongBuffer(n);
            this._outStream.Write(NumBuffer, head, 20 - head);
        }


        public void Write(StringBuilder sb)
        {
            this._outStream.Write(sb.ToCharArray());
        }


        public void Write(object o)
        {
            this._outStream.Write(o);
        }


        public void Write<T>(IEnumerable<T> e, string separator = "")
        {
            this._outStream.Write(new StringBuilder().AppendJoin(e, separator).ToCharArray());
        }


        public void Write<T>(IEnumerable<T> e, char separator)
        {
            this._outStream.Write(new StringBuilder().AppendJoin(e, separator).ToCharArray());
        }


        public void WriteLine()
        {
            this._outStream.WriteLine();
        }


        public void WriteLine(char c)
        {
            this._outStream.WriteLine(c);
        }


        public void WriteLine(char[] buffer)
        {
            this._outStream.WriteLine(buffer);
        }


        public void WriteLine(string s)
        {
            this._outStream.WriteLine(s);
        }

        public void WriteLine(int n)
        {
            int head = GetIntBuffer(n);
            this._outStream.WriteLine(NumBuffer, head, 20 - head);
        }


        public void WriteLine(long n)
        {
            int head = GetLongBuffer(n);
            this._outStream.WriteLine(NumBuffer, head, 20 - head);
        }


        public void WriteLine(StringBuilder sb)
        {
            this._outStream.WriteLine(sb.ToCharArray());
        }


        public void WriteLine<T>(IEnumerable<T> e, string separator = "")
        {
            this._outStream.WriteLine(new StringBuilder().AppendJoin(e, separator).ToCharArray());
        }


        public void WriteLine<T>(IEnumerable<T> e, char separator)
        {
            this._outStream.WriteLine(new StringBuilder().AppendJoin(e, separator).ToCharArray());
        }


        public void WriteLine(object o)
        {
            this._outStream.WriteLine(o);
        }


        public void Flush()
        {
            this._outStream.Flush();
        }


        public void Dispose()
        {
            if (this.Open)
            {
                Flush();
                this._inStream.Dispose();
                this._outStream.Dispose();
                this.Open = false;
            }
        }
        #endregion
    }

    public static class Extensions
    {
        #region Enumerable extensions

        public static bool EqualTo<T>(this IEnumerable<T> e, int count)
        {
            if (count < 0) { return false; }

            int total = 0;
            return !e.Any(t => ++total > count) && total == count;
        }

        public static void ForEach<T>(this IEnumerable<T> e, Action<T> action)
        {
            foreach (T t in e)
            {
                action(t);
            }
        }

        public static bool IsEmpty(this ICollection c)
        {
            return c.Count == 0;
        }

        public static bool GreaterThan<T>(this IEnumerable<T> e, int count)
        {
            if (count < 0) { return true; }

            int total = 0;
            return e.Any(t => ++total > count);
        }

        public static string Join<T>(this IEnumerable<T> e, string separator = "")
        {
            return new StringBuilder().AppendJoin(e, separator).ToString();
        }

        public static string Join<T>(this IEnumerable<T> e, char separator)
        {
            return new StringBuilder().AppendJoin(e, separator).ToString();
        }

        public static bool LessThan<T>(this IEnumerable<T> e, int count)
        {
            if (count <= 0) { return false; }

            int total = 0;
            return !e.Any(t => ++total >= count);
        }

        public static T MaxValue<T, TU>(this IEnumerable<T> e, Func<T, TU> selector) where TU : IComparable<TU>
        {
            using (IEnumerator<T> enumerator = e.GetEnumerator())
            {
                if (!enumerator.MoveNext()) { throw new InvalidOperationException("No elements in sequence"); }

                T max = enumerator.Current;
                TU value = selector(max);
                while (enumerator.MoveNext())
                {
                    TU v = selector(enumerator.Current);
                    if (value.CompareTo(v) < 0)
                    {
                        max = enumerator.Current;
                        value = v;
                    }
                }

                return max;
            }
        }

        public static T MinValue<T, TU>(this IEnumerable<T> e, Func<T, TU> selector) where TU : IComparable<TU>
        {
            using (IEnumerator<T> enumerator = e.GetEnumerator())
            {
                if (!enumerator.MoveNext()) { throw new InvalidOperationException("No elements in sequence"); }

                T min = enumerator.Current;
                TU value = selector(min);
                while (enumerator.MoveNext())
                {
                    TU v = selector(enumerator.Current);
                    if (value.CompareTo(v) > 0)
                    {
                        min = enumerator.Current;
                        value = v;
                    }
                }

                return min;
            }
        }

        public static List<List<T>> DeepClone<T>(this IEnumerable<IEnumerable<T>> e)
        {
            var newList = new List<List<T>>();

            foreach (var item in e)
            {
                List<T> newItem = new List<T>();

                foreach (var inner in item)
                {
                    newItem.Add(inner);
                }

                newList.Add(newItem);
            }

            return newList;
        }

        public static int StringDiff(this string s, string other)
        {
            int diff = 0;

            for (int i = 0; i < s.Length; i++)
            {
                if (i == other.Length)
                {
                    break;
                }

                if (s[i] != other[i])
                {
                    ++diff;
                }
            }

            return diff;
        }

        public static bool ForAllPermutation<T>(this IEnumerable<T> collection, Func<T[], bool> funcExecuteAndTellIfShouldStop)
        {
            var items = collection.ToArray();
            int countOfItem = items.Length;

            if (countOfItem <= 1)
            {
                return funcExecuteAndTellIfShouldStop(items);
            }

            var indexes = new int[countOfItem];
            for (int i = 0; i < countOfItem; i++)
            {
                indexes[i] = 0;
            }

            if (funcExecuteAndTellIfShouldStop(items))
            {
                return true;
            }

            for (int i = 1; i < countOfItem;)
            {
                if (indexes[i] < i)
                {
                    if ((i & 1) == 1)
                    {
                        Swap(ref items[i], ref items[indexes[i]]);
                    }
                    else
                    {
                        Swap(ref items[i], ref items[0]);
                    }

                    if (funcExecuteAndTellIfShouldStop(items))
                    {
                        return true;
                    }

                    indexes[i]++;
                    i = 1;
                }
                else
                {
                    indexes[i++] = 0;
                }
            }

            return false;
        }

        static void Swap<T>(ref T a, ref T b)
        {
            T temp = a;
            a = b;
            b = temp;
        }

        #endregion

        #region String extensions

        public static StringBuilder AppendJoin<T>(this StringBuilder sb, IEnumerable<T> source, string separator = "")
        {
            using (IEnumerator<T> e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    sb.Append(e.Current);
                    while (e.MoveNext())
                    {
                        sb.Append(separator).Append(e.Current);
                    }
                }
            }

            return sb;
        }


        public static StringBuilder AppendJoin<T>(this StringBuilder sb, IEnumerable<T> source, char separator)
        {
            using (IEnumerator<T> e = source.GetEnumerator())
            {
                if (e.MoveNext())
                {
                    sb.Append(e.Current);
                    while (e.MoveNext())
                    {
                        sb.Append(separator).Append(e.Current);
                    }
                }
            }

            return sb;
        }


        public static bool IsEmpty(this string s)
        {
            return String.IsNullOrEmpty(s);
        }


        public static string SubStr(this string s, int start, int end)
        {
            return s.Substring(start, (end - start) + 1);
        }


        public static char[] ToCharArray(this StringBuilder sb)
        {
            char[] buffer = new char[sb.Length];
            sb.CopyTo(0, buffer, 0, sb.Length);
            return buffer;
        }
        #endregion

        #region Number extensions

        public static bool IsPair(this int n)
        {
            return (n & 1) == 0;
        }


        public static bool IsPair(this long n)
        {
            return (n & 1L) == 0;
        }


        public static int Mod(this int n, int m)
        {
            return ((n % m) + m) % m;
        }


        public static long Mod(this long n, long m)
        {
            return ((n % m) + m) % m;
        }


        public static int Triangle(this int n)
        {
            return (n * (n + 1)) / 2;
        }


        public static long Triangle(this long n)
        {
            return (n * (n + 1L)) / 2L;
        }

        /// <summary>
        /// Counts the amount of set bits (1s in the binary representation) of a given integer
        /// </summary>
        /// <param name="n">Integer to get the set bits from</param>
        /// <returns>Amount of set bits of the integer</returns>
        public static int SetBits(this int n)
        {
            n = n - ((n >> 1) & 0x55555555);
            n = (n & 0x33333333) + ((n >> 2) & 0x33333333);
            return (((n + (n >> 4)) & 0x0F0F0F0F) * 0x01010101) >> 24;
        }

        /// <summary>
        /// Counts the amount of set bits (1s in the binary representation) of a given long
        /// </summary>
        /// <param name="n">Long to get the set bits from</param>
        /// <returns>Amount of set bits of the long</returns>
        public static long SetBits(this long n)
        {
            n = n - ((n >> 1) & 0x5555555555555555);
            n = (n & 0x3333333333333333) + ((n >> 2) & 0x3333333333333333);
            return (((n + (n >> 4)) & 0xF0F0F0F0F0F0F0F) * 0x101010101010101) >> 56;
        }


        public static int SumDigits(this int n)
        {
            int total = 0;
            while (n > 0)
            {
                total += n % 10;
                n /= 10;
            }
            return total;
        }
        #endregion

        private static void Swap(long[] arr, int i, int j)
        {
            var t = arr[i];
            arr[i] = arr[j];
            arr[j] = t;
        }

        private static int Partition(long[] arr, int l, int h, bool asc)
        {
            var x = arr[h];
            int i = (l - 1);

            for (int j = l; j <= h - 1; j++)
            {
                if (arr[j] <= x)
                {
                    i++;

                    if (asc)
                    {
                        Swap(arr, i, j);
                    }
                    else
                    {
                        Swap(arr, j, i);
                    }

                }
            }

            if (asc)
            {
                Swap(arr, i + 1, h);
            }
            else
            {
                Swap(arr, h, i + 1);
            }

            return (i + 1);
        }

        public static void QuickSort(this long[] arr, bool asc)
        {
            int l = 0;
            int h = arr.Length - 1;

            // create auxiliary stack
            int[] stack = new int[h - l + 1];

            // initialize top of stack
            int top = -1;

            // push initial values in the stack
            stack[++top] = l;
            stack[++top] = h;

            // keep popping elements until stack is not empty
            while (top >= 0)
            {
                // pop h and l
                h = stack[top--];
                l = stack[top--];

                // set pivot element at it's proper position
                int p = Partition(arr, l, h, asc);

                // If there are elements on left side of pivot,
                // then push left side to stack
                if (p - 1 > l)
                {
                    stack[++top] = l;
                    stack[++top] = p - 1;
                }

                // If there are elements on right side of pivot,
                // then push right side to stack
                if (p + 1 < h)
                {
                    stack[++top] = p + 1;
                    stack[++top] = h;
                }
            }
        }

        public static int BinarySearch(long[] a, long key)
        {
            int low = 0, high = a.Length - 1;
            while (low <= high)
            {
                int mid = (low + high) / 2;
                if (a[mid] == key)
                {
                    high = mid;
                    break;
                }
                else if (a[mid] < key)
                    low = mid + 1;
                else
                    high = mid - 1;
            }

            if (high == a.Length - 1)
                return high;
            while ((high + 1 < a.Length) && a[high + 1] == key)
                high++;
            return high;

        }
    }

    public static class AdvancedDataStructure
    {
        public class SegmentTree<T>
        {

            public class SegmentTreeNode
            {
                public T Value { get; set; }
                public long MinRange { get; set; }
                public long MaxRange { get; set; }
                public SegmentTreeNode LeftNode { get; set; }
                public SegmentTreeNode RightNode { get; set; }
            }

            public Dictionary<long, SegmentTreeNode> SegmentDictionary { get; } = new Dictionary<long, SegmentTreeNode>();
            public SegmentTreeNode Root { get; private set; }
            public T[] Array { get; }
            public T DefaultValue { get; set; }

            public Func<T, long, T> BaseValueProvider { get; }
            public Func<SegmentTreeNode, SegmentTreeNode, T> LevelValueProvider { get; }

            public SegmentTree(T[] array, T defaultValue, Func<T, long, T> baseValueProvider, Func<SegmentTreeNode, SegmentTreeNode, T> levelValueProvider)
            {
                Array = array;
                DefaultValue = defaultValue;
                BaseValueProvider = baseValueProvider;
                LevelValueProvider = levelValueProvider;
                CreateTree(baseValueProvider, levelValueProvider);
            }

            public T Query(long l, long r, Func<T, T, T> queryFunc, Func<SegmentTreeNode, T> inRangeValueProvider)
            {
                var node = Root;
                var value = Travel(node, l, r, queryFunc, inRangeValueProvider);

                return value;
            }

            private void CreateTree(Func<T, long, T> baseValueProvider, Func<SegmentTreeNode, SegmentTreeNode, T> levelValueProvider)
            {
                var baseLength = NextPowerOf2(Array.Length);
                var totalNodes = Math.Pow(2, Math.Log(baseLength, 2) + 1) - 1;
                long leftChildIndex = -1, rightChildIndex = -1;
                long levelLength = baseLength;

                for (int i = 0; i < totalNodes; i++)
                {
                    for (int j = 0; j < levelLength; j++)
                    {

                        SegmentTreeNode left = null, right = null;

                        if (i == baseLength)
                        {
                            leftChildIndex = 0;
                            rightChildIndex = 1;
                        }
                        else if (i > baseLength)
                        {
                            leftChildIndex += 2;
                            rightChildIndex += 2;
                        }
                        else
                        {
                            leftChildIndex = i;
                            rightChildIndex = i;
                        }

                        long minreach;
                        long maxreach;
                        if (i >= baseLength)
                        {
                            left = SegmentDictionary[leftChildIndex];
                            right = SegmentDictionary[rightChildIndex];
                            minreach = left.MinRange;
                            maxreach = right.MaxRange;
                        }
                        else
                        {
                            minreach = maxreach = i;
                        }

                        T value = DefaultValue;

                        if (left != null)
                        {
                            value = levelValueProvider(left, right);
                        }
                        else if (i < Array.Length)
                        {
                            value = baseValueProvider(Array[i], i);
                        }


                        SegmentTreeNode node = new SegmentTreeNode
                        {
                            Value = value,
                            LeftNode = left,
                            RightNode = right,
                            MinRange = minreach,
                            MaxRange = maxreach
                        };

                        SegmentDictionary.Add(i, node);

                        ++i;
                    }

                    levelLength /= 2;
                    --i;
                }

                Root = SegmentDictionary.Last().Value;
            }

            private T Travel(SegmentTreeNode node, long l, long r, Func<T, T, T> queryFunc, Func<SegmentTreeNode, T> inRangeFun)
            {
                if (node == null)
                {
                    return DefaultValue;
                }

                var nodeMinRange = node.MinRange;
                var nodeMaxRange = node.MaxRange;

                var state = RangeAssessment(l, r, nodeMinRange, nodeMaxRange);

                if (state == -1)
                {
                    return DefaultValue;
                }

                if (state == 1)
                {
                    return inRangeFun(node);
                }

                var lvalue = Travel(node.LeftNode, l, r, queryFunc, inRangeFun);
                var rvalue = Travel(node.RightNode, l, r, queryFunc, inRangeFun);

                return queryFunc(lvalue, rvalue);
            }

            private long RangeAssessment(long l, long r, long nodeMinRange, long nodeMaxRange)
            {
                if (nodeMinRange >= l && nodeMaxRange <= r)
                {
                    return 1; //in range, return from here.
                }

                if (nodeMaxRange < l || nodeMinRange > r)
                {
                    return -1; //out of range, ignore.
                }

                return 0; // go both sides.
            }

            private long NextPowerOf2(long n)
            {
                var p = Math.Ceiling(Math.Log(n, 2));
                return (long)Math.Pow(2, p);
            }
        }

    }

    #endregion


}
