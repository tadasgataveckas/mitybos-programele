public class Allergy
{
    public int id_allergy;
    public string name { get; }

    public Allergy(int id_allergy)
    {
        this.id_allergy = id_allergy;
        name = ReturnAllergyName(id_allergy);
    }
    public static string ReturnAllergyName(int id)
    {
        string name;
        switch (id)
        {
            case 1:
                name = "Milk";
                break;
            case 2:
                name = "Eggs";
                break;
            case 3:
                name = "Tree nuts";
                break;
            case 4:
                name = "Peanuts";
                break;
            case 5:
                name = "Shellfish";
                break;
            case 6:
                name = "Wheat";
                break;
            case 7:
                name = "Soy";
                break;
            case 8:
                name = "Fish";
                break;
            case 9:
                name = "Sesame";
                break;
            case 10:
                name = "Vegetarian";
                break;
            case 11:
                name = "Vegan";
                break;
                // added just for omnivore tag 
            case 0:
                name = "Omnivore";
                break;
            default:
                name = "";
                break;
        }
        return name;
    }
}
