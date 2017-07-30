//Solution to the problem belongs to me
using System;
using System.Linq;

namespace CpForCompetitiveProgrammingLcShortestUnsortedContinuousSubarray
{
    public static class LcShortestUnsortedContinuousSubarray
    {
        #region Main

        public static void Main_Solver(string[] args)
        //public static void Main(string[] args)
        {
            var array = Array.ConvertAll(Console.ReadLine().Split(' ').ToArray(), int.Parse);
            var len = Solve(array);

            Console.WriteLine(len);
        }

        #endregion

        #region Solution

        public static int Solve(int[] array)
        {
            int prev = array[0];
            int minIndex = 0;
            bool inc = true;

            for (int i = 1; i < array.Length; i++)
            {
                if (prev > array[i])
                {
                    inc = false;
                    break;
                }

                minIndex = i;
                prev = array[i];
            }

            if (inc)
            {
                return 0;
            }

            var next = array[array.Length - 1];
            int maxIndex = array.Length - 1;

            for (int i = array.Length - 2; i >= 0; i--)
            {
               
                if (next < array[i])
                {
                    break;
                }

                maxIndex = i;
                next = array[i];

            }

            var minValue = (int)1e9;
            var maxValue = (int)-1e9;

            for (int i = minIndex; i <= maxIndex; i++)
            {
                if (array[i] > maxValue)
                {
                    maxValue = array[i];
                }

                if (array[i] < minValue)
                {
                    minValue = array[i];
                }
            }

            int dl = 0, dr = 0;
            int l = minIndex - 1, r = maxIndex + 1;

            while (l >= 0 && array[l] > minValue)
            {
                ++dl;
                --l;
            }

            while (r < array.Length && array[r] < maxValue)
            {
                ++dr;
                ++r;
            }

            return maxIndex - minIndex + 1 + dl + dr;
        }

        #endregion
    }
}
