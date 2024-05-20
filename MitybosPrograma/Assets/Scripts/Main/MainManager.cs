using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Google.Protobuf.WellKnownTypes;

public class MainManager : MonoBehaviour
{
	//PROGRES BAR STUFF DONT DELEETE
	public ProgressBar progressBar_instance;
	//PROGRES BAR STUFF DONT DELEETE


	public GameObject camera;
    public List<GameObject> segments;
    public List<GameObject> segmentButtons;
    public int currentSegment = 0;

    // Current User survey info in the edit fields
    public TextMeshProUGUI user;
    public TextMeshProUGUI info;

    public TextMeshProUGUI currHeight;
    public TextMeshProUGUI currWeight;
    public TMP_Dropdown currGender;
    public TMP_Dropdown currGoal;
    public TMP_Dropdown currPA;
    public TextMeshProUGUI currBirth;
    public TMP_Dropdown currAllergies;
    public TextMeshProUGUI allergySettingDisplay;
    public TMP_Dropdown currPreferences;
    public TextMeshProUGUI dailyCalories;
    public TextMeshProUGUI errorData;
    public GameObject editSettings;

    ClientMethods c = new ClientMethods(new DatabaseMethods());

    // user_data object meant for storing and retrieving info
    private int id_user;
    private UserData userData;
    private string username;
    private List<int> newAllergies;
    public static UserCalories userCalories;

    //level_coins object meant for storing and retrieving info
    private LevelCoins levelCoins;
    public TextMeshProUGUI currLevel;
    private bool isXpVisible = false;
    public TextMeshProUGUI currXp;
    public TextMeshProUGUI currCoins;
    public TextMeshProUGUI currStreak;

    int year;

    public FoodSearch food;
    public float currCalories;

    //GAME DESCRIPTION THINGS
    public Button VitaminHarvestButton;
    public Button MagicExpeditionButton;
    public Button ChickenWingsButton;
    public Button FoodHeistButton;
    public Button AsteroidRushButton;
    public TextMeshProUGUI descr;

    void Start()
    {
        Debug.Log("Iskviestas");
        SceneSetUp();
        StartCoroutine(RepeatSceneSetup());
    }
    private IEnumerator RepeatSceneSetup()
    {
        yield return new WaitForSeconds(0.0001f);
        SceneSetUp();
    }

    public void SceneSetUp()
    {
        Debug.Log("Iskviestas");
        // gets stored user id
        id_user = SessionManager.GetIdKey();

        // returns user to login screen if this scene is accessed without an id
        if (id_user <= 0)
        {
            GoToLogin();

            // idk if other methods run after scene change, so I put this here just in case
            return;
        }
        //Not showing xp at first
        currXp.gameObject.SetActive(false);

        GetAllUserData();
        SwitchSegment(currentSegment);

        GameDescr();


        UpdateUserDisplay();
    }


    public void GetAllUserData()
    {
        // retrieves user_data into object
        userData = new UserData(id_user);
        userData.SynchData();

        // retrieves level_coins into object
        levelCoins = new LevelCoins(id_user);
        levelCoins.SynchData();

        userCalories = new UserCalories(id_user);
        userCalories.SynchData();

        username = c.ReturnUsername(id_user);

        newAllergies = c.GetAllUserAllergies(id_user);

        food.GetComponent<FoodSearch>();
        currCalories = food.ReturnTotalKcal();

        Debug.Log(userData.ToString());
    }

    public void UpdateUserDisplay()
    {
        errorData.text = "";
        //Debug.Log("Year is: " + Year());
        info.text = $"Height: {userData.height}\n" +
                    $"Weight: {userData.weight}\n" +
                    $"Gender: {userData.GetGenderString()}\n" +
                    $"Goal: {userData.GetGoalString()}\n" +
                    $"Physical Activity: {userData.physical_activity}\n" +
                    $"Date of Birth: {userData.date_of_birth}\n" +
                    $"Creation date: {userData.creation_date.Substring(0, 9)}\n" +
                    $"BMI: {userCalories.bmi}\n" +
                    $"Daily Calories: {userCalories.calories}";

        currHeight.text = userData.height.ToString();
        currWeight.text = userData.weight.ToString();
        currBirth.text = userData.date_of_birth;
        currGender.value = (int)userData.gender - 1;
        currGoal.value = (int)userData.goal - 1;
        currPA.value = (int)userData.physical_activity - 1;

        if (newAllergies.Contains(10))
            currPreferences.value = 1;
        else if (newAllergies.Contains(11))
            currPreferences.value = 2;
        else
            currPreferences.value = 0;

        foreach (int allergy in newAllergies)
            if (allergy < 10)
                currAllergies.options[allergy - 1].text = "O - " + Allergy.ReturnAllergyName(allergy);

        UnselectDropdown();
        UpdateAllergyDisplay();

        currCalories = food.ReturnTotalKcal();
        dailyCalories.text = currCalories + " / " + userCalories.calories + "cal";

		//PROGRES BAR STUFF DONT DELEETE
		progressBar_instance.max = userCalories.calories;
        progressBar_instance.UpdateCurr();

        //LEVEL COINS STUFF
        UpdateLevelXPCoins();
        currLevel.text = (levelCoins.xp / 100).ToString();
        currCoins.text = levelCoins.coins.ToString();
        currXp.text = levelCoins.xp.ToString();
        currStreak.text = levelCoins.streak.ToString();

        


        user.text = "User: " + username;
    }

