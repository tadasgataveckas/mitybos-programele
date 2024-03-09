using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    public GameObject camera;

    public List<GameObject> segments;

    public List<GameObject> segmentButtons;

    public int currentSegment = 0;

    public void SwitchSegment(int switchTo)
    {
        //teleport camera to position
        camera.transform.position = new Vector3(segments[switchTo].transform.position.x, 0, -10);
        currentSegment = switchTo;
        for(int i = 0; i < segmentButtons.Count;i++)
        {
            segmentButtons[i].GetComponent<ButtonTransitions>().SetSelectedSegment(i == switchTo);
        }
        camera.GetComponent<CameraScroll>().minY = segments[currentSegment].GetComponent<SegmentInformation>().minYScroll;
        camera.GetComponent<CameraScroll>().maxY = segments[currentSegment].GetComponent<SegmentInformation>().maxYScroll;
    }

    void Start()
    {
        SwitchSegment(currentSegment);
    }

}
