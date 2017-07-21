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
using System.Runtime.InteropServices;
using System.Text;

namespace CpForCompetitiveProgrammingHRSameOccurrence
{
    public static class HRSameOccurrence
    {
        #region Main

        private const long Mod = 1000000007L;
        private const long MaxArrySize = 100000000L;
        private static ConsoleHelper Console { get; set; }

        static HRSameOccurrence()
        {
            Console = new ConsoleHelper();
        }

        //public static void Main_Solver(string[] args)
        public static void Main(string[] args)
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
            var tc = Console.NextInts(2);
            var n = tc[0];
            var q = tc[1];

            var array = Console.NextInts(n);
            //SegmentTree segmentTree = new SegmentTree(array);
            //DTree dtree = new DTree(array);
            //dtree.Print();

            SolveDc solveDc = new SolveDc(array);

            for (int i = 0; i < q; i++)
            {
                var xy = Console.NextLongs(2);

                //var output = Solve(segmentTree, xy[0], xy[1]);

                //var output = Solve2(dtree, xy[0], xy[1], array.Length);

                //var output = SolveBf(array, xy[0], xy[1]);

                var output = SolveUsingDc(solveDc, xy[0], xy[1]);


                Console.WriteLine(output);
            }
        }

#endif

        public static long SolveUsingDc(SolveDc dc, long x, long y)
        {
            return dc.GetCount(x, y);
        }

        public static long SolveBf(int[] array, long x, long y)
        {
            long count = 0;

            for (int i = 0; i < array.Length; i++)
            {
                for (int j = i; j < array.Length; j++)
                {
                    count += GetCountUsingBf(array, x, y, i, j);
                }
            }

            return count;
        }

        private static long GetCountUsingBf(int[] array, long x, long y, int i, int j)
        {
            long xCount = 0;
            long yCount = 0;

            for (int k = i; k <= j; k++)
            {
                if (array[k] == x)
                {
                    ++xCount;
                }

                if (array[k] == y)
                {
                    ++yCount;
                }
            }

            if (xCount == yCount)
            {
                return 1;
            }

            return 0;
        }

        public static long Solve2(DTree tree, long x, long y, long len)
        {
            long count = 0;

            for (long i = 0; i < len; i++)
            {
                for (long j = i; j < len; j++)
                {
                    Dictionary<long, long> d = tree.Root[i][j];
                    long xCount = 0, yCount = 0;

                    if (d.ContainsKey(x))
                    {
                        xCount = d[x];
                    }

                    if (d.ContainsKey(y))
                    {
                        yCount = d[y];
                    }

                    if (xCount == yCount)
                    {

                        //foreach (var dv in d)
                        //{
                        //    Console.Write($"{dv.Key} [{dv.Value}] ");
                        //}

                        //Console.WriteLine($"");
                        count++;
                    }
                }
            }

            return count;
        }

