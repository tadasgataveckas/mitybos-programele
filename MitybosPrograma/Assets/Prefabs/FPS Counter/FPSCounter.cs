using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    public float updateInterval = 0.5f;

    private float accum = 0f;
    private int frames = 0;
    private float timeLeft;

    private TMP_Text fpsText;

    void Start()
    {
        fpsText = GetComponent<TMP_Text>();
        timeLeft = updateInterval;
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;
        accum += Time.timeScale / Time.deltaTime;
        frames++;

        if (timeLeft <= 0.0)
        {
            // Display FPS
            float fps = accum / frames;
            fpsText.text = "FPS: " + fps.ToString("F2");

            // Reset variables
            timeLeft = updateInterval;
            accum = 0f;
            frames = 0;
        }
    }
}
