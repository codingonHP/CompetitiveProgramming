using System;

namespace CompetitiveProgramming
{
    public static class ChefRout
    {
        public static string Activity(string activityString)
        {
            bool validState = false;
            bool cooked = false;
            bool eat = false;

            for (int i = 0; i < activityString.Length; i++)
            {

                if (activityString[i] == 'C')
                {
                    if (i + 1 != activityString.Length)
                    {
                        var nextActivity = activityString[i + 1];
                        if (nextActivity == 'C' || nextActivity == 'E' || nextActivity == 'S')
                        {
                            cooked = true;
                            validState = true;
                        }
                        else
                        {
                            validState = false;
                        }

                    }
                }

                else if (cooked && activityString[i] == 'E')
                {
                    if (i + 1 != activityString.Length)
                    {
                        var nextActivity = activityString[i + 1];
                        if (nextActivity == 'E' || nextActivity == 'S')
                        {
                            eat = true;
                        }
                        else
                        {
                            validState = false;
                        }
                    }
                }

                else if (activityString[i] == 'S')
                {
                    if (i + 1 != activityString.Length)
                    {
                        var nextActivity = activityString[i + 1];
                        if (cooked && eat && nextActivity == 'C' || nextActivity == 'E' || nextActivity == 'S')
                        {

                        }
                        else
                        {
                            validState = false;
                        }
                    }
                }

                if (!validState)
                {
                    break;
                }
            }

            return validState ? "yes" : "no";

        }
    }
}
