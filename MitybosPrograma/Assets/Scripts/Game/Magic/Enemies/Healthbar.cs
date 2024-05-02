using UnityEngine;
using UnityEngine.UI;

public class Healthbar : MonoBehaviour
{
    public Slider healthBar;
    public ME_Entity entity;

    // Start is called before the first frame update
    void Start()
    {
        healthBar.maxValue = entity.MaxHP;
        UpdateHealthbar();
    }

    public void UpdateHealthbar()
    {
        Debug.Log("UPDATING HEALTH");
        healthBar.value = entity.HP;

        //if (healthBar.value >= healthBar.maxValue || healthBar.value <= 0)
        //    gameObject.SetActive(false);
        //else gameObject.SetActive(true);

    }
}
