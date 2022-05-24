using System;
using System.Collections.Generic;

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

            var listResult = FindDifference(first, second);
            listResult.Sort();

            string stringResult = ("[");
            //foreach (var item in listResult)
            //{
            //    if (stringResult.Length > 1) stringResult = stringResult + ",";
            //    stringResult = stringResult + item.ToString();
            //}

            stringResult = $"[{string.Join(",", listResult)}]";

            //stringResult = stringResult + "]";


            Console.WriteLine(stringResult);

            Console.ReadKey();

        }

        static List<int> FindDifference(ICollection<int> firstCollection, ICollection<int> secondCollection)
        {
            List<int> listResult = new List<int>();

            CompaireOneCollectionToAnother(firstCollection, secondCollection, listResult);

            CompaireOneCollectionToAnother(secondCollection, firstCollection, listResult);

            return listResult;
        }

        static void CompaireOneCollectionToAnother(ICollection<int> firstCollection, ICollection<int> secondCollection, List<int> listResult)
        {

            foreach (var itemFirst in firstCollection)
            {
                bool isFound = false;
                foreach (var itemSecond in secondCollection)
                {
                    if (itemFirst == itemSecond) isFound = true;
                }

                if ((!isFound) && (!listResult.Contains(itemFirst))) listResult.Add(itemFirst);
            }


        }
    }
}
