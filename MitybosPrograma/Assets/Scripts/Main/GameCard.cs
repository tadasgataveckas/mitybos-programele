using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class GameCard : MonoBehaviour //, IPointerClickHandler, IPointerDownHandler, IPointerExitHandler
{
    public Color gameColor;
    public string gameName;
    public GameObject gameSelectObject;
    public TextMeshProUGUI gameSelectTitle;
    public Image gameSelectBackground;
    /*private SpriteRenderer sprite;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        sprite.color = new Color(0.8f, 0.8f, 0.8f);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        sprite.color = Color.white;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        sprite.color = Color.white;
        gameSelectObject.SetActive(true);
        gameSelectTitle.text = gameName;
        gameSelectBackground.color = gameColor;
    }*/
    public void OnCardClick() {
        gameSelectObject.SetActive(true);
        gameSelectTitle.text = gameName;
        gameSelectBackground.color = gameColor;
    }
}
