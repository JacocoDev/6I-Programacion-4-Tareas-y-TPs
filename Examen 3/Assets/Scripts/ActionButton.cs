using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ActionButton : MonoBehaviour, IPointerClickHandler
{
    [Header("Referencias")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private Image fillImage;

    public ActionType actionType;

    private bool isLocked;
    private bool isDodgeUsed;

    private float feedbackUntil;

    private Color baseColor = new Color(1f, 1f, 1f);
    private Color disabledColor = new(0.32f, 0.32f, 0.32f);

    private void Update()
    {
        RefreshColor();
    }

    public void SetLocked(bool value)
    {
        isLocked = value;

        if (value)
            isDodgeUsed = false;
    }

    public void SetDodgeUsed(bool value)
    {
        if (actionType == ActionType.Dodge)
            isDodgeUsed = value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isLocked)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (actionType == ActionType.Dodge && isDodgeUsed)
                return;

            feedbackUntil = Time.time + 0.1f;
            audioManager.PlayUIClick();
            gameManager.AddAction(actionType);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (actionType == ActionType.Dodge && !isDodgeUsed)
                return;

            feedbackUntil = Time.time + 0.1f;
            audioManager.PlayUIClick();
            gameManager.RemoveAction(actionType);
        }
    }

    private void RefreshColor()
    {
        bool showFeedback = Time.time < feedbackUntil;

        if (showFeedback || isLocked || (actionType == ActionType.Dodge && isDodgeUsed))
        {
            fillImage.color = disabledColor;
        }
        else
        {
            fillImage.color = baseColor;
        }
    }
}