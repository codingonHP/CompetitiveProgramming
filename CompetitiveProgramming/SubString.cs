using System;
using System.Collections.Generic;

namespace CompetitiveProgramming
{
    public class SubString
    {
        public bool IsSubString(string mainString, string targetString)
        {
            if (targetString.Length > mainString.Length)
            {
                return false;
            }

            Dictionary<char, LinkedList<int>> map = new Dictionary<char, LinkedList<int>>();

            for (var index = 0; index < mainString.Length; index++)
            {
                char c = mainString[index];
                if (!map.ContainsKey(c))
                {
                    var ll = new LinkedList<int>();
                    ll.AddFirst(index);

                    map.Add(c, ll);
                }
                else
                {
                    var ll = map[c];
                    ll.AddLast(index);
                }
            }

            var s = targetString[0];
            if (map.ContainsKey(s))
            {
                var list = map[s];

                while (list.Count > 0)
                {
                    int startAtIndex = list.First.Value;
                    list.RemoveFirst();

                    int index = 0;
                    for (int i = startAtIndex; i < mainString.Length; i++)
                    {
                        if (targetString[index++] != mainString[i])
                        {
                            break;
                        }

                        if (index == targetString.Length)
                        {
                            return true;
                        }
                    }
                }
            }
            else
            {
                return false;
            }

            return false;
        }

        public bool IsSubStringUsingRabinKarp(string mainString, string targetString)
        {
            if (targetString.Length > mainString.Length)
            {
                return false;
            }

            var targetHash = GetStringHash(targetString, 0, targetString.Length);

            for (int i = 0; i < mainString.Length; i++)
            {
                if (i + targetString.Length > mainString.Length)
                {
                    return false;
                }

                var hash = GetStringHash(mainString, i, targetString.Length);
                if (hash == targetHash)
                {
                    bool matched = true;
                    var subIndex = i;
                    foreach (char targetChar in targetString)
                    {
                        if (targetChar != mainString[subIndex++])
                        {
                            matched = false;
                            break;
                        }
                    }

                    if (matched)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public long GetStringHash(string str, int startIndex, int length)
        {
            if (startIndex + length > str.Length)
            {
                return -1;
            }

            long hash = 0;
            int pow = 0;
            int i = startIndex;
            int e = length;

            while (e > 0)
            {
                hash += str[i++] * (long)Math.Pow(101, pow++);
                --e;
            }

            return hash % 999999999999999999;
        }
    }
}
