using System;
using System.Collections.Generic;
using System.Linq;

namespace CofeeMachine
{
    class Program
    {
        static void Main(string[] args)
        {
            var espressoIngredients = new Dictionary<Ingredient, double>
                {
                {Ingredient.Water, 30},
                {Ingredient.Cofee, 15},
                {Ingredient.Sugar, 0},
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

            var availableIngredients = new Dictionary<Ingredient, Tank>
            {
                {Ingredient.Cofee, new Tank{max = 5000} },
                {Ingredient.Water, new Tank{max = 25000} },
                {Ingredient.Sugar, new Tank{max = 7000} }
            };

            CofeeMachine cofeeMachine = new CofeeMachine(drinks, availableIngredients);
            cofeeMachine.AddIngredient(Ingredient.Cofee, 1000);
            cofeeMachine.AddIngredient(Ingredient.Water, 20000);
            // cofeeMachine.AddIngredient(Ingredient.Milk, 20000);
            cofeeMachine.AddIngredient(Ingredient.Sugar, 2000);

            var esp = cofeeMachine.MakeDrink(espresso);

        }
    }

    public class CofeeMachine : ICofeeMachine
    {
        public List<Drink> AvailableDrinks { get; }
        public IDictionary<Ingredient, Tank> AvailableIngredients { get; }

        public IDictionary<Ingredient, double> SpentIngredients { get; }
        public CofeeMachine(IList<Drink> drinks)
        {
            AvailableDrinks = drinks.ToList();

            SpentIngredients = new Dictionary<Ingredient, double> { { Ingredient.Sugar, 0 } };
            AvailableIngredients = new Dictionary<Ingredient, Tank> { { Ingredient.Sugar, new Tank { max = 10000 } } };
            foreach (var portion in AvailableDrinks.Where(drink => drink.Ingredients != null).SelectMany(drink => drink.Ingredients))
            {
                if (!AvailableIngredients.ContainsKey(portion.Key))
                    AvailableIngredients.Add(portion.Key, new Tank { max = 10000 });
                if (!SpentIngredients.ContainsKey(portion.Key))
                    SpentIngredients.Add(portion.Key, 0);
            }

            //foreach (var drink in AvailableDrinks)
            //{
            //    if (drink.Ingredients != null)
            //        foreach (var portion in drink.Ingredients)
            //        {
            //            if (!AvailableIngredients.ContainsKey(portion.Key))
            //                AvailableIngredients.Add(portion.Key, new Tank { max = 10000 });

            //            if (!SpentIngredients.ContainsKey(portion.Key))
            //                SpentIngredients.Add(portion.Key, 0);
            //        }

            //}
        }

        public CofeeMachine(IList<Drink> drinks, IDictionary<Ingredient, Tank> availableIngredients) : this(drinks)
        {
            AvailableIngredients = availableIngredients;
            foreach (var (drink, portion) in AvailableDrinks.Where(drink => drink.Ingredients != null)
                .SelectMany(drink => drink.Ingredients
                .Where(portion => !AvailableIngredients.ContainsKey(portion.Key))
                .Select(portion => (drink, portion))))
            {
                throw new InvalidOperationException($"Ingredient {portion.Key} in {drink.Name} doesn't match this machine!");
            }

            //foreach (var drink in AvailableDrinks)
            //{
            //    if (drink.Ingredients != null)
            //        foreach (var portion in drink.Ingredients)
            //        {
            //            if (!AvailableIngredients.ContainsKey(portion.Key))
            //                throw new InvalidOperationException($"Ingredient {portion.Key} in {drink.Name} doesn't match this machine!");
            //        }

            //}
        }
        public void AddIngredient(Ingredient ingredient, double quatity)
        {
            if (AvailableIngredients.ContainsKey(ingredient))
            {
                var tank = AvailableIngredients[ingredient];
                if (tank.max >= tank.current + quatity)
                {
                    tank.current += quatity;
                    AvailableIngredients[ingredient] = tank;
                }
                else
                    throw new InvalidOperationException($"Invalid operation: max quantity of {ingredient} is {tank.max}!!!");
            }
            else
                throw new InvalidOperationException("Wrong ingredient!!!");
        }
        public CupOfDrink MakeDrink(Drink drink)
        {
            if (AvailableDrinks.Contains(drink) && drink.Ingredients != null)
            {
                //check if all drink ingredietns are in machine 
                foreach (var portion in drink.Ingredients)
                {
                    if (AvailableIngredients.TryGetValue(portion.Key, out Tank tank))
                    {
                        if (portion.Value > tank.current) throw new InvalidOperationException($"[{portion.Key} is out]");
                    }
                    else throw new InvalidOperationException($"[{portion.Key} is wrong ingredient]");
                }

                //update state of the machine 
                foreach (var portion in drink.Ingredients)
                {
                    var maxCurrent = AvailableIngredients[portion.Key];
                    maxCurrent.current -= portion.Value;
                    AvailableIngredients[portion.Key] = maxCurrent;

                    SpentIngredients[portion.Key] += portion.Value;
                }

                Console.WriteLine($"Your {drink.Name} is ready");

                return new CupOfDrink(drink);
            }
            else throw new InvalidOperationException();
        }
    }

    public record Drink
    {
        public string Name { get; }

        public IDictionary<Ingredient, double> Ingredients { get; }

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
        public IDictionary<Ingredient, Tank> AvailableIngredients { get; }
        public IDictionary<Ingredient, double> SpentIngredients { get; }

        public void AddIngredient(Ingredient ingredient, double quatity);
        public CupOfDrink MakeDrink(Drink drink);
    }

    public struct Tank
    {
        internal double max;
        internal double current;
    }

    public enum Ingredient
    {
        Milk,
        Water,
        Cofee,
        Sugar
    }

}
