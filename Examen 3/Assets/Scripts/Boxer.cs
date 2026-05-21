using System.Collections.Generic;
using UnityEngine;

public class Boxer : MonoBehaviour
{
    [Header("Vida")]
    [SerializeField] private int maxHealth = 100;

    [Header("Acciones")]
    [SerializeField] private List<ActionType> actions = new List<ActionType>();

    public int MaxHealth => maxHealth;
    public int CurrentHealth { get; private set; }
    public int ActionCount => actions.Count;
    public IReadOnlyList<ActionType> Actions => actions;

    private void Awake()
    {
        ResetBoxer();
    }

    public void ResetBoxer()
    {
        CurrentHealth = maxHealth;
        actions.Clear();
    }

    public bool AddAction(ActionType action)
    {
        if (actions.Count >= 3)
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