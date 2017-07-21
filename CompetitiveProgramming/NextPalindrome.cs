using System;
using System.Collections.Generic;
using System.IO;

namespace CompetitiveProgramming
{
    public class NextPalindrome
    {
        #region Main

        protected static TextReader Reader;
        public static int MainSolver()
        {
            Dictionary<char, int> buff = new Dictionary<char, int>
            {
                {'0' ,0},
                {'1' ,1},
                {'2' ,2},
                {'3' ,3},
                {'4' ,4},
                {'5' ,5},
                {'6' ,6},
                {'7' ,7},
                {'8' ,8},
                {'9' ,9},
            };


#if DEBUG
            Reader = new StreamReader("..\\..\\input.txt");
            var testCaseCount = ReadIntLine(Reader)[0];

            for (int i = 0; i < testCaseCount; i++)
            {
                var inputs = ReadStringLine(Reader);
                if (inputs.Length > 0)
                {
                    Solve(inputs[0], buff);
                }
            }

#else
            var testCaseCount = ReadIntLine()[0];

            for (int i = 0; i < testCaseCount; i++)
            {
                var inputs = ReadStringLine();
                Solve(inputs[0], buff);
            }
#endif

#if DEBUG
            Reader.Close();
#endif

            return 0;
        }

        public static void Solve(string input, Dictionary<char, int> buff)
        {
            string output;
            var inputCharArry = input.ToCharArray();

            var isEven = inputCharArry.Length % 2 == 0;
            int middle = inputCharArry.Length / 2;

            if (isEven)
            {
                inputCharArry = Parse(inputCharArry, middle - 2, buff);
            }
            else
            {
                inputCharArry = Parse(inputCharArry, middle - 1, buff);
            }

            isEven = inputCharArry.Length % 2 == 0;

            if (isEven)
            {
                output = EvenLengthProcessing(inputCharArry, buff);
            }
            else
            {
                output = OddLengthProcessing(inputCharArry, buff);
            }

            Console.WriteLine(output);
        }

        public static string OddLengthProcessing(char[] inputCharArry, Dictionary<char, int> buff)
        {
            int middle = inputCharArry.Length / 2;

            for (int i = 0; i < middle; i++)
            {
                inputCharArry[inputCharArry.Length - 1 - i] = inputCharArry[i];
            }

            return new string(inputCharArry);
        }

        public static string EvenLengthProcessing(char[] inputCharArry, Dictionary<char, int> buff)
        {
            int middle = inputCharArry.Length / 2;

            if (buff[inputCharArry[middle - 1]] < buff[inputCharArry[middle]])
            {
                inputCharArry[middle - 1]++;
                inputCharArry[middle] = inputCharArry[middle - 1];
            }
            else
            {
                inputCharArry[middle] = inputCharArry[middle - 1];
            }

            for (int i = 0; i < middle - 1; i++)
            {
                inputCharArry[inputCharArry.Length - 1 - i] = inputCharArry[i];
            }

            return new string(inputCharArry);
        }

        private static char[] Parse(char[] inputCharArry, int iValue, Dictionary<char, int> buff)
        {
            int middle = inputCharArry.Length / 2;
            bool allAreEqual = true;

            for (int i = iValue, j = middle + 1; i >= 0; i--, ++j)
            {
                if (inputCharArry[i] < inputCharArry[j])
                {
                    inputCharArry = IncrementArrayValue(inputCharArry, 0, middle, buff);
                    allAreEqual = false;
                    break;
                }

                if (inputCharArry[i] == inputCharArry[j])
                {
                    continue;
                }

                allAreEqual = false;
                break;
            }

            if (allAreEqual)
            {
                inputCharArry = IncrementArrayValue(inputCharArry, 0, middle, buff);
            }

            return inputCharArry;
        }

        private static char[] IncrementArrayValue(char[] inputCharArry, int start, int end, Dictionary<char, int> buff)
        {
            string input = "";

            bool arrayExpanded = false;

            for (int i = end; i >= start; i--)
            {
                var lastDigit = i == end;
                int value = buff[inputCharArry[i]];
                ++value;

                var carryGenerated = value / 10 >= 1;

                if (carryGenerated && lastDigit)
                {
                    inputCharArry[i] = inputCharArry.Length - 1 == end ? '1' : '0';
                }
                else if (carryGenerated)
                {
                    inputCharArry[i] = '0';
                }
                else
                {
                    inputCharArry[i] = value.ToString()[0];
                }

                if (i == 0)
                {
                    input = new string(inputCharArry);
                    if (carryGenerated)
                    {
                        input = "1" + input;
                        arrayExpanded = true;
                    }
                }

                if (carryGenerated) continue;

                input = new string(inputCharArry);
                break;
            }

            if (arrayExpanded)
            {
                inputCharArry = input.ToCharArray();
            }

            return inputCharArry;
        }

        #endregion

        #region InputOutput

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