        public static long Solve(SegmentTree tree, long x, long y)
        {
            var array = tree.Array;
            var n = array.Length;
            var rootDict = tree.Root.MapDictionary;

            if (!rootDict.ContainsKey(x) && !rootDict.ContainsKey(y) || x == y)
            {
                return n * (n + 1) / 2;
            }


            long count = 0;

            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = i + 1; j < array.Length; j++)
                {
                    count += tree.Query(i, j, x, y);
                }
            }

            return count;
        }


        public class Node
        {
            public Dictionary<long, long> MapDictionary = new Dictionary<long, long>();
            public int LeftIndex { get; set; }
            public int RightIndex { get; set; }

            public Node Left { get; set; }
            public Node Right { get; set; }
        }

        public class SegmentTree
        {
            public int[] Array { get; }
            public readonly int NodesCount;
            public Node Root;

            public SegmentTree(int[] array)
            {
                Array = array;
                NodesCount = array.Length;
                CreateTree(array);
            }


            public void CreateTree(int[] array)
            {
                var k = ArrayLength(array.Length);
                Root = Travel(array, null, k, 0);
            }

            private Node Travel(int[] array, Node[] nodeArray, int size, int depth)
            {
                if (size == 0)
                {
                    return nodeArray[0];
                }

                var level = new Node[size];
                for (int i = 0; i < size; i++)
                {
                    if (depth == 0)
                    {

                        level[i] = new Node
                        {
                            LeftIndex = i,
                            RightIndex = i,
                            MapDictionary = i < array.Length ? new Dictionary<long, long> { { array[i], 1 } } :
                                new Dictionary<long, long>()
                        };
                    }
                    else
                    {
                        var dict = new Dictionary<long, long>();

                        var leftNode = nodeArray[2 * i];
                        var rightNode = nodeArray[2 * i + 1];

                        var leftNodeValue = leftNode.MapDictionary;
                        var rightNodeValue = rightNode.MapDictionary;

                        var leftIndex = leftNode.LeftIndex;
                        var rightIndex = rightNode.RightIndex;

                        foreach (var lvalue in leftNodeValue)
                        {
                            if (dict.ContainsKey(lvalue.Key))
                            {
                                var value = dict[lvalue.Key];
                                dict[lvalue.Key] = ++value;
                            }
                            else
                            {
                                dict.Add(lvalue.Key, 1);
                            }
                        }

                        foreach (var rvalue in rightNodeValue)
                        {
                            if (dict.ContainsKey(rvalue.Key))
                            {
                                var value = dict[rvalue.Key];
                                dict[rvalue.Key] = ++value;
                            }
                            else
                            {
                                dict.Add(rvalue.Key, 1);
                            }
                        }

                        level[i] = new Node
                        {
                            MapDictionary = dict,
                            LeftIndex = leftIndex,
                            RightIndex = rightIndex,
                            Left = leftNode,
                            Right = rightNode
                        };
                    }
                }

                return Travel(array, level, size / 2, ++depth);
            }

            private int ArrayLength(int n)
            {
                n--;
                n |= n >> 1;
                n |= n >> 2;
                n |= n >> 4;
                n |= n >> 8;
                n |= n >> 16;
                n |= n >> 32;
                n |= n >> 64;
                n |= n >> 128;

                n++;

                return n;
            }

            public long Query(long i, long j, long x, long y)
            {
                long count = 0;

                TraverseTree(Root, i, j, x, y, ref count);
                return count;
            }

            public void TraverseTree(Node node, long i, long j, long x, long y, ref long count)
            {
                if (node == null)
                {
                    return;
                }

                var range = Range(i, j, node.LeftIndex, node.RightIndex);

                if (range == -1)
                {
                    return;
                }

                if (range == 1)
                {
                    long xCount = 0, yCount = 0;
                    var dict = node.MapDictionary;

                    if (dict.ContainsKey(x))
                    {
                        xCount = dict[x];
                    }

                    if (dict.ContainsKey(y))
                    {
                        yCount = dict[y];
                    }

                    if (xCount == yCount)
                    {
                        count++;
                    }

                    return;
                }

                TraverseTree(node.Left, i, j, x, y, ref count);

                TraverseTree(node.Right, i, j, x, y, ref count);
            }

            private int Range(long min, long max, long nodeMin, long nodeMax)
            {
                if (nodeMin >= min && nodeMax <= max)
                {
                    return 1; // in range
                }

                if (nodeMax < min || nodeMin > max)
                {
                    return -1; //out of bounds
                }

                return 0; // overlapp

            }

        }

        public class DTree
        {
            public Dictionary<long, Dictionary<long, Dictionary<long, long>>> Root { get; }

            public DTree(int[] array)
            {
                Root = CreateDictionary(array);
            }

            public Dictionary<long, Dictionary<long, Dictionary<long, long>>> CreateDictionary(int[] array)
            {
                var dictionary = new Dictionary<long, Dictionary<long, Dictionary<long, long>>>();

                for (int i = 0; i < array.Length; i++)
                {
                    var colDictionary = new Dictionary<long, Dictionary<long, long>>();
                    var valuesDict = new Dictionary<long, long>();

                    for (int j = i; j < array.Length; j++)
                    {
                        var key = array[j];

                        if (valuesDict.ContainsKey(key))
                        {
                            var value = valuesDict[key];
                            valuesDict[key] = ++value;
                        }
                        else
                        {
                            valuesDict.Add(key, 1);
                        }


                        var saveDict = new Dictionary<long, long>();
                        foreach (var value in valuesDict)
                        {
                            saveDict.Add(value.Key, value.Value);
                        }

                        colDictionary.Add(j, saveDict);
                    }

                    dictionary.Add(i, colDictionary);
                }

                return dictionary;
            }

            public void Print()
            {
                foreach (var row in Root)
                {
                    foreach (var col in row.Value)
                    {
                        foreach (var d in col.Value)
                        {
                            Console.Write($"{d.Key} [{d.Value}] ");
                        }

                        Console.WriteLine($"");
                    }

                    Console.WriteLine($"");
                }
            }
        }

        public class SolveDc
        {
            public int[] Array { get; }
            readonly Dictionary<long, List<long>> _dictionary = new Dictionary<long, List<long>>();
            private readonly long _count;

            public SolveDc(int[] array)
            {
                Array = array;
                _count = array.Length;

                for (var index = 0; index < array.Length; index++)
                {
                    var element = array[index];
                    if (_dictionary.ContainsKey(element))
                    {
                        var value = _dictionary[element];
                        value.Add(index);
                    }
                    else
                    {
                        _dictionary.Add(element, new List<long> { index });
                    }
                }
            }

            public long GetCount(long x, long y)
            {
                var n = _count;
                long breakPointKey = -1;

                if (x == y || !_dictionary.ContainsKey(x) && !_dictionary.ContainsKey(y))
                {
                    return n * (n + 1) / 2;
                }

                if (_dictionary.ContainsKey(x) && !_dictionary.ContainsKey(y))
                {
                    breakPointKey = x;
                }
                else if (_dictionary.ContainsKey(y) && !_dictionary.ContainsKey(x))
                {
                    breakPointKey = y;
                }

                if (breakPointKey != -1)
                {
                    var breakpoints = _dictionary[breakPointKey];
                    return GetCount(breakpoints, x, y);
                }

                var xbp = _dictionary[x];
                var ybp = _dictionary[y];

                foreach (var e in ybp)
                {
                    xbp.Add(e);
                }

                xbp = xbp.OrderBy(e => e).ToList();

                return GetCount(xbp, x, y);

            }

            public long GetCount(List<long> breakpoints, long x, long y)
            {
                breakpoints.Add(_count);

                long count = 0;
                long prevBreakPoint = -1;

                for (var index = 0; index < breakpoints.Count; index++)
                {
                    var currbreakPoint = breakpoints[index];

                    if (index > 0)
                    {
                        prevBreakPoint = breakpoints[index - 1];
                    }

                    long prevlen;
                    if (prevBreakPoint == -1)
                    {
                        prevlen = currbreakPoint;
                    }
                    else
                    {
                        prevlen = currbreakPoint - prevBreakPoint - 1;
                    }

                    count += prevlen * (prevlen + 1) / 2;

                    long xCount = 0, yCount = 0;

                    for (var i = currbreakPoint; i < breakpoints.Count; i++)
                    {
                        var bindex = breakpoints[(int)i];
                        if (bindex < Array.Length && Array[bindex] == x)
                        {
                            ++xCount;
                        }
                        else if (bindex < Array.Length && Array[bindex] == y)
                        {
                            ++yCount;
                        }

                        if (xCount == yCount)
                        {
                            long nextLen = 0;
                            if (i != breakpoints.Count - 1)
                            {
                                var nextbreakpoint = breakpoints[(int)i + 1];
                                nextLen = nextbreakpoint - currbreakPoint - 1;
                            }

                            count += prevlen;
                            count += nextLen;
                            count += prevlen * nextLen;
                            ++count;
                        }
                    }

                }

                return count;
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

    #endregion


}
