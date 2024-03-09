using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScroll : MonoBehaviour
{
    public float minY = 0f;
    public float maxY = 10f;
    public float scrollSpeed = 5f;

    private Vector2 touchStartPos;
    private Vector2 touchEndPos;
    private bool isDragging = false;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    touchStartPos = touch.position;
                    isDragging = true;
                    break;

                case TouchPhase.Moved:
                    touchEndPos = touch.position;
                    float deltaY = (touchEndPos.y - touchStartPos.y) * Time.deltaTime * scrollSpeed;
                    Scroll(deltaY);
                    touchStartPos = touchEndPos;
                    break;

                case TouchPhase.Ended:
                    isDragging = false;
                    break;
            }
        }
    }

    void Scroll(float deltaY)
    {
        //Vector3 newPosition = transform.position + new Vector3(0f, deltaY, 0f);
        //Reverse scroll
        Vector3 newPosition = transform.position - new Vector3(0f, deltaY, 0f);
        newPosition.y = Mathf.Clamp(newPosition.y, minY, maxY);
        transform.position = newPosition;
    }
}
