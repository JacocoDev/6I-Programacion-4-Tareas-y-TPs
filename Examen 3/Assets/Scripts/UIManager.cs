using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private GameObject menusUI;
    [SerializeField] private GameObject fightUI;
    [SerializeField] private TMP_Text mainText;
    [SerializeField] private TMP_Text menuButtonText;
    [SerializeField] private ActionButton[] actionButtons;

    [Header("Boxers UI")]
    public BoxerUI boxer1UI;
    public BoxerUI boxer2UI;

    public void InitializeCombat(Boxer boxer1, Boxer boxer2)
    {
        ConfigureHealthBar(boxer1UI.healthBar, boxer1);
        ConfigureHealthBar(boxer2UI.healthBar, boxer2);

        ConfigureActionSlots(boxer1UI);
        ConfigureActionSlots(boxer2UI);

        RefreshActionSlots(boxer1UI, boxer1.actions);
        RefreshActionSlots(boxer2UI, boxer2.actions);
    }

    private void ConfigureHealthBar(Slider healthBar, Boxer boxer)
    {
        healthBar.minValue = 0;
        healthBar.maxValue = boxer.maxHealth;
        healthBar.wholeNumbers = true;
        healthBar.value = boxer.CurrentHealth;
    }

    public void UpdateHealthBars(Boxer boxer1, Boxer boxer2)
    {
        boxer1UI.healthBar.value = boxer1.CurrentHealth;
        boxer2UI.healthBar.value = boxer2.CurrentHealth;
    }

    public void RefreshActionButtons(Boxer boxer, bool isResolvingRound, bool gameEnded)
    {
        bool hasTooManyActions = boxer.actions.Count >= 3;
        bool buttonsLocked = gameEnded || isResolvingRound || hasTooManyActions;
        bool dodgeUsed = boxer.HasAction(ActionType.Dodge);

        foreach (ActionButton button in actionButtons)
        {
            button.SetLocked(buttonsLocked);

            if (buttonsLocked)
            {
                continue;
            }

            if (button.actionType == ActionType.Dodge)
            {
                button.SetDodgeUsed(dodgeUsed);
            }
        }
    }

    private void ConfigureActionSlots(BoxerUI boxerUI)
    {
        for (int i = 0; i < boxerUI.actionSlots.Length; i++)
        {
            boxerUI.actionSlots[i].Configure(i + 1);
        }
    }
    

    public void RefreshActionSlots(BoxerUI boxerUI, List<ActionType> actions)
    {
        for (int i = 0; i < boxerUI.actionSlots.Length; i++)
        {
            if (i >= actions.Count)
            {
                boxerUI.actionSlots[i].SetEmpty();
                continue;
            }

            boxerUI.actionSlots[i].SetPending(actions[i]);
        }
    }

    public void SetExecuting(BoxerUI boxerUI, List<ActionType> actions, int activeIndex)
    {
        for (int i = 0; i < boxerUI.actionSlots.Length; i++)
        {
            if (i >= actions.Count)
            {
                boxerUI.actionSlots[i].SetEmpty();
                continue;
            }

            ActionType action = actions[i];

            if (i < activeIndex)
            {
                boxerUI.actionSlots[i].SetCompleted(action);
                continue;
            }

            if (i == activeIndex)
            {
                boxerUI.actionSlots[i].SetActive(action);
                continue;
            }

            boxerUI.actionSlots[i].SetPending(action);
        }
    }

    public void ShowGameplayUI()
    {
        menusUI.SetActive(false);
        fightUI.SetActive(true);
    }

    public void ShowEndScreen(string message, Color color)
    {
        menuButtonText.text = "¡Revancha!";

        menusUI.SetActive(true);
        fightUI.SetActive(false);

        ShowFinalMessage(message, color);
    }

    private void ShowFinalMessage(string message, Color color)
    {
        mainText.gameObject.SetActive(true);
        mainText.text = message;
        mainText.color = color;
    }
}