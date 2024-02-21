using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string input = "food_data.csv";
        string output = "food_data.sql";

        List<Food> foodlist = File.ReadAllLines(input)
                                       .Skip(1)
                                       .Select(v => Food.FromCsv(v))
                                       .ToList();

        if (File.Exists(output))
            File.Delete(output);

        using (StreamWriter outputFile = new StreamWriter(Path.Combine(output)))
        {
            foreach (Food food in foodlist)
            {
                //insert into product (id_product, product_name, kcal, protein, fat, carbohydrates) values (1, 'Sapphire', 1, 1, 1, 1);
                string line = ("insert into product (id_product, product_name, kcal, protein, fat, carbohydrates) values (" +
                    food.id + ", '" + food.name + "', " + food.kcal + ", " + food.protein + ", " + food.fat + ", " + food.carbohydrates + ");");

                Console.WriteLine(line);
                outputFile.WriteLine(line);
            }
        }
        Console.WriteLine("Done!");
    }
}

class Food
{
    public int id;
    public string name;
    public decimal kcal;
    public decimal protein;
    public decimal fat;
    public decimal carbohydrates;

    public static Food FromCsv(string csvLine)
    {
        string[] values = csvLine.Split(',');
        Food foodValues = new Food();
        foodValues.id = Convert.ToInt32(values[0]);
        foodValues.name = Convert.ToString(values[1]);
        foodValues.kcal = Convert.ToDecimal(values[2]);
        foodValues.protein = Convert.ToDecimal(values[3]);
        foodValues.fat = Convert.ToDecimal(values[4]);
        foodValues.carbohydrates = Convert.ToDecimal(values[5]);

        //Console.WriteLine(Convert.ToInt32(values[0]));

        return foodValues;
    }
}