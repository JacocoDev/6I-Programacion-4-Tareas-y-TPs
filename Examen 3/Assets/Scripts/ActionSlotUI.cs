using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ActionSlotUI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private TMP_Text slotText;
    [SerializeField] private Image background;

    [Header("Colores")]
    [SerializeField] private Color emptyColor = new Color(0.12f, 0.12f, 0.12f);

    [SerializeField] private Color completedColor = new Color(0.32f, 0.32f, 0.32f);

    [SerializeField] private Color pendingColor = new Color(0.65f, 0.65f, 0.65f);

    [SerializeField] private Color activeColor = Color.white;

    private int slotNumber;

    private void Awake()
    {
        if (background == null)
            background = GetComponent<Image>();

        if (slotText == null)
            slotText = GetComponentInChildren<TMP_Text>(true);
    }

    public void Configure(int number)
    {
        slotNumber = number;
        SetEmpty();
    }

    public void SetEmpty()
    {
        SetVisual(
            emptyColor,
            $"{slotNumber}°"
        );
    }

    public void SetPending(ActionType action)
    {
        SetVisual(
            pendingColor,
            $"{slotNumber}° {GetActionName(action)}"
        );
    }

    public void SetActive(ActionType action)
    {
        SetVisual(
            activeColor,
            $"{slotNumber}° {GetActionName(action)}"
        );
    }

    public void SetCompleted(ActionType action)
    {
        SetVisual(
            completedColor,
            $"{slotNumber}° {GetActionName(action)}"
        );
    }

    private void SetVisual(Color backgroundColor, string text)
    {
        if (background != null)
            background.color = backgroundColor;

        if (slotText != null)
        {
            slotText.text = text;
            slotText.color = Color.black;
        }
    }

    private string GetActionName(ActionType action)
    {
        return action switch
        {
            ActionType.Punch => "Pegar",
            ActionType.Block => "Bloquear",
            ActionType.Dodge => "Esquivar",
            _ => "Acción"
        };
    }
}