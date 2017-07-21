using System;
using System.Collections.Generic;
using System.Linq;

namespace CompetitiveProgramming
{
    public static class Anagram
    {
        public static string Sort(string s)
        {
            return new string(s.ToUpper().OrderBy(c => c).ToArray());
        }

        public static bool IsAnagram(string a, string b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            var oa = BubbleSort(a);
            var ob = BubbleSort(b);

            return oa == ob;
        }

        public static bool IsAnagram2(string a, string b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            a = a.ToUpper();
            b = b.ToUpper();

            Dictionary<char, int> dict = new Dictionary<char, int>();

            foreach (var c in a)
            {
                if (dict.ContainsKey(c))
                {
                    dict[c]++;
                }
                else
                {
                    dict.Add(c, 1);
                }
            }

            foreach (var c in b)
            {
                int count;
                if (dict.TryGetValue(c, out count))
                {
                    if (--dict[c] == 0)
                    {
                        dict.Remove(c);
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsAnagram3(string a, string b)
        {
            if (a.Length != b.Length)
            {
                return false;
            }

            var aHash = GetStringWeight(a);
            var bHash = GetStringWeight(b);

            return aHash == bHash;
        }

        public static long GetStringWeight(string str)
        {
            var hash = 0;
            foreach (char t in str.ToUpper())
            {
                hash += t;
            }

            return hash;
        }

        public static string SortSelection(string input)
        {
            char[] s = input.ToCharArray();

            for (int i = 0; i < s.Length - 1; i++)
            {
                var min = i;
                for (int j = i + 1; j < s.Length; j++)
                {
                    if (s[min] > s[j])
                    {
                        min = j;
                    }
                }

                var temp = s[i];
                s[i] = s[min];
                s[min] = temp;
            }

            return new string(s);
        }

        public static string BubbleSort(string input)
        {
            char[] arry = input.ToCharArray();

            for (int i = arry.Length - 1; i > 0; i--)
            {
                for (int j = 0; j < i; j++)
                {
                    if (arry[j] > arry[j + 1])
                    {
                        var temp = arry[j];
                        arry[j] = arry[j + 1];
                        arry[j + 1] = temp;
                    }
                }
            }

            return new string(arry);
        }

        public static string MergeSort(string input)
        {
            var arry = input.ToCharArray();
            Divide(arry, 0, arry.Length - 1);

            return new string(arry);
        }

        private static void Divide(char[] arry, int low, int high)
        {
            if (low >= high)
            {
                return;
            }

            int mid = (low + high) / 2;
            Divide(arry, low, mid);
            Divide(arry, mid + 1, high);

            Merge(arry, low, mid, high);
        }

        private static void Merge(char[] arry, int low, int mid, int high)
        {
            char[] temp = new char[high - low + 1];

            int leftIndex = low, rightIndex = mid + 1, tempIndex = 0;

            while (leftIndex <= mid && rightIndex <= high)
            {
                if (arry[leftIndex] < arry[rightIndex])
                {
                    temp[tempIndex++] = arry[leftIndex++];
                }
                else
                {
                    temp[tempIndex++] = arry[rightIndex++];
                }
            }

            while (leftIndex <= mid)
            {
                temp[tempIndex++] = arry[leftIndex++];
            }

            while (rightIndex <= high)
            {
                temp[tempIndex++] = arry[rightIndex++];
            }

            tempIndex = 0;
            for (int i = low; i <= high; i++)
            {
                arry[i] = temp[tempIndex++];
            }
        }


        public static string QuickSort(string input)
        {
            var arry = input.ToCharArray();
            QSort(arry, 0, arry.Length - 1);

            return new string(arry);
        }

        private static void QSort(char[] arry, int low, int high)
        {
            if (low <= high)
            {
                int p = Partition(arry, low, high);

                QSort(arry, low, p - 1);
                QSort(arry, p + 1, high);
            }
        }

        private static int Partition(char[] arry, int low, int high)
        {
            var pivot = arry[low + (high - low) % new Random(1).Next()];
            int start = low - 1; //partition is initially before the start

            for (int i = low; i <= high - 1; i++)
            {
                if (arry[i] <= pivot) //ASC
                {
                    ++start;
                    Swap(arry, start, i);
                }
            }

            Swap(arry, start + 1, high);
            return start + 1;
        }

        private static void Swap(char[] arry, int a, int b)
        {
            var temp = arry[a];
            arry[a] = arry[b];
            arry[b] = temp;
        }
    }
}
