using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;
using TMPro;
using System.Collections;

public class ButtonObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Enter");
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Exit");
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("DebugName");

        if (eventData.button == PointerEventData.InputButton.Left) // LMB
        {
        }
        if (eventData.button == PointerEventData.InputButton.Right) // RMB
        {
        }
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        //Debug.Log("DebugName2");
        if (eventData.button == PointerEventData.InputButton.Left) // LMB
        {
          
        }
    }
    public void DebugLog()
    {
        Debug.Log("DebugName");
    }

    void Start()
    {
        //Debug.Log("Start");
    }
}
