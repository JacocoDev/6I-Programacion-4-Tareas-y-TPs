using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("Pantalla / Menú")]
    [SerializeField] private GameObject gameTitleObject;
    [SerializeField] private GameObject startRestartButtonObject;
    [SerializeField] private TMP_Text startRestartButtonText;

    [Header("Combat UI - Objetos generales")]
    [SerializeField] private GameObject boxer1IconObject;
    [SerializeField] private GameObject boxer2IconObject;
    [SerializeField] private GameObject actionListBoxer1Object;
    [SerializeField] private GameObject actionListBoxer2Object;

    [Header("Vida")]
    [SerializeField] private Slider playerHealthBar;
    [SerializeField] private Slider enemyHealthBar;

    [Header("Texto final")]
    [SerializeField] private TMP_Text finalText;

    [Header("Lista de acciones del jugador")]
    [SerializeField] private ActionSlotUI[] playerActionSlots = new ActionSlotUI[3];

    [Header("Lista de acciones del enemigo")]
    [SerializeField] private ActionSlotUI[] enemyActionSlots = new ActionSlotUI[3];

    [Header("Botones de acción")]
    [SerializeField] private ActionButton[] actionButtons;

    private const string StartLabel = "¡Pelear!";
    private const string RestartLabel = "¡Revancha!";

    private void Awake()
    {
        ConfigureSlots(playerActionSlots);
        ConfigureSlots(enemyActionSlots);
        SetStartButtonLabel(StartLabel);
        ShowMenu();
    }

    public void InitializeCombat(Boxer player, Boxer enemy)
    {
        if (playerHealthBar != null)
        {
            playerHealthBar.minValue = 0;
            playerHealthBar.maxValue = player.MaxHealth;
            playerHealthBar.wholeNumbers = true;
            playerHealthBar.value = player.CurrentHealth;
        }

        if (enemyHealthBar != null)
        {
            enemyHealthBar.minValue = 0;
            enemyHealthBar.maxValue = enemy.MaxHealth;
            enemyHealthBar.wholeNumbers = true;
            enemyHealthBar.value = enemy.CurrentHealth;
        }

        RefreshPlayerActionSlots(player.Actions);
        RefreshEnemyActionSlots(enemy.Actions);
    }

    public void ShowMenu()
    {
        SetStartButtonLabel(StartLabel);

        if (gameTitleObject != null)
            gameTitleObject.SetActive(true);

        if (startRestartButtonObject != null)
            startRestartButtonObject.SetActive(true);

        if (finalText != null)
            finalText.gameObject.SetActive(false);

        SetCombatUIActive(false);
    }

    public void ShowGameplayUI()
    {
        if (gameTitleObject != null)
            gameTitleObject.SetActive(false);

        if (startRestartButtonObject != null)
            startRestartButtonObject.SetActive(false);

        if (finalText != null)
            finalText.gameObject.SetActive(false);

        SetCombatUIActive(true);
    }

    public void ShowVictoryScreen()
    {
        ShowEndScreen("VICTORIA", Color.green);
    }

    public void ShowDefeatScreen()
    {
        ShowEndScreen("DERROTA", Color.red);
    }

    public void ShowDrawScreen()
    {
        ShowEndScreen("EMPATE", Color.yellow);
    }

    public void UpdateHealthBars(Boxer player, Boxer enemy)
    {
        if (playerHealthBar != null)
            playerHealthBar.value = player.CurrentHealth;

        if (enemyHealthBar != null)
            enemyHealthBar.value = enemy.CurrentHealth;
    }

    public void RefreshPlayerActionSlots(IReadOnlyList<ActionType> actions)
    {
        RefreshActionSlots(playerActionSlots, actions);
    }

    public void RefreshEnemyActionSlots(IReadOnlyList<ActionType> actions)
    {
        RefreshActionSlots(enemyActionSlots, actions);
    }

    public void SetPlayerSlotExecuting(int index, IReadOnlyList<ActionType> actions)
    {
        SetExecuting(playerActionSlots, actions, index);
    }

    public void SetEnemySlotExecuting(int index, IReadOnlyList<ActionType> actions)
    {
        SetExecuting(enemyActionSlots, actions, index);
    }

    public void SetActionButtonsInteractable(bool value)
    {
        if (actionButtons == null)
            return;

        foreach (ActionButton actionButton in actionButtons)
        {
            if (actionButton != null)
                actionButton.SetInteractable(value);
        }
    }

    public void SetStartButtonLabel(string label)
    {
        if (startRestartButtonText != null)
            startRestartButtonText.text = label;
    }

    private void ShowEndScreen(string message, Color color)
    {
        if (gameTitleObject != null)
            gameTitleObject.SetActive(false);

        if (startRestartButtonObject != null)
            startRestartButtonObject.SetActive(true);

        SetStartButtonLabel(RestartLabel);

        if (finalText != null)
        {
            finalText.gameObject.SetActive(true);
            finalText.text = message;
            finalText.color = color;
        }

        SetCombatUIActive(false);
    }

    private void SetCombatUIActive(bool value)
    {
        if (boxer1IconObject != null)
            boxer1IconObject.SetActive(value);

        if (boxer2IconObject != null)
            boxer2IconObject.SetActive(value);

        if (actionListBoxer1Object != null)
            actionListBoxer1Object.SetActive(value);

        if (actionListBoxer2Object != null)
            actionListBoxer2Object.SetActive(value);

        if (playerHealthBar != null)
            playerHealthBar.gameObject.SetActive(value);

        if (enemyHealthBar != null)
            enemyHealthBar.gameObject.SetActive(value);

        SetButtonsActive(actionButtons, value);

        if (!value)
            SetActionButtonsInteractable(false);
    }

    private void SetButtonsActive(ActionButton[] buttons, bool value)
    {
        if (buttons == null)
            return;

        foreach (ActionButton button in buttons)
        {
            if (button != null)
                button.gameObject.SetActive(value);
        }
    }

    private void ConfigureSlots(ActionSlotUI[] slots)
    {
        if (slots == null)
            return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] != null)
                slots[i].Configure(i + 1);
        }
    }

    private void RefreshActionSlots(ActionSlotUI[] slots, IReadOnlyList<ActionType> actions)
    {
        if (slots == null)
            return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            bool hasAction = actions != null && i < actions.Count;

            if (!hasAction)
            {
                slots[i].SetEmpty();
                continue;
            }

            slots[i].SetPending(actions[i]);
        }
    }

    private void SetExecuting(ActionSlotUI[] slots, IReadOnlyList<ActionType> actions, int activeIndex)
    {
        if (slots == null || actions == null)
            return;

        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i] == null)
                continue;

            if (i >= actions.Count)
            {
                slots[i].SetEmpty();
                continue;
            }

            if (i < activeIndex)
                slots[i].SetCompleted(actions[i]);
            else if (i == activeIndex)
                slots[i].SetActive(actions[i]);
            else
                slots[i].SetPending(actions[i]);
        }
    }
}