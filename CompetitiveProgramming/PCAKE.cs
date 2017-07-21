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

namespace CpForPCAKE
{
    public static class PCAKE
    {
        #region Main

        private const long Mod = 1000000007L;
        private const long MaxArrySize = 100000000L;
        private static ConsoleHelper Console { get; set; }

        public const int MAXN = 1000001; //1000001;

        // stores smallest prime factor for every number
        static long[] spf = new long[MAXN];

        static PCAKE()
        {
            Console = new ConsoleHelper();
        }


        //public static void Main(string[] args)
        public static void Main_Sover(string[] args)
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

#if TESTCASES

        private static void TestCases()
        {
            int tc = Console.NextInt(true);
            Sieve();

            for (int i = 0; i < tc; i++)
            {
                long n = Console.NextLong(true);

                long[] array = Console.NextLongs((int)n);

                string numberOfWays = GetNumberOfWaysB(array);
                Console.WriteLine(numberOfWays);
            }
        }

        public static string GetNumberOfWaysB(long[] array)
        {
            var state = AllEvenOrOne(array);
            if (state == 1)
            {
                var n = array.Length;
                if (n > 10000)
                {
                    long terms;

                    if (n % 2 == 0)
                    {
                        terms = n / 2;
                    }
                    else
                    {
                        terms = (n + 1) / 2;
                    }

                    var remTerms = n - terms;


                    var fs = terms / 2 * (terms + 1) + "";
                    var tn = 1 + (terms - 1);
                    var ss = remTerms * (2 * (tn + 1) + (remTerms - 1)) / 2 + "";

                    var total = FindSum(fs, ss);

                    return total;
                }

                var ax = n * (n + 1) / 2 + "";

                return ax;
            }

            if (state == 2)
            {
                return Filter(array).Length + "";
            }


            //Dictionary<long, long> dirtryDictionary = new Dictionary<long, long>();
            //array = Filter(array);

            long count = 0;

            for (long i = 0; i < array.Length; i++)
            {
                long product = 1;
                for (long j = i; j < array.Length; j++)
                {
                    product *= array[j];
                    var p = HasMultiplePrimeInFactors(product);
                    //var p = HasMultiplePrimeInFactors(product, dirtyDictionary);

                    if (!p)
                    {
                        ++count;
                    }
                    else
                    {
                        break;
                    }
                }
            }

            return count + "";
        }

        public static string GetNumberOfWays(long[] array)
        {
            checked
            {
                if (array.All(e => e == 1))
                {
                    var n = array.Length;
                    if (n > 10000)
                    {
                        long terms;

                        if (n % 2 == 0)
                        {
                            terms = n / 2;
                        }
                        else
                        {
                            terms = (n + 1) / 2;
                        }

                        var remTerms = n - terms;


                        var fs = terms / 2 * (terms + 1) + "";
                        var tn = 1 + (terms - 1);
                        var ss = remTerms * (2 * (tn + 1) + (remTerms - 1)) / 2 + "";

                        var total = FindSum(fs, ss);

                        return total;
                    }

                    var ax = n * (n + 1) / 2 + "";

                    return ax;
                }
            }


            //var dirtyDictionary = new Dictionary<long, long>();

            long count = 0;

            for (long i = 0; i < array.Length; i++)
            {
                long product = 1;
                for (long j = i; j < array.Length; j++)
                {
                    product *= array[j];
                    var p = HasMultiplePrimeInFactors(product);
                    //var p = HasMultiplePrimeInFactors(product, dirtyDictionary);

                    if (!p)
                    {
                        ++count;
                    }
                    else
                    {
                        break;
                    }

                }
            }

            return count + "";
        }

        static string FindSum(string str1, string str2)
        {
            if (str1.Length > str2.Length)
                Swap(ref str1, ref str2);

            string str = "";

            int n1 = str1.Length, n2 = str2.Length;

            str1 = new string(str1.Reverse().ToArray());
            str2 = new string(str2.Reverse().ToArray());

            int carry = 0;
            for (int i = 0; i < n1; i++)
            {
                int sum = ((str1[i] - '0') + (str2[i] - '0') + carry);
                str += sum % 10;

                carry = sum / 10;
            }

            for (int i = n1; i < n2; i++)
            {
                int sum = ((str2[i] - '0') + carry);
                str += sum % 10;
                carry = sum / 10;
            }

            if (carry > 0)
                str += carry;

            str = new string(str.Reverse().ToArray());

            return str;
        }

