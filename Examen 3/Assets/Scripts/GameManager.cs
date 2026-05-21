using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Boxer playerBoxer;
    [SerializeField] private Boxer enemyBoxer;
    [SerializeField] private AI ai;
    [SerializeField] private UIManager uiManager;

    [Header("Combate")]
    [SerializeField] private int punchDamage = 10;
    [SerializeField] private int blockDamage = 5;
    [SerializeField] private float turnDelay = 0.8f;

    private bool isResolvingRound;
    private bool gameEnded;
    private Coroutine roundCoroutine;

    private void Start()
    {
        ResetBattleState();
        if (uiManager != null)
            uiManager.ShowMenu();
    }

    public void OnStartRestartPressed()
    {
        StartGame();
    }

    public void AddAction(ActionType action)
    {
        if (gameEnded || isResolvingRound || playerBoxer == null || uiManager == null)
            return;

        bool added = playerBoxer.AddAction(action);
        if (!added)
            return;

        uiManager.RefreshPlayerActionSlots(playerBoxer.Actions);
        UpdateButtonState();

        if (playerBoxer.ActionCount >= 3)
            StartRoundResolution();
    }

    public void RemoveAction(ActionType action)
    {
        if (gameEnded || isResolvingRound || playerBoxer == null || uiManager == null)
            return;

        bool removed = playerBoxer.RemoveLastActionOfType(action);
        if (!removed)
            return;

        uiManager.RefreshPlayerActionSlots(playerBoxer.Actions);
        UpdateButtonState();
    }

    private void StartGame()
    {
        if (roundCoroutine != null)
        {
            StopCoroutine(roundCoroutine);
            roundCoroutine = null;
        }

        ResetBattleState();

        gameEnded = false;
        isResolvingRound = false;

        if (uiManager != null)
        {
            uiManager.ShowGameplayUI();
            uiManager.InitializeCombat(playerBoxer, enemyBoxer);
            uiManager.SetActionButtonsInteractable(true);
        }
    }

    private void ResetBattleState()
    {
        if (playerBoxer != null)
            playerBoxer.ResetBoxer();

        if (enemyBoxer != null)
            enemyBoxer.ResetBoxer();

        if (uiManager != null && playerBoxer != null && enemyBoxer != null)
            uiManager.InitializeCombat(playerBoxer, enemyBoxer);
    }

    private void UpdateButtonState()
    {
        if (uiManager == null || playerBoxer == null)
            return;

        bool enableButtons = !gameEnded && !isResolvingRound && playerBoxer.ActionCount < 3;
        uiManager.SetActionButtonsInteractable(enableButtons);
    }

    private void StartRoundResolution()
    {
        if (isResolvingRound || gameEnded)
            return;

        isResolvingRound = true;
        UpdateButtonState();

        if (ai != null)
        {
            ai.GenerateActions();

            if (uiManager != null && enemyBoxer != null)
                uiManager.RefreshEnemyActionSlots(enemyBoxer.Actions);
        }
        else
        {
            Debug.LogError("GameManager: Falta asignar AI.");
        }

        if (roundCoroutine != null)
            StopCoroutine(roundCoroutine);

        roundCoroutine = StartCoroutine(ResolveRoundCoroutine());
    }

    private IEnumerator ResolveRoundCoroutine()
    {
        for (int i = 0; i < 3; i++)
        {
            if (playerBoxer == null || enemyBoxer == null || uiManager == null)
                yield break;

            ActionType playerAction = playerBoxer.Actions[i];
            ActionType enemyAction = enemyBoxer.Actions[i];

            uiManager.SetPlayerSlotExecuting(i, playerBoxer.Actions);
            uiManager.SetEnemySlotExecuting(i, enemyBoxer.Actions);

            ResolveTurn(playerAction, enemyAction);

            uiManager.UpdateHealthBars(playerBoxer, enemyBoxer);

            yield return new WaitForSeconds(turnDelay);

            if (CheckGameEnd())
                yield break;
        }

        if (!gameEnded)
        {
            playerBoxer.ClearActions();
            enemyBoxer.ClearActions();

            uiManager.RefreshPlayerActionSlots(playerBoxer.Actions);
            uiManager.RefreshEnemyActionSlots(enemyBoxer.Actions);

            isResolvingRound = false;
            UpdateButtonState();
        }
    }

    private void ResolveTurn(ActionType playerAction, ActionType enemyAction)
    {
        bool playerPunch = playerAction == ActionType.Punch;
        bool enemyPunch = enemyAction == ActionType.Punch;

        if (playerPunch && enemyPunch)
        {
            playerBoxer.TakeDamage(punchDamage);
            enemyBoxer.TakeDamage(punchDamage);
            return;
        }

        if (playerPunch)
        {
            DealDamageToEnemy(enemyAction);
            return;
        }

        if (enemyPunch)
        {
            DealDamageToPlayer(playerAction);
        }
    }

    private void DealDamageToEnemy(ActionType enemyAction)
    {
        switch (enemyAction)
        {
            case ActionType.Block:
                enemyBoxer.TakeDamage(blockDamage);
                break;

            case ActionType.Dodge:
                break;

            case ActionType.Punch:
                enemyBoxer.TakeDamage(punchDamage);
                break;
        }
    }

    private void DealDamageToPlayer(ActionType playerAction)
    {
        switch (playerAction)
        {
            case ActionType.Block:
                playerBoxer.TakeDamage(blockDamage);
                break;

            case ActionType.Dodge:
                break;

            case ActionType.Punch:
                playerBoxer.TakeDamage(punchDamage);
                break;
        }
    }

    private bool CheckGameEnd()
    {
        bool playerDead = playerBoxer != null && playerBoxer.IsDead();
        bool enemyDead = enemyBoxer != null && enemyBoxer.IsDead();

        if (!playerDead && !enemyDead)
            return false;

        gameEnded = true;
        isResolvingRound = false;

        if (uiManager != null)
            uiManager.SetActionButtonsInteractable(false);

        if (uiManager != null)
        {
            if (playerDead && enemyDead)
                uiManager.ShowDrawScreen();
            else if (enemyDead)
                uiManager.ShowVictoryScreen();
            else
                uiManager.ShowDefeatScreen();
        }

        return true;
    }
}