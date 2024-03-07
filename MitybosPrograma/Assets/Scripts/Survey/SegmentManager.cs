using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SegmentManager : MonoBehaviour
{
    public bool listValueSegment; //allergies
    public List<GameObject> selectableButtons;

    public void ChangeSelectedButton(GameObject pressedButton) 
    {
        Debug.Log(pressedButton);
        if (!listValueSegment) 
        {
            foreach(GameObject button in selectableButtons) 
            {
                if (button == pressedButton)
                {
                    button.GetComponent<ButtonTransitions>().SetSelected(true);
                }
                else 
                {
                    button.GetComponent<ButtonTransitions>().SetSelected(false);
                }
            }
        }
        else { pressedButton.GetComponent<ButtonTransitions>().SetSelected(true); }
        
    }
}