        static int AllEvenOrOne(long[] array)
        {
            var allOne = true;
            var allEven = true;

            foreach (var e in array)
            {
                if (e != 1)
                {
                    allOne = false;
                }

                if (e % 2 != 0)
                {
                    allEven = false;
                }

                if (!allEven && !allOne)
                {
                    return -1;
                }
            }

            if (allOne)
            {
                return 1;
            }

            return 2;
        }

        static long[] Filter(long[] array)
        {
            return array.Where(e => !HasMultiplePrimeInFactors(e)).ToArray();
        }

        private static void Swap(ref string str1, ref string str2)
        {
            var temp = str1;
            str1 = str2;
            str2 = temp;
        }

        private static bool HasMultiplePrimeInFactors(long n, Dictionary<long, long> dirtyDictionary)
        {
            long k = n;
            if (dirtyDictionary.ContainsKey(n))
            {
                return true;
            }

            var pf = new Dictionary<long, long>();
            while (n % 2 == 0)
            {
                if (pf.ContainsKey(2))
                {
                    dirtyDictionary.Add(k, 1);
                    return true;
                }

                pf.Add(2, n);
                n /= 2;
            }

            for (int i = 3; i <= Math.Sqrt(n); i += 2)
            {
                while (n % i == 0)
                {
                    if (pf.ContainsKey(i))
                    {
                        dirtyDictionary.Add(k, 1);
                        return true;
                    }

                    pf.Add(i, n);

                    n /= i;
                }
            }

            if (n > 2)
            {
                if (pf.ContainsKey(n))
                {
                    dirtyDictionary.Add(k, 1);
                    return true;
                }

                pf.Add(n, n);
            }

            return false;
        }

        static void Sieve()
        {
            try
            {
                spf[1] = 1;
                for (long i = 2; i < MAXN; i++)

                    spf[i] = i;

                for (long i = 4; i < MAXN; i += 2)
                    spf[i] = 2;

                for (long i = 3; i * i < MAXN; i++)
                {
                    if (spf[i] == i)
                    {
                        for (long j = i * i; j < MAXN; j += i)
                            if (spf[j] == j)
                                spf[j] = i;
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        static bool HasMultiplePrimeInFactors(long x)
        {
            try
            {
                var pf = new Dictionary<long, long>();
                while (x != 1)
                {
                    if (pf.ContainsKey(spf[x]))
                    {
                        return true;
                    }

                    pf.Add(spf[x], 0);
                    x = x / spf[x];
                }
            }
            catch (Exception)
            {
            }

            return false;
        }

#endif

        #endregion

        #region Solution


        public static void Solve()
        {

        }

        public static string Multiply(string num1, string num2)
        {
            if (num1.Length == 0 || num2.Length == 0) return "";
            if (num1.Equals("0") || num2.Equals("0")) return "0";
            char[] c1 = new StringBuilder(num1).ToString().Reverse().ToArray();
            char[] c2 = new StringBuilder(num2).ToString().Reverse().ToArray();
            char[] c = new char[c1.Length + c2.Length + 1];

            for (int i = 0; i < c.Length; i++)
            {
                c[i] = '0';
            }

            for (int i = 0; i < c2.Length; i++)
            {
                int dig2 = c2[i] - '0';
                int carry = 0;
                for (int j = 0; j < c1.Length; j++)
                {
                    int dig1 = c1[j] - '0';
                    int temp = c[i + j] - '0';
                    int cur = dig1 * dig2 + temp + carry;
                    c[i + j] = (char)(cur % 10 + '0');
                    carry = cur / 10;
                }
                c[i + c1.Length] = (char)(carry + '0');
            }

            string ret = new string(new StringBuilder(new string(c)).ToString().Reverse().ToArray());
            int pos = 0;
            while (ret[pos] == '0' && pos < ret.Length) pos++;
            return ret.Substring(pos);
        }

        public static string Dec(string num1)
        {
            if (num1.Length == 0) return "";
            if (num1.Equals("0")) return "-1";

            char[] c1 = new StringBuilder(num1).ToString().Reverse().ToArray();

            bool carry = false;
            for (int i = 0; i < c1.Length; i++)
            {
                if (c1[i] == '0')
                {
                    c1[i] = '9';
                    carry = true;
                }
                else if (carry)
                {
                    c1[i] = (char)(c1[i] - '0' - 1);
                    carry = false;
                }
                else
                {
                    c1[i] = char.Parse((c1[i] - '0' - 1) + "");
                    break;
                }
            }

            return new string(c1.Reverse().Where(c => c != '\0').ToArray());

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
