using TMPro;
using UnityEngine;

public class InGameTimer : MonoBehaviour
{
    public TextMeshProUGUI textField;
    private float gameTime = 0;

    // Update is called once per frame
    void Update()
    {
        gameTime += Time.deltaTime;

        int minutes = (int)(gameTime / 60);
        int seconds = (int)(gameTime % 60);

        textField.text = string.Format("{0:D2}:{1:D2}", minutes, seconds);
    }

    public int GetGameTimeMinutes()
    {
        return (int) (gameTime / 60);
    }
}
