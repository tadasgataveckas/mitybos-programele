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
    public List<GameObject> segments;
    public List<GameObject> segmentButtons;
    public int currentSegment = 0;

    ClientMethods c = new ClientMethods(new DatabaseMethods());

    // user_data object meant for storing and retrieving info
    private int id_user;
    //private UserData userData;
    public static UserCalories userCalories;

    //level_coins object meant for storing and retrieving info
    private LevelCoins levelCoins;
    public TextMeshProUGUI currLevel;
    public TextMeshProUGUI currCoins;
    public TextMeshProUGUI currStreak;
    private string today = DateTime.Now.ToString("yyyy-MM-dd");

    //GAME DESCRIPTION THINGS
    public Button VitaminHarvestButton;
    public Button MagicExpeditionButton;
    public Button ChickenWingsButton;
    public Button FoodHeistButton;
    public Button AsteroidRushButton;
    public TextMeshProUGUI descr;

    void Awake()
    {
        CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
        CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

        SceneSetUp();
    }

    public void SceneSetUp()
    {
        id_user = SessionManager.GetIdKey();

        // returns user to login screen if this scene is accessed without an id
        if (id_user <= 0)
        {
            GoToLogin();
            return;
        }
        GetAllUserData();
        SwitchSegment(currentSegment);
        GameDescr();
    }

    private void OnEnable()
    {
        UpdateInfo();
        UpdateUserDisplay();
    }

    public void GetAllUserData()
    {
        // retrieves level_coins into object
        levelCoins = new LevelCoins(id_user);
        levelCoins.SynchData();

        userCalories = new UserCalories(id_user);
        userCalories.SynchData();
    }

    public void UpdateUserDisplay()
    {
        //LEVEL COINS STUFF
        UpdateLevelXPCoins();
        currLevel.text = (levelCoins.xp / 100).ToString();
        currCoins.text = levelCoins.coins.ToString();
        currStreak.text = levelCoins.streak.ToString();
    }

    public void UpdateInfo()
    {
        userCalories.SynchData();
        levelCoins.SynchData();
    }

    public void UpdateLevelXPCoins()
    {
        c.UpdateUserLevel(id_user, levelCoins.xp / 100);

        int streakCount = 0;
        string currentStreakDay = YearMonthDay();
        currentStreakDay = DateTime.Now.ToString("yyyy-MM-dd");

        float currCalories = c.GetTotalKcalFromDate(id_user, DateTime.Today.ToString("yyyy-MM-dd"));
        if (currCalories >= userCalories.calories && currentStreakDay == today)
        {
            streakCount++;
            c.UpdateUserStreak(id_user, streakCount);
        }
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
}
