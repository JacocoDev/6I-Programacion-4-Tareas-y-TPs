using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionSlotUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TMP_Text slotText;
    [SerializeField] private Image background;

    private Color emptyColor = new Color(0.12f, 0.12f, 0.12f);
    private Color completedColor = new Color(0.32f, 0.32f, 0.32f);
    private Color pendingColor = new Color(0.64f, 0.64f, 0.64f);
    private Color activeColor = new Color(1f, 1f, 1f);

    private Dictionary<ActionType, string> ActionNames = new()
    {
        { ActionType.Punch, "Pegar" },
        { ActionType.Block, "Bloquear" },
        { ActionType.Dodge, "Esquivar" }
    };

    private int slotNumber;

    public void Configure(int number)
    {
        slotNumber = number;
        SetEmpty();
    }

    public void SetEmpty()
    {
        SetVisual(emptyColor, $"{slotNumber}°");
    }

    public void SetPending(ActionType action)
    {
        SetVisual(pendingColor, $"{slotNumber}° {GetActionName(action)}");
    }

    public void SetActive(ActionType action)
    {
        SetVisual(activeColor, $"{slotNumber}° {GetActionName(action)}");
    }

    public void SetCompleted(ActionType action)
    {
        SetVisual(completedColor, $"{slotNumber}° {GetActionName(action)}");
    }

    private void SetVisual(Color backgroundColor, string text)
    {
        background.color = backgroundColor;
        slotText.text = text;
    }

    private string GetActionName(ActionType action)
    {
        return ActionNames.GetValueOrDefault(action, "Acción");
    }
}