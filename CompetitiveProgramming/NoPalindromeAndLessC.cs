using System;
using System.IO;

namespace CompetitiveProgramming
{
    public class NoPalindromeAndLessC
    {

        #region Main

        protected static TextReader Reader;
        public static int MainSolver()
        {

#if DEBUG
            Reader = new StreamReader("..\\..\\input.txt");
            int a = ReadInt(Reader);

#else
            int a = ReadInt();
         
#endif

#if DEBUG
            Reader.Close();
#endif

            char[] s = new char[a];
            for (int i = 0; i < a; i++)
            {
                s[i] = GetNext(s, i);
            }

            WriteLine(new string(s));

            return 0;
        }

        public static char GetNext(char[] currentString, int i)
        {
            if (i < 2)
            {
                return 'a';
            }

            var c = currentString[i - 2];
            if (c == 'a')
            {
                return 'b';
            }

            if (c == 'b')
            {
                return 'a';
            }

            if (c == 'c')
            {
                return 'a';
            }

            return ' ';
        }


        #endregion

        #region InputOutput

        private static int ReadInt(TextReader reader = null)
        {
            if (reader != null)
            {
                var data = reader.ReadLine().Trim();
                return int.Parse(data);
            }
            else
            {
                var data = Console.ReadLine().Trim();
                return int.Parse(data);
            }
        }

        private static string ReadString(TextReader reader = null)
        {
            if (reader != null)
            {
                return reader.ReadLine().Trim();
            }
            else
            {
                return Console.ReadLine().Trim();
            }
        }

        private static int[] ReadIntLine(TextReader reader = null)
        {
            var read = reader != null ? reader.ReadLine() : Console.ReadLine();
            if (read != null)
            {
                var readTokens = read.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                int[] result = new int[readTokens.Length];

                int i = 0;
                foreach (var s in readTokens)
                {
                    result[i++] = int.Parse(s);
                }

                return result.Length > 0 ? result : new[] { 0 };
            }

            return new[] { 0 };
        }

        private static string[] ReadStringLine(TextReader reader = null)
        {
            var read = reader != null ? reader.ReadLine() : Console.ReadLine();
            if (read != null)
            {
                var readTokens = read.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                return readTokens.Length == 0 ? new[] { "0" } : readTokens;
            }

            return new[] { "0" };
        }

        private static void WriteLine(string output)
        {
            Console.WriteLine(output);
        }
        private static void WriteLine(string[] output)
        {
            foreach (var s in output)
            {
                Console.WriteLine(s);
            }
        }
        private static void WriteLine(int[] output)
        {
            foreach (var s in output)
            {
                Console.WriteLine(s);
            }
        }

        #endregion

    }

}