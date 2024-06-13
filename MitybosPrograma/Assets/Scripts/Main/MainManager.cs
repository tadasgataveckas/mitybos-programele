using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Globalization;
//using Google.Protobuf.WellKnownTypes;

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
    public TextMeshProUGUI dailyCalories;
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
    public string today = DateTime.Now.ToString("yyyy-MM-dd");

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
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

        Debug.Log("Iskviestas");
        SceneSetUp();
        StartCoroutine(RepeatSceneSetup());

        //YearScroller.defaultValue = Convert.ToInt32(userData.date_of_birth);
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
        int streakCount = 0;
        string currentStreakDay = YearMonthDay();
        Debug.Log("currentStreakDay: " + currentStreakDay);
        currentStreakDay = DateTime.Now.ToString("yyyy-MM-dd");
        Debug.Log("Last day: " + today);
        if (currCalories >= userCalories.calories && currentStreakDay == today)
        {
            streakCount++;
            Debug.Log("streakCount: " + streakCount);
            c.UpdateUserStreak(id_user, streakCount);
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

    public string YearMonthDay()
    {
        // Konvertuojame string'ą į DateTime objektą
        DateTime dataObj;
        if (DateTime.TryParseExact(levelCoins.last_streak_day, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out dataObj))
        {
            // Ištraukiame metus, mėnesį ir dieną
            string yearMonthDay = dataObj.ToString("yyyy-MM-dd");

            // Spausdiname metus, mėnesį ir dieną
            // Debug.Log("Konvertuoti metai, mėnuo ir diena: " + yearMonthDay);
            return yearMonthDay;           
        }
        else
        {
            Debug.Log("Form isnt correct (yyyy-MM-dd)");
            return null;
        }
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
    }

    private void GoToLogin()
    {
        SceneManager.LoadScene("Login");
    }

    public void SubmitChanges()
    {
        editSettings.SetActive(false);
        UpdateInfo();
        UpdateUserDisplay();
    }
}
