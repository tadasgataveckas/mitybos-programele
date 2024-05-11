using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private GridLayoutGroup container;
    [SerializeField] private TextMeshProUGUI text_element;

    public TextMeshProUGUI AddScoreItem(string text)
    {
        TextMeshProUGUI newTextElement = Instantiate(text_element, container.transform);
        newTextElement.text = text;

        return newTextElement;
    }

    /// <summary>
    /// Highlights selected item based on index
    /// </summary>
    /// <param name="number">Index must be between 0 and 9</param>
    public void HighlightElement(int number)
    {
        if (number < 0 || number > 9)
            return;

        try
        {
            Transform highlightContainer = this.transform.Find("Highlight (container)");
            Transform highlight = highlightContainer.transform.Find(number.ToString());
            UnityEngine.UI.Image highlightImage = highlight.GetComponent<UnityEngine.UI.Image>();
            highlightImage.color = new Color32(140, 183, 22, 255);
        }
        catch (Exception e) { System.Console.WriteLine(e.Message); }
    }
}
