#pragma warning disable IDE0018
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
            //var output = tree.QueryBf(nodes);

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
            public Dictionary<long, Dictionary<long, long>> SparseDictionary = new Dictionary<long, Dictionary<long, long>>();
            private long _counter;

            public long RmqQuery(long n)
            {
                RmqDfs(Root, null);
                var array = DfsRmq.ToArray();

                FillSparseTable(array);

                long s = 0;

                for (long i = 1; i <= n; i++)
                {
                    for (long j = i; j <= n; j++)
                    {
                        long lvalue = RmqTravel(i, j);
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


          

           

            private long RmqTravel(long from, long to)
            {
                try
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

                    var lca = QuerySparseTable(l, r);
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

                    weight += RmqTravel(temp, toNode, prevNode, lca);

                    return weight;
                }
                catch (Exception exception)
                {
                    throw;
                }
            }

          

            private long RmqTravel(Node fromNode, Node toNode, Node prev, long lca)
            {
                try
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

                        if (prev != null && nextNode.Value == prev.Value || fromNode.Parent != null && nextNode.InternalValue == fromNode.Parent.InternalValue)
                        {
                            continue;
                        }

                        var to = toNode.InternalValue;
                        var nxt = nextNode.InternalValue;
                        //Swap(ref to, ref nxt);

                        var nextLca = QuerySparseTable(to, nxt);

                        if (nextLca != nxt)
                        {
                            continue;
                        }

                        sum += RmqTravel(nextNode, toNode, fromNode, lca);
                    }

                    return sum;

                }
                catch (Exception exception)
                {

                    throw;
                }
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

                if (node.ReachableNodes.Count > 1)
                {
                    DfsRmq.RemoveAt(DfsRmq.Count - 1);
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

           

            public void FillSparseTable(long[] array)
            {
                var len = array.Length;
                var sdLen = PrevPowerOf2(len);


                for (int i = 1; i <= sdLen; i *= 2)
                {
                    var col = new Dictionary<long, long>();
                    SparseDictionary.Add(i, col);

                    var prev = i / 2;

                    if (prev == 0)
                    {
                        for (int j = 0; j < array.Length; j++)
                        {
                            col.Add(j, array[j]);
                        }
                    }
                    else
                    {
                        var prevRow = SparseDictionary[prev];
                        for (int j = 0; j < array.Length; j++)
                        {
                            if (i + j <= array.Length)
                            {
                                col.Add(j, Math.Min(prevRow[j], prevRow[j + prev]));
                            }
                            else
                            {
                                col.Add(j, long.MaxValue);
                            }
                        }
                    }
                }
            }

            public long QuerySparseTable(long a, long b)
            {
                var fa = FirstEncounter[a];
                var fb = FirstEncounter[b];

                Swap(ref fa, ref fb);

                long len = fb - fa + 1;
                len = PrevPowerOf2(len);

                var row = SparseDictionary[len];

                var i = row[fa];
                var j = row[fb - len + 1];

                return Math.Min(i, j);
            }

            public long PrevPowerOf2(long n)
            {
                var pow = Math.Floor(Math.Log(n, 2));
                return (long)Math.Pow(2, pow);
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

        
      
    }

   

    #endregion


}
