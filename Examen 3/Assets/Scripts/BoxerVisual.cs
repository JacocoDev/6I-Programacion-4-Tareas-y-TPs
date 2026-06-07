using UnityEngine;
using UnityEngine.UI;

public class BoxerVisual : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Image imageComponent;
    
    [Header("Sprites")]
    [SerializeField] private Sprite introSprite;
    [SerializeField] private Sprite idleSprite;
    [SerializeField] private Sprite punchSprite;
    [SerializeField] private Sprite blockSprite;
    [SerializeField] private Sprite dodgeSprite;
    [SerializeField] private Sprite hookSprite;
    [SerializeField] private Sprite hitSprite;
    [SerializeField] private Sprite winSprite;
    [SerializeField] private Sprite loseSprite;


    public void SetIntro()
    {
        ApplySprite(introSprite);
    }

    public void SetIdle()
    {
        ApplySprite(idleSprite);
    }

    public void SetAction(ActionType currentAction, ActionType previousAction)
    {
        switch ((currentAction, previousAction))
        {
            case (ActionType.Punch, ActionType.Dodge):
                ApplySprite(hookSprite);
                break;

            case (ActionType.Punch, _):
                ApplySprite(punchSprite);
                break;

            case (ActionType.Block, _):
                ApplySprite(blockSprite);
                break;

            case (ActionType.Dodge, _):
                ApplySprite(dodgeSprite);
                break;
        }
    }

    public void SetHit()
    {
        ApplySprite(hitSprite);
    }

    public void SetDodge()
    {
        ApplySprite(dodgeSprite);
    }

    public void SetWin()
    {
        ApplySprite(winSprite);
    }

    public void SetLose()
    {
        ApplySprite(loseSprite);
    }

    private void ApplySprite(Sprite sprite)
    {
        imageComponent.sprite = sprite;
    }
}