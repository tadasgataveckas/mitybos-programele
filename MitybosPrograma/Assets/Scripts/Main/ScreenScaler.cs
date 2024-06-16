using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenScaler : MonoBehaviour
{
    [SerializeField] private RectTransform Header;
    [SerializeField] private RectTransform Footer;
    private RectTransform rectTransform;
    // Start is called before the first frame update
    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        SetScreenOffset();        
    }

    // Update is called once per frame
    void SetScreenOffset()
    {
        rectTransform.offsetMax = new Vector2(rectTransform.offsetMin.x, -Header.rect.height);
        rectTransform.offsetMin = new Vector2(rectTransform.offsetMin.x, Footer.rect.height);
    }
}
