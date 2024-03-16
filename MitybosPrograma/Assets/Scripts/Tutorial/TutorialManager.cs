using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialManager : MonoBehaviour
{
    public GameObject TutorialSegment;
    private bool TutorialOn = false;
    public List<Sprite> TutorialSprites;
    public List<Image> DotSprites;
    public List<string> TutorialTexts;
    public Image TutorialImage;
    public TextMeshProUGUI TutorialText;

    public Color DotActiveColor;
    public Color DotNotActiveColor;

    //public Image InfoButton;
    //public Color InfoLoginColor;
    //public Color InfoTutorialColor;

    private int currentImage = 0;


    public void SwitchTutorialImage(int newImageIndex) 
    {
        currentImage = newImageIndex;
        TutorialImage.sprite = TutorialSprites[currentImage];
        for(int i = 0; i < DotSprites.Count; i++) 
        {
            if (i == currentImage) {
                DotSprites[i].color = DotActiveColor;
                    }
            else
            {
                DotSprites[i].color = DotNotActiveColor;
            }
        }
        TutorialText.text = TutorialTexts[currentImage];
    }
    public void NextImage() 
    {
        currentImage++;
        currentImage = Mathf.Clamp(currentImage, 0, TutorialSprites.Count-1);
        SwitchTutorialImage(currentImage);
    }
    public void PreviousImage()
    {
        currentImage--;
        currentImage = Mathf.Clamp(currentImage, 0, TutorialSprites.Count-1);
        SwitchTutorialImage(currentImage);
    }

    void Start()
    {
        SwitchTutorialImage(currentImage);
    }

    public void TurnOnTutorial() 
    {
        TutorialOn = !TutorialOn;
        TutorialSegment.SetActive(TutorialOn);
        SwitchTutorialImage(0);
    }
}
