using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Bytescout.Spreadsheet;
using System.Reflection.Metadata;

class Program
{
    // Imported "Bytescout.Spreadsheet NuGet package"

    // output in \converter\CSV_to_SQL\CSV_to_SQL\bin\Debug\net6.0\food_data.sql

    // if data input isn't being updated - try deleting food_data.xlsx file in:
    // \converter\CSV_to_SQL\CSV_to_SQL\bin\Debug\net6.0\food_data.xlsx

    const string INPUT_FILE = "food_data.xlsx";
    const string OUTPUT_FILE = "food_data.sql";
    const int MAX_ENTRIES = 10000;

    static void Main(string[] args)
    {
        if (File.Exists(OUTPUT_FILE))
            File.Delete(OUTPUT_FILE);

        Spreadsheet spreadsheet = new Spreadsheet();
        spreadsheet.LoadFromFile(INPUT_FILE);
        Worksheet worksheet = spreadsheet.Workbook.Worksheets[0];

        ClearData(OUTPUT_FILE);
        AddProducts(OUTPUT_FILE, worksheet);
        AddMeals(OUTPUT_FILE, worksheet);
        AddMealsFromProducts(OUTPUT_FILE, worksheet);
        AddAllergies(OUTPUT_FILE, worksheet);
        AddAllergiesFromProducts(OUTPUT_FILE, worksheet);
        Console.WriteLine("Done!");
    }



    static void ClearData(string file)
    {
        using (StreamWriter writer = new StreamWriter(file, append: true))
        {
            string line = 
                "DELETE FROM meals_from_products;\n" +
                "DELETE FROM allergies_from_products;\n" +
                "DELETE FROM meal;\n" +
                "DELETE FROM product;\n" +
                "DELETE FROM allergy;";

            writer.WriteLine(line);
            Console.WriteLine("Cleared earlier data");
        }
    }

    static void AddProducts(string file, Worksheet worksheet)
    {
        using (StreamWriter writer = new StreamWriter(file, append: true))
        {
            int i;
            for (i = 1; i < MAX_ENTRIES; i++)
            {
                if (worksheet.Cell(i, 0).ToString() == "" ||
                    worksheet.Cell(i, 1).ToString() == "" ||
                    worksheet.Cell(i, 2).ToString() == "" ||
                    worksheet.Cell(i, 3).ToString() == "" ||
                    worksheet.Cell(i, 4).ToString() == "" ||
                    worksheet.Cell(i, 5).ToString() == "")
                    break;

                string id = worksheet.Cell(i, 0).Value.ToString();
                string name = worksheet.Cell(i, 1).Value.ToString();
                string kcal = worksheet.Cell(i, 2).Value.ToString();
                string protein = worksheet.Cell(i, 3).Value.ToString();
                string fat = worksheet.Cell(i, 4).Value.ToString();
                string carbohydrates = worksheet.Cell(i, 5).Value.ToString();

                string line = ("insert into product (id_product, product_name, kcal, protein, fat, carbohydrates) values (" +
                    id + ", '" + name + "', " + kcal + ", " + protein + ", " + fat + ", " + carbohydrates + ");");

                writer.WriteLine(line);
            }
            Console.WriteLine("Added " + i.ToString() + " products");
        }
    }

    static void AddMeals(string file, Worksheet worksheet)
    {
        using (StreamWriter writer = new StreamWriter(file, append: true))
        {
            int i;
            for (i = 1; i < MAX_ENTRIES; i++)
            {
                if (worksheet.Cell(i, 16).ToString() == "" ||
                    worksheet.Cell(i, 17).ToString() == "" ||
                    worksheet.Cell(i, 18).ToString() == "" ||
                    worksheet.Cell(i, 19).ToString() == "")
                    break;

                string id = worksheet.Cell(i, 16).Value.ToString();
                string name = worksheet.Cell(i, 17).Value.ToString();
                string type = worksheet.Cell(i, 18).Value.ToString();
                string servings = worksheet.Cell(i, 19).Value.ToString();

                string line = ("insert into meal (id_meal, meal_name, meal_type, servings) values (" +
                    id + ", '" + name + "', '" + type + "', " + servings + ");");

                writer.WriteLine(line);
            }
            Console.WriteLine("Added " + i.ToString() + " meals");
        }
    }

    static void AddMealsFromProducts(string file, Worksheet worksheet)
    {
        using (StreamWriter writer = new StreamWriter(file, append: true))
        {
            int i;
            for (i = 1; i < MAX_ENTRIES; i++)
            {
                if (worksheet.Cell(i, 8).ToString() == "" ||
                    worksheet.Cell(i, 9).ToString() == "" ||
                    worksheet.Cell(i, 11).ToString() == "")
                    break;

                string id_meal = worksheet.Cell(i, 8).Value.ToString();
                string id_product = worksheet.Cell(i, 9).Value.ToString();
                string amount = worksheet.Cell(i, 11).Value.ToString();

                string line = ("insert into meals_from_products (id_meal, id_product, amount) values (" +
                    id_meal + ", " + id_product + ", " + amount + ");");

                writer.WriteLine(line);
            }
            Console.WriteLine("Added " + i.ToString() + " product - meal links");
        }
    }

    static void AddAllergies(string file, Worksheet worksheet)
    {
        using (StreamWriter writer = new StreamWriter(file, append: true))
        {
            int i;
            for (i = 1; i < MAX_ENTRIES; i++)
            {
                if (worksheet.Cell(i, 23).ToString() == "" ||
                    worksheet.Cell(i, 24).ToString() == "")
                    break;

                string id_allergy = worksheet.Cell(i, 23).Value.ToString();
                string allergy_name = worksheet.Cell(i, 24).Value.ToString();

                string line = ("insert into allergy (id_allergy, allergy_name) values (" +
                    id_allergy + ", '" + allergy_name + "');");

                writer.WriteLine(line);
            }
            Console.WriteLine("Added " + i.ToString() + " allergies");
        }
    }

    static void AddAllergiesFromProducts(string file, Worksheet worksheet)
    {
        using (StreamWriter writer = new StreamWriter(file, append: true))
        {
            int i;
            for (i = 1; i < MAX_ENTRIES; i++)
            {
                if (worksheet.Cell(i, 27).ToString() == "" ||
                    worksheet.Cell(i, 28).ToString() == "")
                    break;

                string id_allergy = worksheet.Cell(i, 27).Value.ToString();
                string id_product = worksheet.Cell(i, 28).Value.ToString();

                string line = ("insert into allergies_from_products (id_allergy, id_product) values (" +
                    id_allergy + ", " + id_product + ");");

                writer.WriteLine(line);
            }
            Console.WriteLine("Added " + i.ToString() + " allergies from products");
        }
    }
}