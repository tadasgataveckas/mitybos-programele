using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentManager : MonoBehaviour
{
    public bool listValueSegment; //allergies
    public List<GameObject> selectableButtons;

    public void ChangeSelectedButton(GameObject pressedButton) 
    {
        if (!listValueSegment) 
        {
            foreach(GameObject button in selectableButtons) 
            {
                if (button == pressedButton)
                {
                    button.GetComponent<ButtonTransitions>().SetSelectedOption(true);
                }
                else 
                {
                    button.GetComponent<ButtonTransitions>().SetSelectedOption(false);
                }
            }
        }
        else { pressedButton.GetComponent<ButtonTransitions>().SetSelectedOption(true); }
        
    }
}
