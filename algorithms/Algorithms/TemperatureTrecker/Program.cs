using System;
using System.Collections.Generic;
using System.Linq;

namespace TemperatureTrecker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }

    public class TempTrecker
    {
        List<int> Values;

        public TempTrecker(IEnumerable<int> values = null)
        {
            if (values == null) Values = new List<int>(); else Values = values.ToList();
        }

        public void Insert(int value)
        {
            Values.Add(value);
        }

        public int? GetMax()
        {
            return Values.Max();
        }

        public int? GetMin()
        {
            return Values.Min();
        }

        public double? GetMean()
        {
            return Values.Average();
        }
    }
}
