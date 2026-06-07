using UnityEngine;

public class AI : MonoBehaviour
{
    [Header("Referencias")]
    [SerializeField] private Boxer boxer2;

    public ActionType[] WeightedActions =
    {
        ActionType.Punch,
        ActionType.Punch,
        ActionType.Block,
        ActionType.Dodge
    };

    public void GenerateActions()
    {
        boxer2.ClearActions();

        while (boxer2.actions.Count < 3)
        {
            ActionType action = WeightedActions[Random.Range(0, WeightedActions.Length)];

            if (action == ActionType.Dodge && boxer2.HasAction(ActionType.Dodge))
                continue;

            boxer2.AddAction(action);
        }
    }
}