    public void UpdateInfo()
    {
        c.UpdateUserData(userData);
        userCalories.SynchData();

        c.DeleteUserAllergies(userData.id_user);
        foreach (int allergy in newAllergies)
            c.InsertUserAllergy(userData.id_user, allergy);
    }

    public void UpdateLevelXPCoins()
    {
        c.UpdateUserLevel(id_user, levelCoins.xp / 100);
        //if (levelCoins.xp >= 100)
        //{
        //    c.UpdateUserLevel(id_user, 1);
        //}
        //if(levelCoins.xp >= 190)
        //{
        //    c.UpdateUserLevel(id_user, 2);
        //}
        //if (levelCoins.xp >= 350)
        //{
        //    c.UpdateUserLevel(id_user, 3);
        //}

        if(currCalories >= userCalories.calories)
        {
            c.UpdateUserStreak(id_user, 1);
        }
       

        //if ((currCalories - userCalories.calories) < 200)
        //{
        //    c.UpdateUserCoins(id_user, 100);
        //}
        //else if((currCalories - userCalories.calories) > 500)
        //{
        //    c.UpdateUserCoins(id_user, 50);
        //}
    }

    public void GameDescr()
    {
        descr.text = "Coming soon";
        // Add listeners to the buttons
        VitaminHarvestButton.onClick.AddListener(() => ShowDescription("Vitamin Harvest - It's an adventurous style game that involves knowledge and quick thinking. " +
            "You'll be placed in nature, growing fruits for customers. As more customers arrive, you'll need to grow more fruits. Don't be sleepy, work like a professional!"));
        MagicExpeditionButton.onClick.AddListener(() => ShowDescription("Magic Expedition is a thrilling survival game where you battle against waves of junk and unhealthy food. " +
            "The longer you play, the more bad food you’ll face. Armed with your trusty talisman, you must be brave and break all the bones of junk food! " +
            "Stay vigilant, keep your health up, and prove that you have what it takes to survive the Magic Expedition!"));
        ChickenWingsButton.onClick.AddListener(() => ShowDescription("Chicken Wings is an exciting game, where you hop through various platforms with your talisman. " +
            "As you jump higher, you'll encounter different types of platforms, each with its own unique challenges. " +
            "Reach new heights to arrive at special stations where you can eat fruits, learn about their health benefits and discover the power of nutritious fruits in Chicken Wings!"));
        FoodHeistButton.onClick.AddListener(() => ShowDescription("Coming soon...  ;)"));
        AsteroidRushButton.onClick.AddListener(() => ShowDescription("Coming soon...  ;)"));
    }
    void ShowDescription(string description)
    {
        descr.text = description;
    }

    public void ShowXP()
    {
        if (isXpVisible)
        {

            currXp.gameObject.SetActive(false);
            isXpVisible = false;
        }
        else
        {
            currXp.gameObject.SetActive(true);
            isXpVisible = true;
        }
    }

    public void SwitchSegment(int switchTo)
    {
        //teleport camera to position
        //camera.transform.position = new Vector3(segments[switchTo].transform.position.x, 0, -10);

        //changed for canvas
        for (int i = 0; i < segments.Count; i++)
        {
            segments[i].SetActive(i == switchTo); // Turns on chosen segment, turns off other segments
        }

        currentSegment = switchTo;
        for (int i = 0; i < segmentButtons.Count; i++)
        {
            segmentButtons[i].GetComponent<ButtonTransitions>().SetSelectedSegment(i == switchTo);
        }
        //camera.GetComponent<CameraScroll>().minY = segments[currentSegment].GetComponent<SegmentInformation>().minYScroll;
        //camera.GetComponent<CameraScroll>().maxY = segments[currentSegment].GetComponent<SegmentInformation>().maxYScroll;
    }

