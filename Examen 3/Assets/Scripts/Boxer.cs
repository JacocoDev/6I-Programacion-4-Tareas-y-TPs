using System.Collections.Generic;
using UnityEngine;

public class Boxer : MonoBehaviour
{
    public List<ActionType> actions = new List<ActionType>();

    public int maxHealth = 20;
    public int CurrentHealth;

    private void Awake()
    {
        ResetBoxer();
    }

    public void ResetBoxer()
    {
        CurrentHealth = maxHealth;
        ClearActions();
    }

    public bool AddAction(ActionType action)
    {
        if ((actions.Count >= 3) || (action == ActionType.Dodge && HasAction(ActionType.Dodge)))
            return false;

        actions.Add(action);
        return true;
    }

    public bool RemoveLastActionOfType(ActionType action)
    {
        for (int i = actions.Count - 1; i >= 0; i--)
        {
            if (actions[i] == action)
            {
                actions.RemoveAt(i);
                return true;
            }
        }

        return false;
    }

    public bool HasAction(ActionType action)
    {
        return actions.Contains(action);
    }

    public void ClearActions()
    {
        actions.Clear();
    }

    public void TakeDamage(int amount)
    {
        CurrentHealth = Mathf.Max(0, CurrentHealth - amount);
    }

    public bool IsDead()
    {
        return CurrentHealth <= 0;
    }
}