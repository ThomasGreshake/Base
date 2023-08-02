//-------------------------------------------------
// Copyright Thomas Greshake 2023
//-------------------------------------------------


using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

namespace MyFuncs
{
    //A collection of various random functions. Uses Unitys Random.

    
    public static class MyRandom
    {
        //Randomizes the contents of a list. Leaves the original list intact!!!
        public static List<T> RandomizeList<T>(List<T> list)
        {
            List<T> oldList = new List<T>(list);
            List<T> randomList = new List<T>(list.Count);
            while (oldList.Count > 0)
            {
                int i = UnityEngine.Random.Range(0, oldList.Count);
                randomList.Add(oldList[i]);
                oldList.RemoveAt(i);
            }
            return randomList;
        }

        //Randomizes the contents of a list. Changes the original itself!!!
        public static List<T> Randomize<T>(this List<T> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                int j = UnityEngine.Random.Range(i, list.Count);
                T item = list[j];
                list.RemoveAt(j);
                list.Insert(i, item);
            }
            return list;
        }

        //Randomized integers from 0 to count - 1.
        public static List<int> GetRandomizedIntegers(int count)
        {
            List<int> list = new List<int>(count);
            for (int i = 0; i < count; i++)
            {
                list.Add(i);
            }
            return list.Randomize();
        }

        //Randomly rounds up or down with a chance based on how close the float is to eahc neighbouring integer
        public static int RandomRound(float f)
        {
            int i = Mathf.FloorToInt(f);
            return UnityEngine.Random.Range(0f, 1f) < f - i ? i + 1 : i;
        }

        //Returns a weighted random item from a given list.
        //Every item uses its list position (first => 1, second => 2) as weight.
        public static T GetRandomWeightedItem<T>(IList<T> list)
        {
            return GetRandomItemUsingWeightFunction(list, (int i) => { return i + 1; });
        }

        //Returns a weighted random item from the given list.
        //Uses a linear weight function.
        public static T GetRandomItemUsingLinearWeightFunction<T>(IList<T> list, float weightIncrease)
        {
            return GetRandomItemUsingWeightFunction<T>(list, (int i) => { return i * weightIncrease + 1; });
        }

        //Returns a weighted random item from the given list.
        //Uses a given weight function.
        public static T GetRandomItemUsingWeightFunction<T>(IList<T> list, Func<int, float> func)
        {
            if (list.Count == 0)
            {
                return default(T);
            }

            float[] weights = new float[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                weights[i] = func(i);
            }
            return GetRandomItemUsingWeightList<T>(list, weights);
        }
        public static T GetRandomItemUsingWeightFunction<T>(IList<T> list, Func<T, float> func)
        {
            if (list.Count == 0)
            {
                return default(T);
            }

            float[] weights = new float[list.Count];
            for (int i = 0; i < list.Count; i++)
            {
                weights[i] = func(list[i]);
            }
            return GetRandomItemUsingWeightList<T>(list, weights);
        }

        //Returns a weighted random item from the given list.
        //Weights can be given by a list. Negative weights are set to 0. Items without a weight have weight 0.
        public static T GetRandomItemUsingWeightList<T>(IList<T> list, IList<float> weights)
        {
            if (list.Count == 0)
            {
                return default(T);
            }
            if (weights.Count == 0)
            {
                return list.RandomItem(); //If there are no weights, they are all the same?
            }

            int end = Mathf.Min(list.Count, weights.Count);
            float sum = weights.Sum(f => Mathf.Max(0, f));
            if (sum == 0)
            {
                return list.RandomItem(); //If there are no weights, they are all the same?
            }

            float random = UnityEngine.Random.Range(0, sum);
            sum = 0;
            for (int i = 0; i < end; i++)
            {
                sum += Mathf.Max(weights[i], 0);

                if (random < sum)
                {
                    return list[i];
                }
            }

            //Something went wrong
            return list.RandomItem(); ;
        }

        //Returns a random List item
        public static T RandomItem<T>(this IList<T> list)
        {
            if (list.Count == 0)
            {
                return default(T);
            }

            return list[UnityEngine.Random.Range(0, list.Count)];
        }
    }
}

