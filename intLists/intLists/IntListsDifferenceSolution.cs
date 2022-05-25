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

            IDictionary<int, bool> searchDictForSecond = new Dictionary<int, bool>();
            foreach (var itemSecond in secondCollection.Where(itemSecond => !searchDictForSecond.ContainsKey(itemSecond)))
            {
                searchDictForSecond.Add(itemSecond, false);
            }

            foreach (var itemFirst in firstCollection.Where(itemFirst => !searchDictForSecond.TryGetValue(itemFirst, out bool isFound)))
            {
                listResult.Add(itemFirst);
                if (!searchDictForSecond.ContainsKey(itemFirst)) searchDictForSecond.Add(itemFirst, true);
            }

            return listResult.ToArray();

        }
    }
}
