using System.Globalization;

namespace CompetitiveProgramming
{
    public class StringMatchingBruteForce
    {

        public bool StringMatch(string parent, string child)
        {
            var found = false;
            for (int i = 0; i < parent.Length; i++)
            {
                int startIndex = i;
                for (int j = 0; j < child.Length; j++)
                {
                    if (i + j < parent.Length)
                    {
                        if (parent[startIndex++] != child[j])
                        {
                            break;
                        }

                        if (j == child.Length - 1)
                        {
                            found = true;
                        }
                    }
                    else
                    {
                        break;
                    }
                }

                if (found)
                {
                    break;
                }
            }

            return found;
        }
            
    }
}
