using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonTransitions : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private bool isSelected;

    public Color originalColor;
    public Color hoveredColor;
    public Color selectedColor;

    private Image buttonImage;

    void Start()
    {
        buttonImage = GetComponent<Image>(); // Assuming the button has an Image component
        buttonImage.color = originalColor; // Set the initial color
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //if (!isSelected)
            buttonImage.color = hoveredColor; // Change color when button is pressed
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isSelected)
            buttonImage.color = originalColor; // Change color back to original when button is released
    }

    // You may have a method to set the button as selected, where you change the color to 'selectedColor'
    public void SetSelected()
    {
        isSelected = !isSelected;
        if (isSelected)
            buttonImage.color = selectedColor;
        else
            buttonImage.color = originalColor;
    }
}
