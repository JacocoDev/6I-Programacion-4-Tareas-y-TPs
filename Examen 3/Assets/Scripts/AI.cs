using UnityEngine;

public class AI : MonoBehaviour
{
    [SerializeField] private Boxer enemyBoxer;

    public void GenerateActions()
    {
        if (enemyBoxer == null)
        {
            Debug.LogError("AI: Falta asignar enemyBoxer.");
            return;
        }

        enemyBoxer.ClearActions();

        for (int i = 0; i < 3; i++)
        {
            ActionType randomAction = (ActionType)Random.Range(0, 3);
            enemyBoxer.AddAction(randomAction);
        }
    }
}