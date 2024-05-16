using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ME_PowerUpHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image image;
    private Color original;

    void Awake()
    {
        original = image.color;
    }

    public void SelectPowerUp()
    {
        ME_Game_Manager manager = FindObjectOfType<ME_Game_Manager>();
        manager.PickUpgrade(GetComponent<ME_Upgrade>());
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = new Color(1, 0.8f, 0);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = original;
    }
}
