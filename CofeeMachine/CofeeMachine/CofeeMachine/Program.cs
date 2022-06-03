using System;
using System.Collections.Generic;

namespace CofeeMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            List<DrinkType> drinkTypes = new List<DrinkType>
            {
                DrinkType.cappucino,
                DrinkType.espesso,
                DrinkType.americano
            };
            CofeeMachine cofeeMachine = new CofeeMachine(drinkTypes);
            var capuc = cofeeMachine.MakeDrink(DrinkType.cappucino);

            if (capuc == null) throw new InvalidOperationException();
        }
    }

    public class CofeeMachine: ICofeeMachine
    {
        // public List<DrinkType> AvailableDrinkTypes { get; set; }

        public List<Drink> AvailableDrinks { get; set; }

        public void AddMilk(double milk)
        {

        }

        private IDictionary<Ingredient, double> availableIngredients { get; set; }

        public IDictionary<Ingredient, double> SpentIngredients { get; }

        public CofeeMachine(List<DrinkType> drinkTypes)
        {
            AvailableDrinkTypes = drinkTypes;
        }

        public Drink MakeDrink(DrinkType drinkType)
        {
            if (AvailableDrinkTypes.Contains(drinkType))
            {
                Console.WriteLine($"Your {drinkType.ToString()} is ready");

                return new Drink(drinkType);
            }
            else return null;
        }
    }

    public class Ingredient
    {
        public string Name { get; set; }
        public double Price { get; set; }
        public Units Unit { get; set; }

    }

    public enum Units
    {
        liter,
        kg,
    }

    public class Drink
    {
        public string Name { get; }

        public double Volume { get; set; }

        public IDictionary<Ingredient, double> Ingredients;
        public double Price { get; set; }

        public Drink(string name)
        {
            Name = name;
        }
    }

    public interface ICofeeMachine
    {
        List<DrinkType> AvailableDrinkTypes { get; set; }

        IDictionary<Ingredient, double> AvailableIngredients { get; set; }

        IDictionary<Ingredient, double> SpentIngredients { get; }

        public Drink MakeDrink(DrinkType drinkType);
    }

    public enum DrinkType
    {
        espesso,
        cappucino,
        americano,
        glace
    }
}
