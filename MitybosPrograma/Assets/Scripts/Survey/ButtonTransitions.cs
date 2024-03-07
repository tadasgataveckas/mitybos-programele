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
        isSelected = false;
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


    public void SetSelected(bool selectionState)
    {
        if (isSelected && selectionState) {
            isSelected = false; } // Deselect option, clicking on button while it is already selected
        else {
            isSelected = selectionState; }

        if (isSelected)
            buttonImage.color = selectedColor;
        else
            buttonImage.color = originalColor;
    }
}
