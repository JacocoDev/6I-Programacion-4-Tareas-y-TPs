using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Boxer boxer1;
    [SerializeField] private Boxer boxer2;
    [SerializeField] private AI AI;
    [SerializeField] private UIManager UIManager;
    [SerializeField] private AudioManager audioManager;

    [Header("Sprites de boxeadores")]
    [SerializeField] private BoxerVisual boxer1Visual;
    [SerializeField] private BoxerVisual boxer2Visual;

    private int punchDamage = 2;
    private float spriteTime = 0.75f;

    private bool isResolvingRound;
    private bool gameEnded;
    private int currentTurn;
    private float roundTimer;

    private RoundPhase roundPhase = RoundPhase.None;

    private enum RoundPhase
    {
        None,
        ShowingAction,
        ShowingImpact
    }

    private void Start()
    {
        boxer1.ResetBoxer();
        boxer2.ResetBoxer();

        ResetRoundState();

        boxer1Visual.SetIntro();
        boxer2Visual.SetIntro();
    }

    private void Update()
    {
        if (!isResolvingRound || gameEnded || roundPhase == RoundPhase.None)
            return;

        roundTimer -= Time.deltaTime;

        if (roundTimer <= 0f)
            AdvanceRoundFlow();
    }

    public void OnMenuButtonPressed()
    {
        audioManager.PlayUIClick();
        StartGame();
    }

    private void StartGame()
    {
        boxer1.ResetBoxer();
        boxer2.ResetBoxer();

        ResetRoundState();

        UIManager.ShowGameplayUI();
        UIManager.InitializeCombat(boxer1, boxer2);
        UIManager.RefreshActionButtons(boxer1, isResolvingRound, gameEnded);

        boxer1Visual.SetIdle();
        boxer2Visual.SetIdle();
    }

    private void ResetRoundState()
    {
        gameEnded = false;
        StopRound();
    }

    private void StopRound()
    {
        isResolvingRound = false;
        currentTurn = 0;
        roundTimer = 0f;
        roundPhase = RoundPhase.None;
    }

    public void AddAction(ActionType action)
    {
        if (!CanAddActions() || !boxer1.AddAction(action))
            return;

        UIManager.RefreshActionSlots(UIManager.boxer1UI, boxer1.actions);
        UIManager.RefreshActionButtons(boxer1, isResolvingRound, gameEnded);

        if (boxer1.actions.Count >= 3)
            StartRoundResolution();
    }

    public void RemoveAction(ActionType action)
    {
        if (!CanAddActions() || !boxer1.RemoveLastActionOfType(action))
            return;

        UIManager.RefreshActionSlots(UIManager.boxer1UI, boxer1.actions);
        UIManager.RefreshActionButtons(boxer1, isResolvingRound, gameEnded);
    }

    private bool CanAddActions()
    {
        return !gameEnded && !isResolvingRound;
    }

    private void StartRoundResolution()
    {
        if (isResolvingRound || gameEnded)
            return;

        isResolvingRound = true;
        UIManager.RefreshActionButtons(boxer1, isResolvingRound, gameEnded);

        AI.GenerateActions();

        UIManager.RefreshActionSlots(UIManager.boxer1UI, boxer1.actions);
        UIManager.RefreshActionSlots(UIManager.boxer2UI, boxer2.actions);

        currentTurn = 0;
        roundPhase = RoundPhase.ShowingAction;
        roundTimer = spriteTime;

        BeginTurn();
    }

    private void AdvanceRoundFlow()
    {
        if (roundPhase == RoundPhase.ShowingAction)
        {
            ResolveCurrentTurn();
            roundPhase = RoundPhase.ShowingImpact;
            roundTimer = spriteTime;
            return;
        }

        if (roundPhase != RoundPhase.ShowingImpact)
            return;

        if (CheckGameEnd())
            return;

        currentTurn++;

        if (currentTurn >= 3)
        {
            EndRound();
            return;
        }

        BeginTurn();
        roundPhase = RoundPhase.ShowingAction;
        roundTimer = spriteTime;
    }

    private void BeginTurn()
    {
        ActionType boxer1Action = GetCurrentAction(boxer1);
        ActionType boxer2Action = GetCurrentAction(boxer2);
        ActionType boxer1PreviousAction = GetPreviousAction(boxer1);
        ActionType boxer2PreviousAction = GetPreviousAction(boxer2);

        UIManager.SetExecuting(UIManager.boxer1UI, boxer1.actions, currentTurn);
        UIManager.SetExecuting(UIManager.boxer2UI, boxer2.actions, currentTurn);

        boxer1Visual.SetAction(boxer1Action, boxer1PreviousAction);
        boxer2Visual.SetAction(boxer2Action, boxer2PreviousAction);
    }

    private void ResolveCurrentTurn()
    {
        ActionType boxer1Action = GetCurrentAction(boxer1);
        ActionType boxer2Action = GetCurrentAction(boxer2);
        ActionType boxer1PreviousAction = GetPreviousAction(boxer1);
        ActionType boxer2PreviousAction = GetPreviousAction(boxer2);

        CombatSystem.ResolveTurn(boxer1Action, boxer1PreviousAction, boxer2Action, boxer2PreviousAction, boxer1, boxer2, punchDamage, out bool boxer1TookDamage, out bool boxer2TookDamage);

        if (boxer1TookDamage || boxer2TookDamage)
        {
            audioManager.PlayPunch();
        }

        if ((boxer1Action == ActionType.Punch && boxer2Action == ActionType.Punch) && (boxer1TookDamage || boxer2TookDamage))
        {
            audioManager.PlayHit();
        }

        UIManager.UpdateHealthBars(boxer1, boxer2);

        bool bothArePunch = boxer1Action == ActionType.Punch && boxer2Action == ActionType.Punch;
        bool boxer1KeepsDodgePose = KeepsDodgePose(boxer1);
        bool boxer2KeepsDodgePose = KeepsDodgePose(boxer2);

        ApplyPostTurnVisual(boxer1Visual, bothArePunch, boxer1TookDamage, boxer1KeepsDodgePose);
        ApplyPostTurnVisual(boxer2Visual, bothArePunch, boxer2TookDamage, boxer2KeepsDodgePose);
    }

    private void ApplyPostTurnVisual(BoxerVisual visual, bool bothArePunch, bool tookDamage, bool keepsDodgePose)
    {
        if (bothArePunch && tookDamage)
        {
            visual.SetHit();
        }
        else if (keepsDodgePose)
        {
            visual.SetDodge();
        }
        else
        {
            visual.SetIdle();
        }
    }

    private void EndRound()
    {
        boxer1.ClearActions();
        boxer2.ClearActions();

        UIManager.RefreshActionSlots(UIManager.boxer1UI, boxer1.actions);
        UIManager.RefreshActionSlots(UIManager.boxer2UI, boxer2.actions);

        boxer1Visual.SetIdle();
        boxer2Visual.SetIdle();

        StopRound();
        UIManager.RefreshActionButtons(boxer1, isResolvingRound, gameEnded);
    }

    private bool CheckGameEnd()
    {
        bool boxer1Dead = boxer1.IsDead();
        bool boxer2Dead = boxer2.IsDead();

        if (!boxer1Dead && !boxer2Dead)
            return false;

        audioManager.PlayBell();
        gameEnded = true;
        StopRound();
        UIManager.RefreshActionButtons(boxer1, isResolvingRound, gameEnded);

        if (boxer1Dead && boxer2Dead)
        {
            boxer1Visual.SetLose();
            boxer2Visual.SetLose();
            UIManager.ShowEndScreen("EMPATE", Color.yellow);
            return true;
        }

        if (boxer2Dead)
        {
            boxer1Visual.SetWin();
            boxer2Visual.SetLose();
            UIManager.ShowEndScreen("VICTORIA", Color.green);
            return true;
        }

        boxer1Visual.SetLose();
        boxer2Visual.SetWin();
        UIManager.ShowEndScreen("DERROTA", Color.red);
        return true;
    }

    private ActionType GetCurrentAction(Boxer boxer)
    {
        return boxer.actions[currentTurn];
    }

    private ActionType GetPreviousAction(Boxer boxer)
    {
        int index = Mathf.Max(0, currentTurn - 1);
        return boxer.actions[index];
    }

    private bool KeepsDodgePose(Boxer boxer)
    {
        if (GetCurrentAction(boxer) != ActionType.Dodge)
            return false;

        if (currentTurn >= boxer.actions.Count - 1)
            return false;

        return boxer.actions[currentTurn + 1] == ActionType.Punch;
    }
}