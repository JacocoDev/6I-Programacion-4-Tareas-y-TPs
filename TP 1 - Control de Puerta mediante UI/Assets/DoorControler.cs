using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DoorControler : MonoBehaviour
{
    public Door door;

    public TMP_Text textButton;
    
    public bool DoorClosed = true;
    
    void Start()
    {
        DoorClosed = true;
    }

    public void OpenOrCloseDoor()
    {
        if (DoorClosed == true)
        {
            door.OpenDoor();
            DoorClosed = false;
            textButton.text = "Close";
        }
        else
        {
            door.CloseDoor();
            DoorClosed = true;
            textButton.text = "Open";
        }
    }
}