using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class BoxerUI
{
    public Slider healthBar;
    public GameObject iconObject;
    public GameObject actionListObject;
    public ActionSlotUI[] actionSlots = new ActionSlotUI[3];
}