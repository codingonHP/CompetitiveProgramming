using System;
using System.Collections.Generic;
using System.IO;

namespace CompetitiveProgramming
{
    public class InfixToPostFix
    {
        #region Main

        protected static TextReader Reader;
        static void Process()
        {
            int testCaseCount = 0;
            string[] inputs;

#if DEBUG
            Reader = new StreamReader("..\\..\\input.txt");
            testCaseCount = ReadIntLine(Reader)[0];

            for (int i = 0; i < testCaseCount; i++)
            {
                inputs = ReadStringLine(Reader);
                Solve(inputs[0]);
            }

#else
            testCaseCount = ReadIntLine()[0];

            for (int i = 0; i < testCaseCount; i++)
            {
                inputs = ReadStringLine();
                Solve(inputs[0]);
            }
#endif

#if DEBUG
            Reader.Close();
#endif
        }

        public static void Solve(string input)
        {
            Stack<char> operatorStack = new Stack<char>();
            Stack<string> literalStack = new Stack<string>();
            string output = "";

            foreach (var c in input)
            {
                if (c == ')')
                {
                    output = "";

                    var b = literalStack.Pop();
                    var a = literalStack.Pop();
                    literalStack.Pop();

                    output += a + b;
                    output += operatorStack.Pop();

                    literalStack.Push(output);
                }
                else if (c == '/' || c == '*' || c == '+' || c == '-' || c == '^')
                {
                    operatorStack.Push(c);
                }
                else
                {
                    literalStack.Push(c + "");
                }
            }

            Console.WriteLine(output);
        }

        #endregion

        #region InputOutput

        private static int[] ReadIntLine(TextReader reader = null)
        {
            var read = reader != null ? reader.ReadLine() : Console.ReadLine();
            var readTokens = read.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            int[] result = new int[readTokens.Length];

            int i = 0;
            foreach (var s in readTokens)
            {
                result[i++] = int.Parse(s);
            }

            return result;
        }
        private static string[] ReadStringLine(TextReader reader = null)
        {
            var read = reader != null ? reader.ReadLine() : Console.ReadLine();
            var readTokens = read.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return readTokens;
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