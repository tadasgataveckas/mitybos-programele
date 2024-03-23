using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class MainManager : MonoBehaviour
{
    public GameObject camera;

    public List<GameObject> segments;

    public List<GameObject> segmentButtons;

    public int currentSegment = 0;

    public DatabaseMethods databaseMethods;

    string constring = "Server=localhost;User ID=root;Password=root;Database=food_db";
    // User survey info

    public TextMeshPro user;
    public TextMeshPro info;

    ClientMethods c = new ClientMethods(new DatabaseMethods());

    public static int id;

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
        user.text = "User: " + c.ReturnUsername(LoginManager.id, constring);
        string userDataString = c.ReturnUserData(LoginManager.id, constring);
        string[] userDataParts = userDataString.Split(';');
        info.text = $"Height: {userDataParts[0]}\n" +
                    $"Weight: {userDataParts[1]}\n" +
                    $"Gender: {userDataParts[2]}\n" +
                    $"Goal: {userDataParts[3]}\n" +
                    $"Physical Activity: {userDataParts[4]}\n" +
                    $"Date of Birth: {userDataParts[5]}\n" +
                    $"Creation Date: {userDataParts[6]}";
    }
}
