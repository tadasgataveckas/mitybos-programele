using System;
using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider healthBar;
    public ME_Entity entity;
    private CanvasGroup canvas;

    private void Awake()
    {
        canvas = GetComponent<CanvasGroup>();
    }

    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = entity.MaxHP;
        UpdateSlider();
    }

    public void UpdateSlider()
    {
        try
        {
            if (entity.HP >= 0)
                healthBar.value = entity.HP;
            else
                healthBar.value = 0;

            // hides when at full or 0 health
            if (healthBar.value >= healthBar.maxValue || healthBar.value < 0)
                canvas.alpha = 0f;
            else
                canvas.alpha = 1f;
        }
        catch (Exception e)
        {
            Debug.Log(e);
        }
    }
}
