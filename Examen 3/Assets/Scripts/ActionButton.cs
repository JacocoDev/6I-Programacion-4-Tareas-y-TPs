using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ActionButton : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private ActionType actionType;
    [SerializeField] private GameManager gameManager;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void SetInteractable(bool value)
    {
        if (button != null)
            button.interactable = value;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameManager == null || button == null || !button.interactable)
            return;

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            gameManager.AddAction(actionType);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            gameManager.RemoveAction(actionType);
        }
    }
}