using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodClass
{   
    public string Name { get; set; }
    public int Calories { get; set; }
    /*    public int Protein { get; set; }
        public int Carbohydrates { get; set; }
        public int Fat { get; set; } */

    public FoodClass(string name, int calories /*, int protein, int carbohydrates, int fat*/)
    {
        Name = name;
        Calories = calories;
        /* Protein = protein;
         Carbohydrates = carbohydrates;
         Fat = fat; */
    }
}
