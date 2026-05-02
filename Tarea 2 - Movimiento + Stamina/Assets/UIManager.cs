using UnityEngine;

public class UIManager : MonoBehaviour
{
    public bool paused = false;

    public void Pause()
    {
        paused = !paused;
    }
}
