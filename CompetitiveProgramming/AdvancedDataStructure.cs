﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace CompetitiveProgramming
{
    public static class AdvancedDataStructure
    {
        public class SegmentTree<T>
        {
            public class SegmentTreeNode
            {
                public T Value { get; set; }
                public long MinRange { get; set; }
                public long MaxRange { get; set; }
                public SegmentTreeNode LeftNode { get; set; }
                public SegmentTreeNode RightNode { get; set; }
            }

            public Dictionary<long, SegmentTreeNode> SegmentDictionary { get; } = new Dictionary<long, SegmentTreeNode>();
            public SegmentTreeNode Root { get; private set; }
            public T[] Array { get; }
            public object DefaultValue { get; set; }

            public Func<T, long, object> BaseValueProvider { get; }
            public Func<SegmentTreeNode, SegmentTreeNode, object> LevelValueProvider { get; }

            public SegmentTree(T[] array, object defaultValue, Func<T, long, object> baseValueProvider, Func<SegmentTreeNode, SegmentTreeNode, object> levelValueProvider)
            {
                Array = array;
                DefaultValue = defaultValue;
                BaseValueProvider = baseValueProvider;
                LevelValueProvider = levelValueProvider;
                CreateTree(baseValueProvider, levelValueProvider);
            }

            public object Query(long l, long r, Func<T, T, object> queryFunc)
            {
                var node = Root;
                var value = Travel(node, l, r, queryFunc);

                return value;
            }

            private void CreateTree(Func<T, long, object> baseValueProvider, Func<SegmentTreeNode, SegmentTreeNode, object> levelValueProvider)
            {
                var baseLength = NextPowerOf2(Array.Length);
                var totalNodes = Math.Pow(2, Math.Log(baseLength, 2) + 1) - 1;
                long leftChildIndex = -1, rightChildIndex = -1;
                long levelLength = baseLength;

                for (int i = 0; i < totalNodes; i++)
                {
                    for (int j = 0; j < levelLength; j++)
                    {

                        SegmentTreeNode left = null, right = null;

                        if (i == baseLength)
                        {
                            leftChildIndex = 0;
                            rightChildIndex = 1;
                        }
                        else if (i > baseLength)
                        {
                            leftChildIndex += 2;
                            rightChildIndex += 2;
                        }
                        else
                        {
                            leftChildIndex = i;
                            rightChildIndex = i;
                        }

                        long minreach;
                        long maxreach;
                        if (i >= baseLength)
                        {
                            left = SegmentDictionary[leftChildIndex];
                            right = SegmentDictionary[rightChildIndex];
                            minreach = left.MinRange;
                            maxreach = right.MaxRange;
                        }
                        else
                        {
                            minreach = maxreach = i;
                        }

                        object value = DefaultValue;

                        if (left != null)
                        {
                            value = levelValueProvider(left, right);
                        }
                        else if (i < Array.Length)
                        {
                            value = baseValueProvider(Array[i], i);
                        }


                        SegmentTreeNode node = new SegmentTreeNode
                        {
                            Value = (T)value,
                            LeftNode = left,
                            RightNode = right,
                            MinRange = minreach,
                            MaxRange = maxreach
                        };

                        SegmentDictionary.Add(i, node);

                        ++i;
                    }

                    levelLength /= 2;
                    --i;
                }

                Root = SegmentDictionary.Last().Value;
            }
           
            private object Travel(SegmentTreeNode node, long l, long r, Func<T, T, object> queryFunc)
            {
                if (node == null)
                {
                    return DefaultValue;
                }

                var nodeMinRange = node.MinRange;
                var nodeMaxRange = node.MaxRange;

                var state = RangeAssessment(l, r, nodeMinRange, nodeMaxRange);

                if (state == -1)
                {
                    return DefaultValue;
                }

                if (state == 1)
                {
                    return node.Value;
                }

                var lvalue = Travel(node.LeftNode, l, r, queryFunc);
                var rvalue = Travel(node.RightNode, l, r, queryFunc);

                return queryFunc(lvalue, rvalue);
            }

            private long RangeAssessment(long l, long r, long nodeMinRange, long nodeMaxRange)
            {
                if (nodeMinRange >= l && nodeMaxRange <= r)
                {
                    return 1; //in range, return from here.
                }

                if (nodeMaxRange < l || nodeMinRange > r)
                {
                    return -1; //out of range, ignore.
                }

                return 0; // go both sides.
            }

            private long NextPowerOf2(long n)
            {
                var p = Math.Ceiling(Math.Log(n, 2));
                return (long)Math.Pow(2, p);
            }
        }

    }
}
