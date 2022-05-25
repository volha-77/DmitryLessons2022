using System;
using System.Collections.Generic;
using System.Linq;

namespace intLists
{
    class IntListsDifferenceSolution
    {
        static void Main(string[] args)
        {
            int[] first = { 1, 3, 3, 4, 6, 5, 4 };
            int[] second = { 6, 3, 5, 2, 2 };

            //main task
            //Implement a logic that finds difference between "first" and "secord" lists
            // and prints the result to the console:
            // [1, 2, 4]
            //enhanced task
            //** try to come up with solution wich doesn't use set data structure

            int[] result = FindDifference(first, second);
            Array.Sort(result);

            string stringResult = $"[{string.Join(",", result)}]";

            Console.WriteLine(stringResult);

            Console.ReadKey();

        }

        static int[] FindDifference(int[] firstCollection, int[] secondCollection)
        {
            int[] firstResult = CompaireOneCollectionToAnother(firstCollection, secondCollection);

            int[] secondResult = CompaireOneCollectionToAnother(secondCollection, firstCollection);

            int[] result = new int[firstResult.Length + secondResult.Length];

            firstResult.CopyTo(result, 0);
            secondResult.CopyTo(result, firstResult.Length);

            return result;
        }

        static int[] CompaireOneCollectionToAnother(int[] firstCollection, int[] secondCollection)
        {
            List<int> listResult = new List<int>();

            HashSet<int> searchForSecond = new HashSet<int>();

            foreach (var itemSecond in secondCollection)
            {
                searchForSecond.Add(itemSecond);
            }

            foreach (var itemFirst in firstCollection.Where(itemFirst => !searchForSecond.Contains(itemFirst)))
            {
                listResult.Add(itemFirst);
                searchForSecond.Add(itemFirst);
            }

            return listResult.ToArray();

        }
    }
}
