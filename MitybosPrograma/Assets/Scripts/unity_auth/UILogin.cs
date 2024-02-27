using Unity.Services.Authentication;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UILogin : MonoBehaviour
{
    [SerializeField] private Button loginButton;
    [SerializeField] private TMP_Text userIdText;
    [SerializeField] private Transform loginPanel, userPanel;
    [SerializeField] private LoginController loginController;

    private void OnEnable()
    {
        loginButton.onClick.AddListener(LoginButtonPresed);
        loginController.OnSignedIn += LoginController_OnSignedIn;
    }

    public void LoginController_OnSignedIn(PlayerInfo playerInfo, string playerName)
    {
        loginPanel.gameObject.SetActive(false);
        userPanel.gameObject.SetActive(true);
        userIdText.text = $"id_{playerInfo.Id}";
        Debug.Log(playerName);
        //playerInfo.name
    }

    private async void LoginButtonPresed()
    {
        await loginController.InitSignIn();
    }

    private void OnDisable()
    {
        loginButton.onClick.RemoveListener(LoginButtonPresed);
        loginController.OnSignedIn -= LoginController_OnSignedIn;
    }


}
