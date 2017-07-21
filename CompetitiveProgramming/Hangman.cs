using System;
using System.Collections.Generic;
using System.IO;

namespace CompetitiveProgramming
{
    public class Hangman
    {

        #region Main

        protected static TextReader Reader;
        public static int MainSolver()
        {
            Dictionary<string, string> inputs = new Dictionary<string, string>();
#if DEBUG
            Reader = new StreamReader("..\\..\\input.txt");
            int tcCount = ReadInt(Reader);
            ReadString(Reader);

            for (int i = 0; i < tcCount; i++)
            {
                inputs.Add(ReadString(Reader), ReadString(Reader));
                Reader.ReadLine();
            }


#else
            int tcCount = ReadInt(Reader);
            ReadString();

            for (int i = 0; i < tcCount; i++)
            {
                inputs.Add(ReadString(), ReadString());
                Reader.ReadLine();
            }
         
#endif

#if DEBUG
            Reader.Close();
#endif

            foreach (var input in inputs)
            {
                Solved(input.Key, input.Value);
            }

            return 0;
        }

        public static void Solved(string actual, string guess)
        {
            if (actual == guess)
            {
                Console.WriteLine("You guessed it right");
                return;
            }

            Dictionary<char, List<int>> stringMap = new Dictionary<char, List<int>>();
            List<char> wrongGuess = new List<char>();

            int index = 0;
            int remainingChances = 7;
            int remainingGuesses = actual.Length;

            foreach (var s in actual)
            {
                if (stringMap.ContainsKey(s))
                {
                    var posList = stringMap[s];
                    posList.Add(index++);
                }
                else
                {
                    var posList = new List<int> { index++ };
                    stringMap.Add(s, posList);
                }
            }

            bool failed = false;

            var actualArry = actual.ToCharArray();

            foreach (char t in guess)
            {
                if (wrongGuess.Contains(t))
                {
                    continue;
                }

                if (stringMap.ContainsKey(t))
                {
                    var getPositions = stringMap[t];
                    if (actualArry[0] != '1')
                    {
                        foreach (var position in getPositions)
                        {
                            actualArry[position] = '1';
                        }

                        remainingGuesses -= getPositions.Count;
                        if (remainingGuesses <= 0)
                        {
                            remainingGuesses = 0;
                            break;
                        }
                    }
                }
                else
                {
                    wrongGuess.Add(t);
                    --remainingChances;
                    if (remainingChances == 0)
                    {
                        failed = true;
                        break;
                    }
                }
            }

            if (failed)
            {
                Console.WriteLine("loose");
            }
            else if (remainingGuesses > 0 && remainingChances > 0)
            {
                Console.WriteLine("You chickened out");
            }
            else if (remainingGuesses == 0)
            {
                Console.WriteLine("You guessed it right");
            }
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

    //Test Cases

    //9

    //honeymoon
    //omnyenh

    //vishal
    //a

    //apple
    //mango

    //rain
    //free

    //visualstudio
    //laksjdfjsdjlsslkjfdk

    //a
    //a

    //maya
    //amy

    //traveller
    //leratv

    //ilovemycoutntryindiaandiamproudofit
    //zzzzzzz

}