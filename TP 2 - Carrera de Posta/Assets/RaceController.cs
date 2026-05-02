using System.Collections.Generic;
using UnityEngine;

public class RaceControler : MonoBehaviour
{
    [SerializeField] UIController UIController;

    [SerializeField] List<Transform> bases;
    [SerializeField] List<Runner> runners;

    [SerializeField] int laps;

    bool areRunnersSet;
    bool raceRunning;
    int activeRunnerIndex = -1;
    int moveIndex;
    int totalMoves;

    void Update()
    {
        UpdateStepsUI();

        if (!areRunnersSet && AllRunnersArrived())
        {
            areRunnersSet = true;
            UIController.EnableRaceUI(true);
        }

        if (!raceRunning || activeRunnerIndex < 0 || activeRunnerIndex >= runners.Count) return;

        if (!runners[activeRunnerIndex].hasArrive) return;

        moveIndex++;

        if (moveIndex >= totalMoves)
        {
            FinishRace();
            return;
        }

        StartMove(moveIndex);
    }

    void UpdateStepsUI()
    {
        for (int i = 0; i < runners.Count; i++)
        {
            UIController.UpdateStepsText(runners[i].Index, runners[i].Steps);
        }
    }

    public void SetRunners()
    {
        UIController.EnableSetButton(false);
        UIController.EnableRaceUI(false);

        areRunnersSet = false;
        raceRunning = false;
        activeRunnerIndex = -1;
        moveIndex = 0;

        for (int i = 0; i < runners.Count; i++)
        {
            runners[i].Init(i);
            runners[i].Move(runners[i].transform, bases[i], 1f);
        }
    }

    public void Race()
    {
        if (!areRunnersSet || laps <= 0) return;

        UIController.Racing(false, 0f);

        raceRunning = true;
        moveIndex = 0;
        totalMoves = runners.Count * runners.Count * laps;

        StartMove(moveIndex);
    }

    bool AllRunnersArrived()
    {
        for (int i = 0; i < runners.Count; i++)
        {
            if (!runners[i].hasArrive) return false;
        }
        return true;
    }

    void FinishRace()
    {
        raceRunning = false;
        areRunnersSet = false;
        activeRunnerIndex = -1;

        UIController.Racing(true, 1f);
    }

    void StartMove(int step)
    {
        int count = runners.Count;
        int lapIndex = (step / count) % count;
        int runnerIndex = (count - lapIndex + (step % count)) % count;
        int baseIndex = (runnerIndex + 1 + lapIndex) % bases.Count;

        activeRunnerIndex = runnerIndex;

        runners[runnerIndex].Move(
            runners[runnerIndex].transform,
            bases[baseIndex],
            UIController.GetSpeed()
        );
    }
}