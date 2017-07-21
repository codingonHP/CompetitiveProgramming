using System;

namespace CompetitiveProgramming
{
    public class CapgeminiMaxSumForFigureProblem
    {

        #region Main

        public static int DoMain()
        {

            var matrix = new int[6, 6];

            for (int i = 0; i < 6; i++)
            {
                var inputs = ReadStringLine();
                if (inputs.Length > 0)
                {
                    for (int j = 0; j < 6; j++)
                    {
                        matrix[i, j] = Convert.ToInt32(inputs[j]);
                    }
                }
            }

            Console.WriteLine(MaxSum(matrix));

            return 0;
        }

        private static int MaxSum(int[,] matrix)
        {
            int maxSum = 0;
            for (int y = 0; y <= 3; y++)
            {
                for (int x = 0; x <= 3; x++)
                {
                    var sum = 0;
                    for (int i = 0; i < 3; i++)
                    {
                        sum += matrix[y, x + i];
                    }

                    sum += matrix[y + 1, x + 1];

                    for (int i = 0; i < 3; i++)
                    {
                        sum += matrix[y + 2, x + i];
                    }

                    if (sum > maxSum)
                    {
                        maxSum = sum;
                    }
                }
            }

            return maxSum;
        }

        #endregion

        #region InputOutput

        private static string[] ReadStringLine()
        {
            var read = Console.ReadLine();
            if (read != null)
            {
                var readTokens = read.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
                return readTokens.Length == 0 ? new[] { "0" } : readTokens;
            }

            return new[] { "0" };
        }

        #endregion

    }

}