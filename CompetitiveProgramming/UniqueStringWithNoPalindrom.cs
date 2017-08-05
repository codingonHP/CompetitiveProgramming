using System;
using System.IO;
using System.Linq;

namespace CompetitiveProgramming
{
    class P345
    {
        static void MainSolver()
        {
            new Magatro().Solve();
        }
    }

    public class Scanner
    {
        private StreamReader _sr;

        private string[] _s;
        private int _index;
        private const char Separator = ' ';

        public Scanner(Stream source)
        {
            _index = 0;
            _s = new string[0];
            _sr = new StreamReader(source);
        }

        private string[] Line()
        {
            return _sr.ReadLine().Split(Separator);
        }

        public string Next()
        {
            string result;
            if (_index >= _s.Length)
            {
                _s = Line();
                _index = 0;
            }
            result = _s[_index];
            _index++;
            return result;
        }
        public int NextInt()
        {
            return int.Parse(Next());
        }
        public double NextDouble()
        {
            return double.Parse(Next());
        }
        public long NextLong()
        {
            return long.Parse(Next());
        }
        public decimal NextDecimal()
        {
            return decimal.Parse(Next());
        }
        public string[] StringArray(int index = 0)
        {
            Next();
            _index = _s.Length;
            return _s.Skip(index).ToArray();
        }
        public int[] IntArray(int index = 0)
        {
            return StringArray(index).Select(int.Parse).ToArray();
        }
        public long[] LongArray(int index = 0)
        {
            return StringArray(index).Select(long.Parse).ToArray();
        }
        public bool EndOfStream
        {
            get { return _sr.EndOfStream; }
        }
    }

    class Magatro
    {
        private int _n;
        private void Scan()
        {
            var cin = new Scanner(Console.OpenStandardInput());
            _n = cin.NextInt();
        }

        public void Solve()
        {
            Scan();
            var ans = new char[_n];
            for (int i = 0; i < _n; i++)
            {
                switch (i % 4)
                {
                    case 0:
                    case 1:
                        ans[i] = 'a';
                        break;
                    case 3:
                    case 2:
                        ans[i] = 'b';
                        break;
                }
            }
            Console.WriteLine(new string(ans));
        }
    }
}