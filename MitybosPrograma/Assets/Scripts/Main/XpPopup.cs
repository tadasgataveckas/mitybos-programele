using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class XpPopup : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject Popup;
    [SerializeField] private TextMeshProUGUI XpText;

    // Start is called before the first frame update
    void Start()
    {
        Popup.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Popup.gameObject.SetActive(true);

        int id_user = SessionManager.GetIdKey();
        LevelCoins levelCoins = new LevelCoins(id_user);
        levelCoins.SynchData();
        XpText.SetText("XP: " + levelCoins.xp);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Popup.gameObject.SetActive(false);
    }
}
