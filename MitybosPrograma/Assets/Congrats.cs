using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Congrats : MonoBehaviour
{
    ClientMethods c = new ClientMethods(new DatabaseMethods());
    public TextMeshProUGUI user;
    private string username;
    private int id_user;
    // Start is called before the first frame update
    void Start()
    {
        id_user = SessionManager.GetIdKey();
        // returns user to login screen if this scene is accessed without an id
        if (id_user <= 0)
        {
            GoToLogin();

            // idk if other methods run after scene change, so I put this here just in case
            return;
        }
        
        UpdateData();
    }
    public void UpdateData()
    {      
        username = c.ReturnUsername(id_user);
        user.text = username;
    }
    private void GoToLogin()
    {
        SceneManager.LoadScene("Login");
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    public void GoToMain()
    {
        //Go to Main scene
        SceneManager.LoadScene("Main");
    }
}
