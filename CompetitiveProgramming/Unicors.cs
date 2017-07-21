using System.Collections.Generic;

namespace CompetitiveProgramming
{
    public static class Unicors
    {
        public static int GetMaxCount(int[] courseArry)
        {
            int totalCount = courseArry.Length;
            int usedCount = 0;

            foreach (int dep in courseArry)
            {
                if (dep > usedCount)
                {
                    usedCount = usedCount + (dep - usedCount);
                }
            }

            return totalCount - usedCount;
        }
    }
}
