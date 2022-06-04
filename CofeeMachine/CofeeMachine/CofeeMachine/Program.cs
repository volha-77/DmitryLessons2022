using System;
using System.Collections.Generic;

namespace CofeeMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            var espressoIngredients = new Dictionary<Ingredient, double>
                {
                {Ingredient.water, 30},
                {Ingredient.cofee, 15},
                {Ingredient.sugar, 0},
                };

            Drink cappucino = new Drink("Cappuccino");

            Drink americano = new Drink("Americano");
            Drink espresso = new Drink("Espresso", espressoIngredients);

            List<Drink> drinks = new List<Drink>
            {
               cappucino,
               americano,
               espresso
            };
            CofeeMachine cofeeMachine = new CofeeMachine(drinks);
            cofeeMachine.AddIngredient(Ingredient.cofee, 1000);
            cofeeMachine.AddIngredient(Ingredient.water, 20000);
           // cofeeMachine.AddIngredient(Ingredient.milk, 20000);
            cofeeMachine.AddIngredient(Ingredient.sugar, 2000);

            var esp = cofeeMachine.MakeDrink(espresso);

        }
    }

    public class CofeeMachine: ICofeeMachine
    {
        public List<Drink> AvailableDrinks { get; }
        public IDictionary<Ingredient, double> AvailableIngredients { get; }
        public IDictionary<Ingredient, double> SpentIngredients { get; }
        public CofeeMachine(List<Drink> drinks)
        {
            AvailableDrinks = drinks;
            AvailableIngredients = new Dictionary<Ingredient, double> { {Ingredient.sugar, 0 } };
            SpentIngredients = new Dictionary<Ingredient, double> { { Ingredient.sugar, 0 } };

            foreach (var drink in AvailableDrinks)
            {
                if (drink.Ingredients != null)
                foreach (var portion in drink.Ingredients)
                {
                    if (!AvailableIngredients.ContainsKey(portion.Key))
                        AvailableIngredients.Add(portion.Key, 0);
                    if (!SpentIngredients.ContainsKey(portion.Key))
                        SpentIngredients.Add(portion.Key, 0);
                }

            }
        }
        public void AddIngredient(Ingredient ingredient, double quatity)
        {
            if (AvailableIngredients.ContainsKey(ingredient))
            {
                AvailableIngredients[ingredient] += quatity;
            }
            else
               throw new InvalidOperationException("wrong ingredient!!!");
        }
        public CupOfDrink MakeDrink(Drink drink)
        {
            if (AvailableDrinks.Contains(drink) && drink.Ingredients != null)
            {
                //check if all drink ingredietns are in machine 
                foreach (var portion in drink.Ingredients)
                {
                    if (AvailableIngredients.TryGetValue(portion.Key, out double quantity))
                    {
                        if (portion.Value > quantity) throw new InvalidOperationException($"[{portion.Key} is out]");
                    }
                    else throw new InvalidOperationException($"[{portion.Key} is wrong ingredient]");
                }

                //update state of the machine 
                foreach (var portion in drink.Ingredients)
                {
                    AvailableIngredients[portion.Key] -= portion.Value;
                    SpentIngredients[portion.Key] += portion.Value;
                }

                Console.WriteLine($"Your {drink.Name} is ready");

                return new CupOfDrink(drink);
            }
            else throw new InvalidOperationException();
        }
    }

    public class Drink
    {
        public string Name { get; }

        public IDictionary<Ingredient, double> Ingredients;

        public Drink(string name)
        {
            Name = name;
        }
        public Drink(string name, IDictionary<Ingredient, double> ingredients) : this(name)
        {
            Ingredients = ingredients;
        }
    }

    public class CupOfDrink
    {
        private Drink _drink { get; }

        public double Price { get; set; }

        public CupOfDrink(Drink drink)
        {
            _drink = drink;
        }
    }


    public interface ICofeeMachine
    {
        public List<Drink> AvailableDrinks { get; }
        public IDictionary<Ingredient, double> AvailableIngredients { get; }
        public IDictionary<Ingredient, double> SpentIngredients { get; }

        public void AddIngredient(Ingredient ingredient, double quatity);
        public CupOfDrink MakeDrink(Drink drink);
    }


    public enum Ingredient
    {
        milk,
        water,
        cofee,
        sugar
    }

}
