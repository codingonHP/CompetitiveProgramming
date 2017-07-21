using System;
using System.Collections.Generic;
using System.Linq;

namespace CompetitiveProgramming
{
    public class Node
    {
        public int X { get; set; }
        public int Y { get; set; }
        public string Value { get; set; }
        public List<Node> ReachableNodes = new List<Node>();
    }

    public class Maze
    {
        private List<Node> Blocks { get; }
        private int MinimumPathDistance { get; set; } = -1;

        public Maze(string[,] matrix)
        {
            Blocks = ConvertToBlocks(matrix);
        }

        public int Travel(int x1, int y1, int x2, int y2)
        {
            var init = Blocks.FirstOrDefault(b => b.X == x1 && b.Y == y1);
            var final = Blocks.FirstOrDefault(b => b.X == x2 && b.Y == y2);

            if (init != null && final != null)
            {
                Stack<Node> journey = new Stack<Node>();
                journey.Push(init);

                TravelTo(init, final, journey);
            }

            return MinimumPathDistance;
        }

        private void TravelTo(Node init, Node final, Stack<Node> journey)
        {

            if (init != null && init.X == final.X && init.Y == final.Y)
            {
                PrintStack(journey);
                CaculateDistanceTravelled(journey, final.X, final.Y);
                journey.Pop();
                return;
            }

            if (init != null && init.ReachableNodes.Any())
            {
                foreach (var next in init.ReachableNodes)
                {
                    if (next.X == final.X && next.Y == final.Y)
                    {
                        PrintStack(journey);
                        CaculateDistanceTravelled(journey, final.X, final.Y);
                        journey.Pop();
                        return;
                    }

                    if (!journey.Contains(next))
                    {
                        journey.Push(next);
                        TravelTo(next, final, journey);
                    }
                }
            }

            journey.Pop();
        }

        private List<Node> ConvertToBlocks(string[,] matrix)
        {
            List<Node> blocks = new List<Node>();
            var xLength = matrix.GetLength(0);
            var yLength = matrix.GetLength(1);

            for (int x = 0; x < xLength; x++)
            {
                for (int y = 0; y < yLength; y++)
                {
                    Node block = new Node
                    {
                        X = x,
                        Y = y,
                        Value = matrix[x, y]
                    };

                    blocks.Add(block);
                }
            }

            for (int x = 0; x < xLength; x++)
            {
                for (var y = 0; y < yLength; y++)
                {
                    var block = blocks.FirstOrDefault(b => b.X == x && b.Y == y);

                    if (block != null && CanGo(matrix, x, y - 1))
                    {
                        var leftBlock = blocks.FirstOrDefault(b => b.X == x && b.Y == y - 1);
                        if (leftBlock != null)
                        {
                            block.ReachableNodes.Add(leftBlock);
                        }
                    }

                    if (block != null && CanGo(matrix, x, y + 1))
                    {
                        var rightBlock = blocks.FirstOrDefault(b => b.X == x && b.Y == y + 1);
                        if (rightBlock != null)
                        {
                            block.ReachableNodes.Add(rightBlock);
                        }
                    }

                    if (block != null && CanGo(matrix, x - 1, y))
                    {
                        var upBlock = blocks.FirstOrDefault(b => b.X == x - 1 && b.Y == y);
                        if (upBlock != null)
                        {
                            block.ReachableNodes.Add(upBlock);
                        }
                    }

                    if (block != null && CanGo(matrix, x + 1, y))
                    {
                        var downBlock = blocks.FirstOrDefault(b => b.X == x + 1 && b.Y == y);
                        if (downBlock != null)
                        {
                            block.ReachableNodes.Add(downBlock);
                        }
                    }

                    //Cross movement
                    if (block != null && CanGo(matrix, x + 1, y + 1))
                    {
                        var downBlock = blocks.FirstOrDefault(b => b.X == x + 1 && b.Y == y + 1);
                        if (downBlock != null)
                        {
                            block.ReachableNodes.Add(downBlock);
                        }
                    }

                    if (block != null && CanGo(matrix, x - 1, y - 1))
                    {
                        var downBlock = blocks.FirstOrDefault(b => b.X == x - 1 && b.Y == y - 1);
                        if (downBlock != null)
                        {
                            block.ReachableNodes.Add(downBlock);
                        }
                    }

                    if (block != null && CanGo(matrix, x + 1, y - 1))
                    {
                        var downBlock = blocks.FirstOrDefault(b => b.X == x + 1 && b.Y == y - 1);
                        if (downBlock != null)
                        {
                            block.ReachableNodes.Add(downBlock);
                        }
                    }

                    if (block != null && CanGo(matrix, x - 1, y + 1))
                    {
                        var downBlock = blocks.FirstOrDefault(b => b.X == x - 1 && b.Y == y + 1);
                        if (downBlock != null)
                        {
                            block.ReachableNodes.Add(downBlock);
                        }
                    }
                }
            }

            return blocks;
        }

        private static bool CanGo(string[,] matrix, int x1, int y1)
        {
            int xBound = matrix.GetLength(0);
            int yBound = matrix.GetLength(1);

            return x1 >= 0 && x1 < xBound && y1 >= 0 && y1 < yBound && matrix[x1, y1] == ".";
        }

        private void PrintStack(Stack<Node> stack)
        {
            Stack<Node> temp = new Stack<Node>();
            foreach (var s in stack)
            {
                temp.Push(s);
            }

            Console.WriteLine();
            foreach (var s in temp)
            {
                Console.Write($"({s.X},{s.Y}) -> ");
            }
            Console.WriteLine();
        }

        private void CaculateDistanceTravelled(Stack<Node> stack, int x2, int y2)
        {
            int distance = 0;
            stack.Push(new Node
            {
                X = x2,
                Y = y2
            });

            Stack<Node> temp = new Stack<Node>();
            foreach (var s in stack)
            {
                temp.Push(s);
            }

            stack.Pop();

            Node first = null;
            Node second = null;
            Node third = null;

            if (temp.Count > 0)
            {
                first = temp.Pop();

                if (temp.Count > 0)
                {
                    second = temp.Pop();

                    if (temp.Count > 0)
                    {
                        third = temp.Pop();
                    }
                }
            }

            do
            {
                if (first != null && second != null && third != null)
                {
                    if (first.X == second.X && second.X == third.X)
                    {
                        if (temp.Count > 0)
                        {
                            first = second;
                            second = third;
                            third = temp.Pop();
                        }
                        else
                        {
                            first = null;
                            ++distance;
                        }
                        continue;
                    }

                    if (first.Y == second.Y && second.Y == third.Y)
                    {
                        if (temp.Count > 0)
                        {
                            first = second;
                            second = third;
                            third = temp.Pop();
                        }
                        else
                        {
                            first = null;
                            ++distance;
                        }
                        continue;
                    }

                    if (first.X == first.Y && second.X == second.Y && third.X == third.Y)
                    {
                        if (temp.Count > 0)
                        {
                            first = second;
                            second = third;
                            third = temp.Pop();
                        }
                        else
                        {
                            first = null;
                            ++distance;
                        }

                        continue;
                    }

                    distance++;
                    if (temp.Count > 0)
                    {
                        first = second;
                        second = third;
                        third = temp.Pop();
                    }
                    else
                    {
                        first = null;
                        ++distance;
                    }
                }
                else
                {
                    ++distance;
                }

            } while (temp.Count > 0 || (first != null && second != null && third != null));

            if (MinimumPathDistance > distance || MinimumPathDistance < 0)
            {
                MinimumPathDistance = distance;
            }
        }
    }
}
