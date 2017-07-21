using System;
using System.Collections.Generic;
using System.Linq;

namespace CompetitiveProgramming
{
    public class LittleElephantAndArray
    {
        public static int GetElements(int[] array, int start, int end)
        {
            Dictionary<int, int> bufferDictionary = new Dictionary<int, int>();
            for (int i = start - 1; i < end; i++)
            {
                if (bufferDictionary.ContainsKey(array[i]))
                {
                    bufferDictionary[array[i]]++;
                }
                else
                {
                    bufferDictionary.Add(array[i], 1);
                }
            }

            int count = 0;
            foreach (var keyValuePair in bufferDictionary)
            {
                if (keyValuePair.Key == keyValuePair.Value)
                {
                    ++count;
                }
            }

            return count;
        }

        public static int Count(int[] array, int start, int end)
        {
            Sort(array, start - 1, end - 1);
            int count = 0;
            for (int i = start - 1; i < end; i++)
            {
                if (array[i] + i - 1 >= 0 && array[i] + i - 1 < end
                    && array[i] == array[array[i] + i - 1])
                {
                    if (array[array[i] + i] < end && array[array[i] + i - 1] != array[array[i] + i])
                    {
                        ++count;
                        i += array[i] - 1;
                    }
                    else if (array[array[i] + i] == end)
                    {
                        ++count;
                        i += array[i] - 1;
                    }
                }
            }

            return count;
        }

        public static void Sort(int[] array, int start, int end)
        {
            if (start < end)
            {
                int pivot = Pivot(array, start, end);
                Sort(array, start, pivot - 1);
                Sort(array, pivot + 1, end);
            }
        }

        public static int Pivot(int[] arry, int start, int end)
        {
            int pivot = end;
            int wall = start;

            for (int i = start; i < end; i++)
            {
                if (arry[pivot] > arry[i])
                {
                    Swap(arry, wall, i);
                    ++wall;
                }
            }

            Swap(arry, wall, pivot);

            return wall;
        }

        public static void Swap(int[] arry, int i, int j)
        {
            var temp = arry[i];
            arry[i] = arry[j];
            arry[j] = temp;
        }

    }
}
