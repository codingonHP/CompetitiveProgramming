using System;
using System.Collections.Generic;

namespace CompetitiveProgramming
{
    public static class ChefSuba
    {
        //public static IEnumerable<int> Solve(int[] array, string pattern, int window)
        //{
        //    foreach (var request in pattern)
        //    {
        //        if (request.Equals('?'))
        //        {
        //            yield return Get1Count(array, window);
        //        }
        //        else
        //        {
        //            RotateArry(array);
        //        }
        //    }
        //}

        //public static int Get1Count(int[] arry, int windowSize)
        //{
        //    int count = 0;
        //    int maxCollectedCount = 0;

        //    for (var index = 0; index < arry.Length; index++)
        //    {
        //        int element = arry[index];
        //        if (count == windowSize)
        //        {
        //            return windowSize;
        //        }

        //        if (element == 1 && index < arry.Length - 1 && arry[index + 1] == 1)
        //        {
        //            count += 2;
        //            ++index;
        //        }
        //        else if (element == 1)
        //        {
        //            ++count;
        //        }
        //        else if (maxCollectedCount < count)
        //        {
        //            maxCollectedCount = count;
        //            count = 0;
        //        }
        //    }

        //    return maxCollectedCount > windowSize ? windowSize : (maxCollectedCount > count ? maxCollectedCount : count);
        //}

        //public static int[] RotateArry(int[] arry)
        //{
        //    var last = arry.Length - 1;
        //    for (var i = 0; i < last; i += 1)
        //    {
        //        arry[i] ^= arry[last];
        //        arry[last] ^= arry[i];
        //        arry[i] ^= arry[last];
        //    }

        //    return arry;
        //}

        //public static int Get1Count(int number, int windowSize)
        //{
        //    int count = 0;
        //    bool prevOne = false;
        //    while (number != 0)
        //    {
        //        bool currOne = (number & 1) == 1;
        //        if (currOne && !prevOne)
        //            count++;
        //        number = number >> 1;
        //        prevOne = currOne;
        //    }


        //    return count > windowSize ? windowSize : count;
        //}

        //public static int RotateArry(int number, int length)
        //{
        //    var last = number % 10;
        //    var left = number / 10;
        //    return last * int.Parse(Math.Pow(10, length - 1) + "") + left;

        //}

        //public static int ArryToNumber(int[] arry)
        //{
        //    int number = 0;
        //    foreach (var num in arry)
        //    {
        //        number = 10 * number + num;
        //    }

        //    return number;
        //}


        //public static IEnumerable<int> Solve(int[] array, string pattern, int window)
        //{
        //    foreach (var request in pattern)
        //    {
        //        if (request.Equals('?'))
        //        {
        //            yield return Get1Count(array, window);
        //        }
        //        else
        //        {
        //            RotateArry(array);
        //        }
        //    }
        //}

        //public static int Get1Count(int[] arry, int windowSize)
        //{
        //    int count = 0;
        //    int maxCollectedCount = 0;

        //    for (var index = 0; index < arry.Length; index++)
        //    {
        //        int element = arry[index];
        //        if (count == windowSize)
        //        {
        //            return windowSize;
        //        }

        //        if (element == 1 && index < arry.Length - 1 && arry[index + 1] == 1)
        //        {
        //            count += 2;
        //            ++index;
        //        }
        //        else if (element == 1)
        //        {
        //            ++count;
        //        }
        //        else if (maxCollectedCount < count)
        //        {
        //            maxCollectedCount = count;
        //            count = 0;
        //        }
        //    }

        //    return maxCollectedCount > windowSize ? windowSize : (maxCollectedCount > count ? maxCollectedCount : count);
        //}

        //public static int[] RotateArry(int[] arry)
        //{
        //    var last = arry.Length - 1;
        //    for (var i = 0; i < last; i += 1)
        //    {
        //        arry[i] ^= arry[last];
        //        arry[last] ^= arry[i];
        //        arry[i] ^= arry[last];
        //    }

        //    return arry;
        //}

        //public static int Get1Count(int number, int windowSize)
        //{
        //    int count = 0;
        //    bool prevOne = false;
        //    while (number != 0)
        //    {
        //        bool currOne = (number & 1) == 1;
        //        if (currOne && !prevOne)
        //            count++;
        //        number = number >> 1;
        //        prevOne = currOne;
        //    }


        //    return count > windowSize ? windowSize : count;
        //}

        //public static int RotateArry(int number, int length)
        //{
        //    var last = number % 10;
        //    var left = number / 10;
        //    return last * int.Parse(Math.Pow(10, length - 1) + "") + left;

        //}

        //public static int ArryToNumber(int[] arry)
        //{
        //    int number = 0;
        //    foreach (var num in arry)
        //    {
        //        number = 10 * number + num;
        //    }

        //    return number;
        //}

        public static IEnumerable<int> Solve(string[] number, string pattern, int window)
        {
            for (var index = 0; index < pattern.Length; index++)
            {
                var request = pattern[index];
                if (request.Equals('?'))
                {
                    yield return Get1Count(number, window);
                }
                else
                {
                    int rotateTimes = 1;
                    ++index;

                    while (index < pattern.Length && pattern[index] == '!')
                    {
                        ++rotateTimes;
                        ++index;
                    }

                    number = RotateArry(number, rotateTimes);
                    index -= 1;
                }
            }
        }

        public static int Get1Count(string[] arry, int windowSize)
        {
            int count = 0;
            int maxCollectedCount = 0;

            for (var index = 0; index < arry.Length; index++)
            {
                count = 0;

                while (index < arry.Length && arry[index] == "1")
                {
                    count++;
                    index++;

                    if (count == windowSize)
                    {
                        return windowSize;
                    }
                }

                if (maxCollectedCount < count)
                {
                    maxCollectedCount = count;
                }
            }

            return maxCollectedCount > windowSize ? windowSize : (maxCollectedCount > count ? maxCollectedCount : count);
        }

        public static string[] RotateArry(string[] number, int rotateTimes)
        {
            var position = new int[number.Length];
            for (int i = 0; i < number.Length; i++)
            {
                position[i] = (i + rotateTimes) % number.Length;
            }

            var newArrayState = new string[number.Length];
            for (int i = 0; i < number.Length; i++)
            {
                newArrayState[position[i]] = number[i];
            }

            return newArrayState;
        }


    }
}
