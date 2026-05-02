using UnityEngine;

public class Door : MonoBehaviour
{
    private Vector3 ClosedPos = new Vector3(0, 0, 0);
    private Vector3 OpenedPos = new Vector3(2, 0, 0);

    public float speed = 2f;

    private Vector3 targetPos;

    void Start()
    {
        transform.position = ClosedPos;
        targetPos = ClosedPos;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, targetPos, speed * Time.deltaTime);
    }

    public void OpenDoor()
    {
        targetPos = OpenedPos;
    }

    public void CloseDoor()
    {
        targetPos = ClosedPos;
    }
}