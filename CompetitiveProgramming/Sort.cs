using System;

namespace CompetitiveProgramming
{
    public static partial class Sort
    {
        public static void MergeSort(int[] array, bool asc)
        {
            Divide(array, 0, array.Length - 1, asc);
        }

        private static void Divide(int[] array, int low, int high, bool asc)
        {
            if (low >= high)
            {
                return;
            }

            int mid = (low + high) / 2;
            Divide(array, low, mid, asc);
            Divide(array, mid + 1, high, asc);

            Merge(array, low, mid, high, asc);

        }

        private static void Merge(int[] arry, int low, int mid, int high, bool asc)
        {
            int[] temp = new int[high - low + 1];
            int left = low, right = mid + 1;
            int k = -1;

            while (left <= mid && right <= high)
            {
                if (asc)
                {
                    if (arry[left] < arry[right])
                    {
                        temp[++k] = arry[left];
                        left++;
                    }
                    else
                    {
                        temp[++k] = arry[right];
                        right++;
                    }
                }
                else
                {
                    if (arry[left] > arry[right])
                    {
                        temp[++k] = arry[left];
                        left++;
                    }
                    else
                    {
                        temp[++k] = arry[right];
                        right++;
                    }
                }

            }

            if (left <= mid)
            {
                while (left <= mid)
                {
                    temp[++k] = arry[left];
                    ++left;
                }
            }

            if (right <= high)
            {
                while (right <= high)
                {
                    temp[++k] = arry[right];
                    ++right;
                }
            }

            k = -1;
            for (int i = low; i <= high; i++)
            {
                arry[i] = temp[++k];
            }
        }
    }

    public static partial class Sort
    {
        public static void Hello()
        {
            var x = Array.ConvertAll("1 2 3 4".Split(' '), int.Parse );
        }
    }
}