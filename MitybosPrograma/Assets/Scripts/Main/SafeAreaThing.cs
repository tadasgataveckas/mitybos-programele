using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeAreaThing : MonoBehaviour
{
    [SerializeField] private bool topAdjusted = true;
    [SerializeField] private bool bottomAdjusted = true;

    void Start()
    {
        RectTransform rectum = GetComponent<RectTransform>();

        Rect safeArea = Screen.safeArea;
        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;
        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // top
        if (topAdjusted)
            rectum.anchorMax = anchorMax;

        // bottom
        if (bottomAdjusted)
            rectum.anchorMin = anchorMin;
    }
}