    public void InputHeight(string newHeight)
    {
        if (double.TryParse(newHeight, out userData.height))
        {
            //Debug.Log("Edited height is: " + userData.height);
        }
    }

    public void InputWeight(string newWeight)
    {
        if (double.TryParse(newWeight, out userData.weight))
        {
            //Debug.Log("Edited weight is: " + userData.weight);
        }
    }

    public void InputGender(int val)
    {
        userData.gender = (UserData.Gender)(val + 1);
        //Debug.Log("Edited gender is: " + userData.gender);
    }

    public void InputGoal(int val1)
    {
        userData.goal = (UserData.Goal)(val1 + 1);
        //Debug.Log("Edited goal is: " + userData.goal);
    }

    public void InputActivity(int val2)
    {
        userData.physical_activity = (val2 + 1);
        //Debug.Log("Edited physical activity is: " + userData.physical_activity);
    }

    public void InputBirth(string newBirth)
    {
        userData.date_of_birth = newBirth;
        //Debug.Log("Edited birth is: " + userData.date_of_birth);
    }

    public void InputAllergy(int val)
    {
        // added to escape self call (9 is non existent item acting as null)
        if (val >= 9)
            return;

        UnselectDropdown();

        // ++, cuz dropdown values start at 0
        val = val + 1;

        if (newAllergies.Contains(val))
        {
            newAllergies.Remove(val);
            currAllergies.options[val - 1].text = Allergy.ReturnAllergyName(val);
        }
        else
        {
            newAllergies.Add(val);
            currAllergies.options[val - 1].text = "O - " + Allergy.ReturnAllergyName(val);
        }

        UpdateAllergyDisplay();
    }

    private void UpdateAllergyDisplay()
    {
        allergySettingDisplay.text = "";
        foreach (int i in newAllergies)
            if (i < 10)
                allergySettingDisplay.text = allergySettingDisplay.text + Allergy.ReturnAllergyName(i) + ", ";

        if (allergySettingDisplay.text.Length > 0)
            allergySettingDisplay.text = allergySettingDisplay.text.Substring(0, allergySettingDisplay.text.Length - 2);
    }

    public void InputPreference(int val)
    {
        switch (val)
        {
            case 1:
                newAllergies.Remove(11);
                newAllergies.Add(10);
                break;
            case 2:
                newAllergies.Remove(10);
                newAllergies.Add(11);
                break;
            default:
                newAllergies.Remove(10);
                newAllergies.Remove(11);
                break;
        }
    }

    // this is added cuz you can't select "nothing", and input allergy triggers
    // on value "changed" and not "selected"
    private void UnselectDropdown()
    {
        currAllergies.options.Add(new TMP_Dropdown.OptionData());
        currAllergies.value = 10;
        currAllergies.options.RemoveAt(9);
    }

    /// <summary>
    /// Transferring from birth date to year
    /// </summary>
    /// <returns></returns>
    public int Year()
    {
        // Konvertuojame string'ą į DateTime objektą
        DateTime dataObj;
        if (DateTime.TryParseExact(userData.date_of_birth, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dataObj))
        {
            // Ištraukiame metus
            year = dataObj.Year;

            // Spausdiname metus
            //Debug.Log("Konvertuoti metai: " + year);
        }
        else
        {
            Debug.Log("Please write your birth date in correct form! (yyyy-MM-dd)");
        }
        return year;
    }

    private void GoToLogin()
    {
        SceneManager.LoadScene("Login");
    }

    public void LogOut()
    {
        SessionManager.CloseSession();
        GoToLogin();
    }

    public void SubmitChanges()
    {
        DateTime dataObj;

        if (250 <= userData.height || userData.height <= 120)
        {
            errorData.text = "Height is invalid";
            return;
        }
        else if (350 <= userData.weight || userData.weight <= 30)
        {
            errorData.text = "Weight is invalid";
            return;
        }
        else if (!DateTime.TryParseExact(userData.date_of_birth, "yyyy-MM-dd", null, System.Globalization.DateTimeStyles.None, out dataObj))
        {
            errorData.text = "Please write your birth date in correct form! (yyyy-MM-dd)";
            return;
        }

        editSettings.SetActive(false);
        UpdateInfo();
        UpdateUserDisplay();
    }
}
