using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScrollerPrefab : MonoBehaviour
{
    public int startIndex = 1;
    public int endIndex = 10;
    public int defaultValue = 1;
    private float currentValue;

    // added at the end of item display
    public string flavorText = "";

    [SerializeField] private GridLayoutGroup container;
    //[SerializeField] private RectTransform scroller;
    [SerializeField] private TextMeshProUGUI text_element;

    // Start is called before the first frame update
    void Start()
    {
        RefreshScroller();
    }

    public void RefreshScroller()
    {

        // checking index range validity
        if (startIndex > endIndex)
            endIndex = startIndex + 1;

        // checking default value validity
        if (defaultValue < startIndex || defaultValue > endIndex)
            defaultValue = startIndex;

        RectTransform R = container.GetComponent<RectTransform>();
        GridLayoutGroup G = container.GetComponent<GridLayoutGroup>();

        // sets scroller height
        R.sizeDelta = new Vector2(R.sizeDelta.x, G.cellSize.y *
            ((Mathf.Abs(startIndex - endIndex) + 1)) + G.cellSize.y / 2);

        // sets default scroller position
        SetDefaultValue(defaultValue);
        PopulateScroller();
        SetCurrentValue();
    }


    // populate scroller with items
    public void PopulateScroller()
    {

        foreach (Transform child in container.transform)
        {
            Destroy(child.gameObject);
        }

        for (int i = startIndex; i <= endIndex; i++)
        {
            TextMeshProUGUI item = Instantiate(text_element, container.transform);
            item.text = i.ToString() + flavorText;
        }
    }

    // set
    public void SetCurrentValue()
    {
        float value = (int)(container.transform.localPosition.y /
            container.GetComponent<GridLayoutGroup>().cellSize.y + startIndex);

        // check for when scrolling outside of bounds
        if (value < startIndex)
            currentValue = startIndex;
        else if (value > endIndex)
            currentValue = endIndex;
        else
            currentValue = value;
    }

    // get
    public float GetValue()
    {
        Debug.Log("Value: " + currentValue);
        return currentValue;
    }

    public void SetDefaultValue(int value)
    {
        RectTransform R = container.GetComponent<RectTransform>();
        GridLayoutGroup G = container.GetComponent<GridLayoutGroup>();

        defaultValue = value;
        R.anchoredPosition3D = new Vector2(R.anchoredPosition3D.x, G.cellSize.y * (defaultValue - startIndex));

        SetCurrentValue();
    }
}
